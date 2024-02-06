using System;
using UnityEngine;

namespace DuckTunes.Systems.ObjectPooling
{
    public class ObjectInstance
    {
        GameObject _poolObj;
        Transform _transform;

        bool hasPoolObjScript;
        PoolObject _poolObjScript;

        public ObjectInstance(GameObject objInstance)
        {
            _poolObj = objInstance;
            _transform = _poolObj.transform;
            _poolObj.SetActive(false);

            if (objInstance.TryGetComponent(out PoolObject poolObj))
            {
                hasPoolObjScript = true;
                _poolObjScript = poolObj;
            }
        }

        public void Reuse(Vector3 pos, Quaternion rot)
        {
            _poolObj.SetActive(true);
            _transform.position = pos;
            _transform.rotation = rot;

            if (hasPoolObjScript)
            {
                _poolObjScript.OnObjectReuse();
            }
        }

        public static explicit operator GameObject(ObjectInstance v) => v._poolObj;

        public void SetParentTransform(Transform parent)
        {
            _transform.parent = parent;
        }
    }
}

