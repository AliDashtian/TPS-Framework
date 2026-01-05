
[System.Serializable]
public class FloatReference
{
    public bool UseConstant = false;
    public float ConstantValue;
    public FloatVariable Variable;

    public float Value
    {
        get
        {
            return UseConstant ? ConstantValue : Variable.GetValue();
        }
        set
        {
            if (UseConstant)
            {
                ConstantValue = value;
            }
            else
            {
                Variable.SetValue(value);
            }
        }
    }
}
