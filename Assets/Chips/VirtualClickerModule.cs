using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualClickerModule : Module
{
    [SerializeField] float incomeDelay = 1;
    [SerializeField] float incomePerTick = 1;

    float accumulator;

    void Update()
    {
        if (!IsPowered) return;
        accumulator += Time.deltaTime;
        if (accumulator >= incomeDelay)
        {
            accumulator -= incomeDelay;
            Bank.Deposit(incomePerTick);
        }
    }

    public override void Tierify(int tier)
    {
        price = tier;
        incomePerTick *= Mathf.Pow(2, tier);
    }
}
