using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArmModule : Module, IPointerUpHandler, IPointerDownHandler
{
    public enum Direction
    {
        LEFT,
        RIGHT,
    }

    const float OFFSET_DISTANCE = 1f;

    [SerializeField] float armSpeed = 5f;
    [Tooltip("Determines the length of the up/down motion")]
    [SerializeField] [Range(-1, 1)] float cutoff = 0f;
    [SerializeField] SpriteRenderer up;
    [SerializeField] SpriteRenderer down;

    bool? armWasDown = false;
    Direction facing = Direction.LEFT;
    bool wasRecentlyDragged = false;
    Vector3 calculatedFacingVector = Vector3.zero;
    IDictionary<Direction, Vector3> offsetByDirection = new Dictionary<Direction, Vector3>()
    {
        {Direction.LEFT, Vector3.left * OFFSET_DISTANCE},
        {Direction.RIGHT, Vector3.right * OFFSET_DISTANCE}
    };

    bool IsArmDown { get => Mathf.Sin(Time.time * armSpeed) > cutoff; }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!wasRecentlyDragged)
        {
            var currentArmStateDown = IsArmDown;
            if (currentArmStateDown)
            {
                ReleaseNeighboringButton();
            }
            calculatedFacingVector = Vector3.zero;
            facing = facing == Direction.LEFT ? Direction.RIGHT : Direction.LEFT;
        }
    }

    void PressNeighboringButton()
    {
        var neighbor = GetNeighboringModule();
        if (neighbor)
        {
            ButtonModule bm = neighbor.GetComponent<ButtonModule>();
            if (bm)
            {
                bm.Press();
            }
        }
    }

    void ReleaseNeighboringButton()
    {
        var neighbor = GetNeighboringModule();
        if (neighbor)
        {
            ButtonModule bm = neighbor.GetComponent<ButtonModule>();
            if (bm)
            {
                bm.Release();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        wasRecentlyDragged = false;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        armWasDown = null;
        wasRecentlyDragged = true;
        if (IsArmDown)
        {
            ReleaseNeighboringButton();
        }
    }

    void RefreshVisual()
    {
        down.enabled = IsArmDown;
        up.enabled = !down.enabled;
    }

    Collider2D GetNeighboringModule()
    {
        Vector3 positionToCheck = transform.position + offsetByDirection[facing];
        return Physics2D.OverlapPoint(positionToCheck, LayerMask.GetMask("Draggable"));
    }

    void Update()
    {
        if (!IsPowered)
        {
            return;
        }
        var armIsDown = IsArmDown;
        if (!armWasDown.HasValue || armIsDown != armWasDown.Value)
        {
            if (armIsDown)
            {
                PressNeighboringButton();
            }
            else
            {
                ReleaseNeighboringButton();
            }
            RefreshVisual();
        }
        armWasDown = armIsDown;
    }
}
