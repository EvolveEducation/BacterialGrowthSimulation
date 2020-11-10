using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderComponent : MonoBehaviour
{
    public Text textComponent;
    public Slider slider;
    public bool temp;
    public bool time;
    public bool nm;

    public void SetSliderValue()
    {
        if (temp)
        {
            textComponent.text = slider.value.ToString("0.0") + "°C";
        }
        else if (time)
        {
            int remainder = (int) Math.Floor(Math.Abs(Math.Floor(slider.value) - slider.value) * 60);
            string rString = (remainder < 10) ? "0" + remainder : remainder.ToString();
            textComponent.text = Math.Floor(slider.value).ToString() + ":" + rString;
        } 
        else if (nm)
        {
            textComponent.text = slider.value.ToString("0") + " nm";
        }
        else
        {
            textComponent.text = slider.value.ToString("0");
        }
    }
}
