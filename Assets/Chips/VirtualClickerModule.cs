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
        price = tier;
        incomePerTick.BaseValue *= Mathf.Pow(2, tier);
    }
}
