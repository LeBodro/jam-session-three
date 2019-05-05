using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualClickerModule : Module
{
    Stat hertz = null;
    Stat incomePerTick = null;

    float accumulator;

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
        incomePerTick.BaseValue = Mathf.Pow(1.25f, tier);
        hertz.BaseValue = Mathf.Pow(2, tier) * 0.5f;
    }
}
