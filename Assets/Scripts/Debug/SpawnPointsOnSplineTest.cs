using UnityEngine;
using DuckTunes.TargetTool;
using DuckTunes.TargetTool.Data;

namespace DuckTunes.DebugAndTesting
{
    public class SpawnPointsOnSplineTest : MonoBehaviour
    {
        public float Spacing = .1f;
        public float Resolution = 1;
        [SerializeField] Material _lineMat;
        [SerializeField] LineRenderer _renderer;
        [SerializeField] GameObject _circlePrefab;
        TargetCreator _creator;
        Spline _spline => _creator.Spline;
        [HideInInspector] public Vector2[] Points;
        Vector3[] v3Points;

        void Start()
        {
            _creator = FindObjectOfType<TargetCreator>();
            Points = GetSplinePoints();
            v3Points = GetV3Points(Points);

            CreateLine();
            CreateCircles();
        }

        private Vector3[] GetV3Points(Vector2[] points)
        {
            Vector3[] splinePointsV3 = new Vector3[points.Length];
            for (int i = 0; i < Points.Length; i++)
            {
                splinePointsV3[i] = new Vector3(points[i].x, points[i].y, 0);
            }
            return splinePointsV3;
        }

        public Vector2[] GetSplinePoints()
        {
            Vector2[] splinePointsV2 = _spline.CalculateEvenlySpacedPoints(Spacing, Resolution);
            return splinePointsV2;
        }

        public void CreateLine()
        {
            //Line Renderer
            _renderer = GetComponent<LineRenderer>();
            int pointCount = Points.Length;
            _renderer.positionCount = pointCount;
            _renderer.SetPositions(v3Points);
            _renderer.generateLightingData = true;
            _lineMat.color = Color.yellow;
            _renderer.material = _lineMat;

        }

        private void CreateCircles()
        {
            Vector2 startPos = _renderer.GetPosition(0);
            Vector2 endPos = _renderer.GetPosition(_renderer.positionCount - 1);

            GameObject startCircle = Instantiate(_circlePrefab, startPos, Quaternion.identity);
            GameObject endCircle = Instantiate(_circlePrefab, endPos, Quaternion.identity);

            endCircle.transform.localScale = new Vector3(_renderer.endWidth, _renderer.endWidth, _renderer.endWidth);
            startCircle.transform.localScale = new Vector3(_renderer.endWidth, _renderer.endWidth, _renderer.endWidth);

            endCircle.transform.SetParent(this.transform);
            startCircle.transform.SetParent(this.transform);
        }

        public void CreateGameObjects()
        {
            //Spawn GameObjects
            foreach (Vector2 p in Points)
            {
                GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                g.transform.position = p;
                g.transform.localScale = Vector3.one * Spacing * .5f;
            }
        }
    }
}
