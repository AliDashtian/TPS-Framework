using UnityEngine;
using UnityEngine.UI;

public class FloatVariableText : MonoBehaviour
{
    public FloatVariable Variable;

    private Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
        UpdateText();
    }

    private void OnEnable()
    {
        Variable.OnValueChanged += UpdateText;
    }

    private void OnDisable()
    {
        Variable.OnValueChanged -= UpdateText;
    }

    private void UpdateText()
    {
        _text.text = Variable.GetValue().ToString();
    }
}
