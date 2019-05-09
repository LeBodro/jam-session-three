using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArmModule : Module, IPointerUpHandler, IPointerDownHandler
{
    private const float TAU = Mathf.PI * 2;
    public enum Direction
    {
        LEFT = 0,
        RIGHT,
    }

    const float OFFSET_DISTANCE = 1f;

    [Tooltip("Determines the length of the up/down motion")]
    [SerializeField] [Range(-1, 1)] float cutoff = 0f;
    [SerializeField] SpriteRenderer up = null;
    [SerializeField] SpriteRenderer down = null;

    Stat hertz = null;
    bool? armWasDown = false;
    Direction facing;
    bool wasRecentlyDragged = false;
    IDictionary<Direction, Vector3> offsetByDirection = new Dictionary<Direction, Vector3>()
    {
        {Direction.LEFT, Vector3.left * OFFSET_DISTANCE},
        {Direction.RIGHT, Vector3.right * OFFSET_DISTANCE}
    };

    bool IsArmDown { get => Mathf.Sin(Time.time * hertz.ProcessedValue * TAU) > cutoff; }
    protected override int Prefab { get => 2; }
    string InstanceIDString { get => _instanceIDString == null ? (_instanceIDString = GetInstanceID().ToString()) : _instanceIDString; }
    string _instanceIDString = null;

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!wasRecentlyDragged)
        {
            bool currentArmStateDown = IsArmDown;
            if (currentArmStateDown)
            {
                ReleaseNeighboringButton();
            }
            facing = facing == Direction.LEFT ? Direction.RIGHT : Direction.LEFT;
            RefreshArmDirection();
        }
    }

    void RefreshArmDirection()
    {
        up.flipX = facing == Direction.RIGHT;
        down.flipX = facing == Direction.RIGHT;
    }

    void PressNeighboringButton()
    {
        var neighbor = GetNeighboringModule();
        if (neighbor)
        {
            ButtonModule bm = neighbor.GetComponent<ButtonModule>();
            if (bm)
            {
                bm.Press(InstanceIDString);
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
                bm.Release(InstanceIDString);
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
        if (IsArmDown)
        {
            ReleaseNeighboringButton();
        }
        armWasDown = null;
        wasRecentlyDragged = true;
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
        bool armIsDown = IsArmDown;
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

    public override void Tierify(int tier)
    {
        hertz = stats[STAT_HERTZ];
        base.Tierify(tier);
        Price = CalculatePrice(2.5f, 0.5f, 0.25f);
        hertz.BaseValue = Mathf.Pow(2, tier) * 0.5f;
    }

    public override ModuleData Serialize(int index)
    {
        return new ModuleData(index, Prefab, Tier, bought, IsPowered, null, facing);
    }

    public override void Deserialize(ModuleData data)
    {
        base.Deserialize(data);
        facing = data.direction;
        RefreshArmDirection();
    }
}
