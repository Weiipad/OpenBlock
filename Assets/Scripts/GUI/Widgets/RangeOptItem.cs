using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RangeOptItem : MonoBehaviour
{
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private InputField numInput;
    [SerializeField]
    private Slider numSilde;

    [Space]
    [SerializeField]
    private string itemName;
    [SerializeField]
    private float min, max;

    [SerializeField]
    private UnityEvent<float> onValueChanged;

    private float curValue = 0;

    private void OnEnable()
    {
        nameText.text = itemName;
        numSilde.minValue = min;
        numSilde.maxValue = max;
        UpdateChildren();
    }

#if UNITY_EDITOR
    public void OnValidate()
    {
        nameText.text = itemName;
        numSilde.minValue = min;
        numSilde.maxValue = max;
        UpdateChildren();
    }
#endif

    public void SetValue(float value)
    {
        curValue = (float)System.Math.Round(value, 2);
        onValueChanged?.Invoke(curValue);
        UpdateChildren();
    }

    public void OnNumberInput(string numberStr)
    {
        SetValue(float.Parse(numberStr));
    }

    public float GetValue() => curValue;

    private void UpdateChildren()
    {
        numInput.text = curValue.ToString();
        numSilde.value = curValue;
    }
}
