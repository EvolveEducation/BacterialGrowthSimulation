using UnityEngine;
using UnityEngine.UI;

public class SliderComponent : MonoBehaviour
{
    public Text textComponent;
    public Slider slider;

    public void SetSliderValue()
    {
        textComponent.text = slider.value.ToString("0.00");
    }
}
