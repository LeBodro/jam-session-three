using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualClickerModule : Module, IPointerUpHandler, IPointerDownHandler
{
    Stat hertz = null;
    Stat incomePerTick = null;
    Segment segment = null;
    protected override decimal[] PricePerTier { get => new decimal[]{2.25m, 157.50m, 2186.81m, 21549.09m, 210093.14m, 2156976.28m}; }

    [SerializeField] GameObject[] beatIndicators = null;

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

    /*
    For development purposes. Helps get a sense of how much the VCM produces on a minute basis
    */
    public float IncomePerMinute()
    {
        return incomePerTick.ProcessedValue * hertz.ProcessedValue * 60;
    }

    public override void Tierify(int tier)
    {
        hertz = stats[STAT_HERTZ];
        incomePerTick = stats[STAT_INCOME];
        base.Tierify(tier);
        incomePerTick.BaseValue = CalculateIncome(0.15f, 3, 0.75f);
        hertz.BaseValue = Mathf.Pow(2, tier) * 0.5f;
        segment = new Segment(Tier, 10);
    }

    public override ModuleData Serialize(int index)
    {
        return new ModuleData(index, Prefab, Tier, bought, IsPowered, segment);
    }

    public override void Deserialize(ModuleData data)
    {
        base.Deserialize(data);
        segment = data.segment;
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
