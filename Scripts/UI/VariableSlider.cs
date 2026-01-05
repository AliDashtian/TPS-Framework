using UnityEngine;
using UnityEngine.UI;

public class VariableSlider : MonoBehaviour
{
    public FloatReference MaxValue;
    public FloatReference CurrentValue;

    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();

        _slider.maxValue = MaxValue.Value;
    }

    private void OnEnable()
    {
        CurrentValue.Variable.OnValueChanged += UpdateSlider;
    }

    private void OnDisable()
    {
        CurrentValue.Variable.OnValueChanged -= UpdateSlider;
    }

    void UpdateSlider()
    {
        _slider.value = CurrentValue.Value;
    }
}
