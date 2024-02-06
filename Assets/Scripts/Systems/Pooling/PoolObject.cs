using UnityEngine;

namespace DuckTunes.Systems.ObjectPooling
{
    public abstract class PoolObject : MonoBehaviour
    {
        public virtual void OnObjectReuse()
        {

        }

        public void Destroy()
        {
            gameObject.SetActive(false);
        }
    }
}

