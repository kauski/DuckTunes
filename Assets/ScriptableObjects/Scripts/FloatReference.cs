using System;

namespace DuckTunes.ScriptableObjects
{
    [Serializable]
    public class FloatReference
    {
        public bool UseConstant = true;
        public float ConstantValue;
        public FloatVariable Variable;

        //public FloatReference(float value)
        //{
        //    Variable.Value = value;
        //}

        //public FloatReference(FloatVariable floatVar)
        //{
        //    Variable = floatVar;
        //}

        public float Value
        {
            get { return UseConstant ? ConstantValue : Variable.Value; }
            set
            {
                if (UseConstant)
                {
                    ConstantValue = value;
                }
                else
                {
                    Variable.Value = value;
                }
            }
        }

        //public static implicit operator float (FloatReference reference)
        //{
        //    return reference.Value;
        //}

        //public static implicit operator FloatReference (float value)
        //{
        //    return new FloatReference(value);
        //}

        //public static implicit operator FloatReference(FloatVariable value)
        //{
        //    return new FloatReference(value);
        //}
    }
}
