using UnityEngine;

namespace DuckTunes.Systems.ObjectPooling
{
    public class ObjectPoolManager : ObjectPoolManagerBase
    {
        public void CreatePool(GameObject prefab, int size)
        {
            int poolKey = GetPoolKey(prefab);
            GameObject poolHolder = CreateParentGameObject(prefab);

            if (PoolDictionary.ContainsKey(poolKey)) { return; }

            CreateNewQueue(poolKey);
            CreateObjectInstances(prefab, size, poolKey, poolHolder);
        }

        public GameObject ReuseObject(GameObject prefab, Vector3 pos, Quaternion rot)
        {
            int poolKey = GetPoolKey(prefab);

            if (!PoolDictionary.ContainsKey(poolKey)) { return null; }

            ObjectInstance objectToReuse = Dequeue(poolKey);
            Enqueue(poolKey, objectToReuse);

            objectToReuse.Reuse(pos, rot);

            return ((GameObject)objectToReuse);
        }

        public GameObject[] GetPoolObjects()
        {
            return _poolObjects.ToArray();
        }
    }

}
