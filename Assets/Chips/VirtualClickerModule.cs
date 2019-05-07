using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualClickerModule : Module
{
    Stat hertz = null;
    Stat incomePerTick = null;

    float accumulator;

    protected override int Prefab { get => 1; }

    public Segment GetSegment()
    {
        // TODO: Note should be set/increment by button
        // TODO: Make sequence modifiable and serialize it in save
        // TODO: Keep a reference to segment to modify it's sequence easily
        return new Segment(Tier, 5);
    }

    void Update()
    {
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
    }
}
