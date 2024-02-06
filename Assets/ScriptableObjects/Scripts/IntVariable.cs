using UnityEngine;

namespace DuckTunes.ScriptableObjects
{
    [CreateAssetMenu]
    public class IntVariable : ScriptableObject
    {
        public int Value;

        public IntVariable(int val)
        {

        }

        //#if UNITY_EDITOR
        //    [Multiline]
        //    public string Description = "";
        //#endif

        //    public void SetValue(float value)
        //    {
        //        Value = value;
        //    }

        //    public void Change(float amount)
        //    {
        //        Value += amount;
        //    }

        //    public void Change(FloatVariable amount)
        //    {
        //        Value += amount.Value;
        //    }

        public static IntVariable operator ++(IntVariable a) => new IntVariable(a.Value + 1);
    }
}