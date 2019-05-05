using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualClickerModule : Module
{
    Stat incomeDelay = null;
    Stat incomePerTick = null;

    float accumulator;

    void Update()
    {
        if (!IsPowered) return;
        accumulator += Time.deltaTime;
        if (accumulator >= 1 / incomeDelay.ProcessedValue)
        {
            accumulator -= incomeDelay.ProcessedValue;
            Bank.Deposit(incomePerTick.ProcessedDecimal);
        }
    }

    public override void Tierify(int tier)
    {
        incomeDelay = stats[STAT_HERTZ];
        incomePerTick = stats[STAT_INCOME];
        base.Tierify(tier);
        Price = CalculatePrice(3f, 2f, 1.25f);
        incomePerTick.BaseValue = Mathf.Pow(1.25f, tier);
        incomeDelay.BaseValue = Mathf.Pow(2, tier) * 0.5f;
    }
}
