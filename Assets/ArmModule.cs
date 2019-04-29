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

    private const float CELL_SIZE = 1f;

    private bool? lastArmStateDown = null;

    private Direction facing;
    private bool wasRecentlyDragged = false;
    private Vector3 calculatedFacingVector = Vector3.zero;

    [SerializeField] float armSpeed = 5f;
    // between -1 and +1. Determines the length of the up/down motion
    [SerializeField] float cutoff = 0f;

    void Start()
    {
        wasRecentlyDragged = false;
        lastArmStateDown = null;
        calculatedFacingVector = Vector3.zero;
        facing = Direction.LEFT;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!wasRecentlyDragged)
        {
            var currentArmStateDown = GetArmStateDown();
            if(currentArmStateDown)
            {
                ReleaseNeighboringButton();
            }
            calculatedFacingVector = Vector3.zero;
            facing = facing == Direction.LEFT ? Direction.RIGHT : Direction.LEFT;
        }
    }
    private void PressNeighboringButton()
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
    private void ReleaseNeighboringButton()
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
        lastArmStateDown = null;
        wasRecentlyDragged = true;
        if(GetArmStateDown())
        {
            ReleaseNeighboringButton();
        }
    }

    Collider2D GetNeighboringModule()
    {
        return Physics2D.OverlapPoint(GetCheckSpot(), LayerMask.GetMask("Draggable"));
    }

    Vector3 GetCheckSpot()
    {
        return transform.position + transform.TransformDirection(GetDirectionalVector()) * CELL_SIZE;
    }

    void OnDrawGizmos()
    {
        if (!IsPowered) {
            return;
        }
        Gizmos.color = GetNeighboringModule() ? Color.green : Color.yellow;
        Gizmos.DrawSphere(GetCheckSpot(), 0.1f);
    }

    private Vector3 GetDirectionalVector()
    {
        if (calculatedFacingVector == Vector3.zero)
        {
            switch (facing)
            {
                case Direction.RIGHT:
                    calculatedFacingVector = Vector3.right;
                    break;
                default:
                case Direction.LEFT:
                    calculatedFacingVector = Vector3.left;
                    break;
            }
        }
        return calculatedFacingVector;
    }

    bool GetArmStateDown()
    {
        return Mathf.Sin(Time.time * armSpeed) > cutoff;
    }

    void Update()
    {
        if (!IsPowered) {
            return;
        }
        var currentArmStateDown = GetArmStateDown();
        if(!lastArmStateDown.HasValue || currentArmStateDown != lastArmStateDown.Value)
        {
            if (currentArmStateDown)
            {
                PressNeighboringButton();
            }
            else
            {
                ReleaseNeighboringButton();
            }
        }
        lastArmStateDown = currentArmStateDown;
    }
}
