using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Float Variable", menuName = "Scriptable Objects/Float Variable")]
public class FloatVariable : ScriptableObject
{
    [SerializeField]
    private float Value;

    /// <summary>
    /// Using this method is good for my game
    /// but for a larger system, that has more data and needs something like a Save Button,
    /// I should write a separate Saving system using JSON
    /// </summary>
    [Header("Saving Functionality")]
    public bool SaveValue;
    public string Key;
    public float DefaultValue = 0f;

    public event Action OnValueChanged;

    private void OnEnable()
    {
        if (Application.isPlaying)
        {
            Value = LoadValue();
        }
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            OnValueChanged?.Invoke();
        }
    }

    public float GetValue()
    {
        return Value;
    }

    public void SetValue(float value)
    {
        Value = value;

        if (Application.isPlaying)
        {
            Save();
            OnValueChanged?.Invoke();
        }
    }

    [ContextMenu("Delete Key")]
    public void DeleteKey()
    {
        if (SaveValue)
        {
            PlayerPrefs.DeleteKey(Key);
            Value = DefaultValue;
        }
    }

    private void Save()
    {
        if (SaveValue)
        {
            PlayerPrefs.SetFloat(Key, Value);
        }
    }

    private float LoadValue()
    {
        if (SaveValue)
        {
            return PlayerPrefs.GetFloat(Key, DefaultValue);
        }

        return Value;
    }
}
