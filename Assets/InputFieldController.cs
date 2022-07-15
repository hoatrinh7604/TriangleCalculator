using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputFieldController : MonoBehaviour
{
    [SerializeField] int index;
    [SerializeField] TMP_InputField input;
    private string currentString;

    private void Start()
    {
        input.onValueChanged.AddListener(delegate {OnValueChanged();});
    }
    public void OnValueChanged()
    {
        if (input.text.Length > 20)
        {
            input.text = currentString;
        }
        else
        {
            currentString = input.text;
        }
        Controller.Instance.OnValueChanged();
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }

    public void OnSelected()
    {
        TouchScreenKeyboard.Open("1", TouchScreenKeyboardType.NumberPad);
    }

    public void OnDisabled()
    {
        TouchScreenKeyboard.hideInput = true;
    }
}
