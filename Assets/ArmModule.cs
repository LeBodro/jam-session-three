using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArmModule : Module, IPointerUpHandler
{
    public enum Direction
    {
        LEFT,
        RIGHT,
    }

    private const float CELL_SIZE = 1f;

    private bool lastArmStateDown = false;

    private Direction facing;
    private Vector3 calculatedFacingVector = Vector3.zero;

    [SerializeField] float armSpeed = 5f;
    // between -1 and +1. Determines the length of the up/down motion
    [SerializeField] float cutoff = 0f;

    void Start()
    {
        lastArmStateDown = false;
        calculatedFacingVector = Vector3.zero;
        facing = Direction.LEFT;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // TODO: Rotate clockwise. Make sure to update the calculated facing vector
        // TODO: Ignore when dragged!!
        // Idea: Keep a flag that is set when dragging starts and is checked when OnPointerUp is called and is reset when OnPointerDown is called
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
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

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
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
        if (!isPowered) {
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
        if (!isPowered) {
            return;
        }
        var currentArmStateDown = GetArmStateDown();
        if(currentArmStateDown != lastArmStateDown)
        {
            var neighbor = GetNeighboringModule();
            if (neighbor)
            {
                ButtonModule bm = neighbor.GetComponent<ButtonModule>();
                if (bm)
                {
                    if (currentArmStateDown)
                    {
                        bm.Press();
                    }
                    else
                    {
                        bm.Release();
                    }
                }
            }
        }

        lastArmStateDown = currentArmStateDown;
    }
}
