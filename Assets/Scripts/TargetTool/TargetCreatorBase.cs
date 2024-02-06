using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using DuckTunes.ScriptableObjects;
using DuckTunes.TargetTool.Data;
using DuckTunes.Systems.IO;

namespace DuckTunes.TargetTool.Utility
{
    public class TargetCreatorBase : MonoBehaviour
    {
        [HideInInspector] public TargetDataHolder CurrentTarget;
        [HideInInspector] public Spline Spline;
        [HideInInspector] public Circle[] Circles = new Circle[2];
        [HideInInspector] public List<Vector2> Handles = new List<Vector2>();
        [HideInInspector] public List<Vector2> Anchors = new List<Vector2>();
        public Circle this[int i] => Circles[i];

        [Header("Spline Settings")]
        public Color AnchorColor = Color.red;
        public Color HandleColor = Color.white;
        public Color SegmentColor = Color.green;
        public Color SelectedSegmentColor = Color.yellow;
        public float AnchorDiameter = .1f;
        public float HandleDiameter = .07f;
        public bool DisplayControlPoints = true;

        [Header("Circle Settings")]
        public Color StartCircleColor = Color.blue;
        public Color EndCircleColor = Color.cyan;
        public float CircleSize = .6f;

        [Header("Prefab Settings")]
        [SerializeField] private float Resolution = 1f;
        [SerializeField] private float Spacing = 0.1f;

        [SerializeField] private GameObject _targetPrefab;
        public FloatVariable id;

#if UNITY_EDITOR

        public virtual void Save()
        {
            SaveDataAsCSV();
        }

        public void UpdateAllHandlePositions()
        {
            Handles.Clear();
            for (int i = 0; i < Spline.NumPoints; i++)
            {
                if (i % 3 != 0)
                {
                    Handles.Add(Spline[i]);
                }
            }
        }

        public void UpdateAllAnchorPositions()
        {
            Anchors.Clear();

            Vector2[] v2A = new Vector2[Spline.NumSegments + 1];
           
            for (int i = 0; i < Spline.NumSegments; i++)
            {
                Vector2[] points = Spline.GetPointsInSegment(i);
                v2A[i] = points[0];
                v2A[i + 1] = points[3];
            }
            Anchors = v2A.ToList();
        }

        public List<Vector2> GetHandlePositions()
        {
            return Handles;
        }

        public List<Vector2> GetAnchorPositions()
        {
            return Anchors;
        }

        public Circle[] CreateCircles()
        {
            Circle[] cs = new Circle[2];
            Vector2[] endPoints = Spline.GetStartAndEndPoints();
            for (int i = 0; i < Circles.Length; i++)
            {
                bool isFirst = (i == 0) ? true : false;
                cs[i] = new Circle(endPoints[i], isFirst);
            }
            return cs;
        }

        private void SaveDataAsCSV()
        {
            //id, points, anchors
            string[] dataString = new string[3] {
                GenerateID(),
                "",
                "",
            };

            string points = "";
            Vector2[] splinePoints = GetSplinePoints();
            Vector3[] v3Points = GetV3Points(splinePoints);
            for (int i = 0; i < v3Points.Length; i++)
            {
                if (points != "")
                {
                    points += " ";
                }
                string[] sPoints = new string[v3Points.Length];
                string[] trimmedsPoints = new string[v3Points.Length];
                sPoints[i] = v3Points[i].ToString();
                trimmedsPoints[i] = string.Concat(sPoints[i].Where(c => !char.IsWhiteSpace(c)));
                points += trimmedsPoints[i];
            }
            dataString[1] = points;

            Vector3[] anchorPoints = GetV3Points(Anchors.ToArray());
            string anchors = "";
            for (int i = 0; i < anchorPoints.Length; i++)
            {
                if (anchors != "")
                {
                    anchors += " ";
                }
                string[] sAnchors = new string[anchorPoints.Length];
                string[] trimmedAnchors = new string[anchorPoints.Length];
                sAnchors[i] = anchorPoints[i].ToString();
                trimmedAnchors[i] = string.Concat(sAnchors[i].Where(c => !char.IsWhiteSpace(c)));
                anchors += trimmedAnchors[i];
            }
            dataString[2] = anchors;

            Debug.Log("Data prepared for Saving: " + dataString);

            CSVManager.AppendToFile(dataString);
        }

        private Vector3[] GetV3Points(Vector2[] points)
        {
            Vector3[] splinePointsV3 = new Vector3[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                splinePointsV3[i] = new Vector3(points[i].x, points[i].y, 0);
            }
            return splinePointsV3;
        }

        public Vector2[] GetSplinePoints()
        {
            Vector2[] splinePointsV2 = CurrentTarget.Spline.CalculateEvenlySpacedPoints(Spacing, Resolution);
            return splinePointsV2;
        }

        private string GenerateID()
        {
            id.Value++;
            return id.Value.ToString();
        }
#endif
    }
}
