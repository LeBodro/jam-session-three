using UnityEngine;
using UnityEngine.UI;

public class NotePopUp : PopUp<int>
{
    protected override string Interpret(int value) => value.ToString();
}
