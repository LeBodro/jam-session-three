using UnityEngine;
using UnityEngine.UI;

public class NotePopUp : PopUp<int>
{
    protected override string Interpret(int value)
    {
        switch (value)
        {
            case 0: return "A1";
            case 1: return "C2";
            case 2: return "D2";
            case 3: return "E2";
            case 4: return "G2";
            case 5: return "A2";
            case 6: return "C3";
            case 7: return "D3";
            default: return value.ToString();
        }
    }
}
