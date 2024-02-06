using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuckTunes
{
    public interface ITargetCreator
    {
#if UNITY_EDITOR
        public void Create();
        public void Save();
#endif
    }
}

