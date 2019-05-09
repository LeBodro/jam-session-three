using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualClickerModule : Module, IPointerUpHandler, IPointerDownHandler
{
    Stat hertz = null;
    Stat incomePerTick = null;
    Segment segment = null;

    [SerializeField] GameObject[] beatIndicators;

    event System.Action<VirtualClickerModule> _onMouseClick = delegate { };
    public event System.Action<VirtualClickerModule> OnMouseClick
    {
        add { _onMouseClick += value; }
        remove { _onMouseClick -= value; }
    }

    float accumulator;

    protected override int Prefab { get => 1; }

    public Segment GetSegment()
    {
        // TODO: Serialize sequence in save
        return segment;
    }

    void Update()
    {
        for (int i = 0; i < beatIndicators.Length; i++)
        {
            beatIndicators[i].SetActive(IsPowered && segment.PlayBeat(i));
        }
        if (!IsPowered) return;
        accumulator += Time.deltaTime;
        float delay = 1 / hertz.ProcessedValue;
        if (accumulator >= delay)
        {
            accumulator -= delay;
            GenerateIncome(incomePerTick.ProcessedDecimal);
        }
    }

    public override void Tierify(int tier)
    {
        hertz = stats[STAT_HERTZ];
        incomePerTick = stats[STAT_INCOME];
        base.Tierify(tier);
        Price = CalculatePrice(3f, 2f, 1.25f);
        incomePerTick.BaseValue = CalculateIncome(0.15f, 3, 0.75f);
        hertz.BaseValue = Mathf.Pow(2, tier) * 0.5f;
        segment = new Segment(Tier, 10);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (IsPowered)
        {
            _onMouseClick(this);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Must be implemented in order for onpointerup to work
    }
}
