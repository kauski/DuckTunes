using UnityEngine;
using DuckTunes.Systems.IO;
using System.Globalization;
using System.Collections.Generic;
using DuckTunes.Systems;

namespace DuckTunes.Targets
{
    public class TargetManager : MonoBehaviour
    {
        [HideInInspector] public int ActiveSplineCount => _activeSplines.Count;
        [SerializeField] private List<GameObject> _activeSplines = new List<GameObject>();

        [HideInInspector] public int ActiveTomatoCount => _ActiveTomatoes.Count;
        [SerializeField] private List<GameObject> _ActiveTomatoes = new List<GameObject>();

        [Header("Spline Target")]
        public string TargetDataPath = "TargetDataCSV/targetdata";
        public TargetData[] Data;
        [SerializeField] private GameObject _defaultTarget;

        [Header("TomatoTarget")]
        [SerializeField] private GameObject _tomatoTarget;
        [SerializeField] private int _tomatoPoolSize = 10;


#region Public Functions
        public void SpawnTarget(SpawnObject objToSpawn)
        {
            if (GameManager.Instance.Paused) { return; }

            switch (objToSpawn)
            {
                case SpawnObject.Spline:
                    _activeSplines.Add(GameManager.PoolManager.ReuseObject(_defaultTarget, transform.position, Quaternion.identity));
                    break;

                case SpawnObject.Tomato:
                    _ActiveTomatoes.Add(GameManager.PoolManager.ReuseObject(_tomatoTarget, transform.position, Quaternion.identity));
                    break;
            }
        }

        public void RemoveTargetFromActiveList(SpawnObject objType, GameObject objToRemove)
        {
            switch (objType)
            {
                case SpawnObject.Spline:
                    RemoveObjFromList(objToRemove, _activeSplines);
                        break;
                case SpawnObject.Tomato:
                    RemoveObjFromList(objToRemove, _ActiveTomatoes);
                    break; 
            }
        }

        public void RemoveAllActiveTargets()
        {
            var tomatoes = GetActiveObjects(SpawnObject.Tomato);
            var splines = GetActiveObjects(SpawnObject.Spline);

            foreach (var t in tomatoes)
            {
                t.GetComponent<TomatoTarget>().DisableTarget();
                RemoveTargetFromActiveList(SpawnObject.Tomato, t);
            }

            foreach (var s in splines)
            {
                s.GetComponent<SplineTarget>().DisableTarget();
                RemoveTargetFromActiveList(SpawnObject.Spline, s);
            }
        }

        #endregion


#region Queries
        public GameObject[] GetActiveObjects(SpawnObject spawnObj)
        {
            switch (spawnObj)
            {
                case SpawnObject.Spline:
                    return _activeSplines.ToArray();

                case SpawnObject.Tomato:
                    return _ActiveTomatoes.ToArray();

                default:
                    return null;
;
            }

        }

        public GameObject[] GetAllPoolObjects(SpawnObject spawnObject)
        {
            GameObject[] targets = GameManager.PoolManager.GetPoolObjects();

            switch (spawnObject)
            {
                case SpawnObject.Tomato:
                    var tomatoes = new List<GameObject>();
                    for (int i = 0; i < targets.Length; i++)
                    {
                        if (targets[i].TryGetComponent(out TomatoTarget t))
                        {
                            tomatoes.Add(targets[i]);
                        }
                    }
                    return tomatoes.ToArray();

                case SpawnObject.Spline:
                    var splines = new List<GameObject>();
                    for (int i = 0; i < targets.Length; i++)
                    {
                        if (targets[i].TryGetComponent(out SplineTarget t))
                        {
                            splines.Add(targets[i]);
                        }
                    }
                    return splines.ToArray();
            }

            return null;
        }

        #endregion


#region Private Functions
        private void Awake()
        {
            CreateSplineTargetPool();
            CreateTomatoTargetPool();
            SpawnSplines();
            SpawnTomatoes();
        }

        private void CreateSplineTargetPool()
        {
            ReadTargetData();
            GameManager.PoolManager.CreatePool(_defaultTarget, Data.Length);
        }

        private void SpawnSplines()
        {
            var splines = GetAllPoolObjects(SpawnObject.Spline);
            for (var i = 0; i < splines.Length; i++)
            {
                var t = splines[i].GetComponent<SplineTarget>();
                t.Data = Data[i];
                t.Spawn();
                t.SetSortOrder(i);
            }
        }

        private void ReadTargetData()
        {
            string[] data = CSVManager.ReadFile(TargetDataPath);
            Data = new TargetData[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                string[] row = data[i].Split(';');
                string[] pointsData = row[1].Split(' ');
                string[] anchorsData = row[2].Split(' ');
                Vector3[] points = new Vector3[pointsData.Length];
                Vector3[] anchors = new Vector3[anchorsData.Length];

                Data[i] = new TargetData();
                int.TryParse(row[0], out Data[i].ID);

                for (int j = 0; j < pointsData.Length; j++)
                {
                    if (pointsData[j].StartsWith('(') && pointsData[j].EndsWith(')'))
                    {
                        pointsData[j] = pointsData[j].Substring(1, pointsData[j].Length - 2);
                    }
                    string[] values = pointsData[j].Split(',');
                    points[j] = new Vector3(
                        float.Parse(values[0], CultureInfo.InvariantCulture),
                        float.Parse(values[1], CultureInfo.InvariantCulture),
                        float.Parse(values[2], CultureInfo.InvariantCulture)
                    );
                }

                for (int k = 0; k < anchorsData.Length; k++)
                {
                    if (anchorsData[k].Contains('\r'))
                    {
                        anchorsData[k] = anchorsData[k].Replace("\r", "");
                    }
                    if (anchorsData[k].StartsWith('(') && anchorsData[k].EndsWith(')'))
                    {
                        anchorsData[k] = anchorsData[k].Substring(1, anchorsData[k].Length - 2);
                    }
                    string[] values = anchorsData[k].Split(',');
                    anchors[k] = new Vector3(
                        float.Parse(values[0], CultureInfo.InvariantCulture),
                        float.Parse(values[1], CultureInfo.InvariantCulture),
                        float.Parse(values[2], CultureInfo.InvariantCulture)
                    );
                }

                Data[i].Points = points;
                Data[i].Anchors = anchors;
            }
        }

        private void CreateTomatoTargetPool()
        {
            GameManager.PoolManager.CreatePool(_tomatoTarget, _tomatoPoolSize);
        }

        private void SpawnTomatoes()
        {
            var tomatoes = GetAllPoolObjects(SpawnObject.Tomato);
            for (var i = 0; i < tomatoes.Length; i++)
            {
                var target = tomatoes[i].GetComponent<TomatoTarget>();
                target.Spawn();
                var order = GetAllPoolObjects(SpawnObject.Spline).Length;
                target.SetSortOrder(order + i);
            }
        }

        private void RemoveObjFromList(GameObject objToRemove, List<GameObject> removeFrom) 
        {
            if (removeFrom.Contains(objToRemove))
            {
                removeFrom.Remove(objToRemove);
            }
        }

#endregion
    }
}
