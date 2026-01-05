using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Int Variable", menuName = "Scriptable Objects/Int Variable")]
public class IntVariable : ScriptableObject
{
    [SerializeField]
    private int Value;

    [Header("Saving Functionality")]
    public bool SaveValue;
    public string Key;
    public int DefaultValue = 0;

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

    public int GetValue()
    {
        return Value;
    }

    public void SetValue(int value)
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
            PlayerPrefs.SetInt(Key, Value);
        }
    }

    private int LoadValue()
    {
        if (SaveValue)
        {
            return PlayerPrefs.GetInt(Key, DefaultValue);
        }

        return Value;
    }
}
