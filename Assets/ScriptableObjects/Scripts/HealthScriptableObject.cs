using UnityEngine;

namespace DuckTunes.ScriptableObjects
{
    [CreateAssetMenu(fileName = "HealthData", menuName = "PlayerData/Health")]
    public class HealthScriptableObject : ScriptableObject
    {
        public FloatReference MaxHP;
        public FloatReference CurrentHp;
    }
}