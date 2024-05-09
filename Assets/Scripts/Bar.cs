using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public float maxValue = 0;
    public Slider slider;

    public void setPrecentage(float value)
    {
        if (value > maxValue) value = maxValue;
        slider.value = value;
    }

    public void setMaxValue(float value)
    {
        this.maxValue = value;
        slider.maxValue = value;
    }
}
