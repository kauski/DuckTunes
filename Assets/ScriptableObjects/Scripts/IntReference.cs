using System;

namespace DuckTunes.ScriptableObjects
{
    [Serializable]
    public class IntReference
    {
        public bool UseConstant = true;
        public int ConstantValue;
        public IntVariable Variable;

        //public FloatReference(float value)
        //{
        //    Variable.Value = value;
        //}

        //public FloatReference(FloatVariable floatVar)
        //{
        //    Variable = floatVar;
        //}

        public int Value
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
