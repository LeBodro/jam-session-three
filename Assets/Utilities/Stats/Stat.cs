using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    class Modifier
    {
        public Modifier(float multiplier, float duration)
        {
            this.multiplier = multiplier;
            timeLeft = duration;
        }

        public float multiplier;
        public float timeLeft;
    }

    [SerializeField] string referenceName = string.Empty;
    [SerializeField] float baseValue = 0;

    IList<Modifier> bonuses = new List<Modifier>();
    IList<Modifier> maluses = new List<Modifier>();
    bool isDirty = true;
    float highestBonus = 1;
    float lowestMalus = 1;
    float processedValue;

    public float BaseValue
    {
        get { return baseValue; }
        set
        {
            baseValue = value;
            isDirty = true;
        }
    }

    public float ProcessedValue
    {
        get
        {
            if (isDirty)
                ProcessValue();
            return processedValue;
        }
    }

    public int ProcessedInt { get { return Mathf.RoundToInt(ProcessedValue); } }

    public KeyValuePair<string, Stat> ToKeyValuePair()
    {
        return new KeyValuePair<string, Stat>(referenceName, this);
    }

    public void AddModifier(float multiplier, float duration)
    {
        if (multiplier < 1)
            maluses.Add(new Modifier(multiplier, duration));
        else if (multiplier > 1)
            bonuses.Add(new Modifier(multiplier, duration));
        else
            return;

        isDirty = true;
    }

    public void Update()
    {
        bonuses = UpdateModifierList(bonuses);
        maluses = UpdateModifierList(maluses);
    }

    IList<Modifier> UpdateModifierList(IList<Modifier> modifiers)
    {
        if (modifiers.Count == 0)
            return modifiers;

        float deltaTime = Time.deltaTime;
        IList<Modifier> remainingModifiers = new List<Modifier>();
        for (int i = 0; i < modifiers.Count; i++)
        {
            modifiers[i].timeLeft -= deltaTime;
            if (modifiers[i].timeLeft > 0)
                remainingModifiers.Add(modifiers[i]);
            else
                isDirty = true;
        }
        return remainingModifiers;
    }

    void ProcessValue()
    {
        highestBonus = lowestMalus = 1;
        foreach (var modifier in bonuses)
            highestBonus = Mathf.Max(highestBonus, modifier.multiplier);

        foreach (var modifier in maluses)
            lowestMalus = Mathf.Min(lowestMalus, modifier.multiplier);

        processedValue = baseValue * highestBonus * lowestMalus;
        isDirty = false;
    }
}
