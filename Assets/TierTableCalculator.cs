using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TierTableCalculator : MonoBehaviour
{
    // Start is called before the first frame update
    public VirtualClickerModule VCM;
    public ArmModule Arm;
    public ButtonModule Button;
    void Start()
    {
        var vcminstance = Instantiate(VCM);
        var arminstance = Instantiate(Arm);
        var buttoninstance = Instantiate(Button);
        Debug.Log("Tier,Description,Income,Price");
        for (int tier = 0; tier < 6; tier++) {
            vcminstance.Tierify(tier);
            arminstance.Tierify(tier);
            buttoninstance.Tierify(tier);
            Debug.LogFormat("Tier {0},only VCM,{1},{2}", tier + 1, vcminstance.IncomePerMinute() * 16, vcminstance.Price * 16);
            Debug.LogFormat("Tier {0},Arm + button,{1},{2}", tier + 1, arminstance.ClicksPerMinute() * buttoninstance.IncomePerClick() * 8, (arminstance.Price + buttoninstance.Price) * 8);
            Debug.LogFormat("Tier {0},button price {1}, arm price {2}", tier + 1, buttoninstance.Price * 8, arminstance.Price * 8);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
