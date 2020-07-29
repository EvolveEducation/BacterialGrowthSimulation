using UnityEngine;
using UnityEngine.UI;

public class SliderComponent : MonoBehaviour
{
    public Text textComponent;
    public Slider slider;
    public bool whole;

    public void SetSliderValue()
    {
        if (whole)
        {
        }
        textComponent.text = slider.value.ToString("0.00");
    }
}
