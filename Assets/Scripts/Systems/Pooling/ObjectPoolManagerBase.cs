using System.Collections.Generic;
using UnityEngine;

namespace DuckTunes.Systems.ObjectPooling
{
    public abstract class ObjectPoolManagerBase : MonoBehaviour
    {
        protected Dictionary<int, Queue<ObjectInstance>> PoolDictionary = new Dictionary<int, Queue<ObjectInstance>>();
        protected List<GameObject> _poolObjects = new List<GameObject>();

        protected void CreateNewQueue(int poolKey)
        {
            PoolDictionary.Add(poolKey, new Queue<ObjectInstance>());
        }

        protected void CreateObjectInstances(GameObject prefab, int size, int poolKey, GameObject poolHolder)
        {
            for (int i = 0; i < size; i++)
            {
                ObjectInstance newObject = new ObjectInstance(Instantiate(prefab) as GameObject);
                _poolObjects.Add((GameObject)newObject);
                PoolDictionary[poolKey].Enqueue(newObject);
                newObject.SetParentTransform(poolHolder.transform);
            }
        }

        protected GameObject CreateParentGameObject(GameObject prefab)
        {
            GameObject poolHolder = new GameObject(prefab.name + " pool");
            poolHolder.transform.parent = transform;
            return poolHolder;
        }

        protected static int GetPoolKey(GameObject prefab)
        {
            return prefab.GetInstanceID();
        }

        protected void Enqueue(int poolKey, ObjectInstance objectToReuse)
        {
            PoolDictionary[poolKey].Enqueue(objectToReuse);
        }

        protected ObjectInstance Dequeue(int poolKey)
        {
            return PoolDictionary[poolKey].Dequeue();
        }
    }
}

