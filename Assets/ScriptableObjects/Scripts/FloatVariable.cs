using UnityEngine;

namespace DuckTunes.ScriptableObjects
{
    [CreateAssetMenu]
    public class FloatVariable : ScriptableObject
    {
        public float Value;

        public FloatVariable(float val)
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

        public static FloatVariable operator ++(FloatVariable a) => new FloatVariable(a.Value + 1);
    }
}
