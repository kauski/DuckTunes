using UnityEngine;
using UnityEditor;
using DuckTunes.TargetTool;
using DuckTunes.TargetTool.Data;

namespace DuckTunes.EditorScripts
{
[CustomEditor(typeof(TargetCreator))]
public class TargetEditor : Editor 
{
    TargetCreator _creator;
    TargetDataHolder _target => _creator.CurrentTarget;

    const float _segmentSelectDstThreshold = 0.2f;
    int _selectedSegmentIndex = -1;

    //Inspector functionality
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();

        ToggleAutoHandlesInInspector();
        GUILayout.Space(20);
        CreateNewSplineInInspector();
        SaveAsPrefab();

        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
    }
 
    //Scene functionality
    private void OnSceneGUI()
    {
        Input();
        Draw();
    }

    private void Input()
    {
        Event OnGuiEvent = Event.current;
        Vector2 mousePos = HandleUtility.GUIPointToWorldRay(OnGuiEvent.mousePosition).origin;

        HandleAddingAndSplittingSegment(OnGuiEvent, mousePos);
        HandleDeletingSegments(OnGuiEvent, mousePos);
        HandleSelectingSegment(OnGuiEvent, mousePos);

        HandleUtility.AddDefaultControl(0);
    }

    private void Draw()
    {
        DrawBezierCurve();
        DrawAnchorAndHandlePoints();
        DrawCircle();
    }

    private void OnEnable()
    {
        _creator = (TargetCreator)target;

        if (_creator.CurrentTarget.IsNull())
        {
            _creator.Create();
        }

        Tools.hidden = true;
    }

    void OnDisable()
    {
        Tools.hidden = false;
    }

    #region drawing_functions
    private void DrawAnchorAndHandlePoints()
    {
        for (int i = 0; i < _target.Spline.NumPoints; i++)
        {
            if (i % 3 == 0 || _creator.DisplayControlPoints)
            {
                Handles.color = (i % 3 == 0) ? _creator.AnchorColor : _creator.HandleColor;
                float handleSize = (i % 3 == 0) ? _creator.AnchorDiameter : _creator.HandleDiameter;
                Vector2 newPos = Handles.FreeMoveHandle(_target.Spline[i], Quaternion.identity, handleSize, Vector2.zero, Handles.CylinderHandleCap);
                if (_target.Spline[i] != newPos)
                {
                    Undo.RecordObject(_creator, "Move point");
                    if (i % 3 != 0)
                    {
                        _creator.UpdateAllHandlePositions();
                    }
                    else if (i % 3 == 0)
                    {
                        _creator.UpdateAllAnchorPositions();
                    }
                    _target.Spline.MovePoint(i, newPos);
                }
            }
        }
    }

    private void DrawCircle()
    {
        for (int i = 0; i < _target.Circles.Length; i++)
        {
            Handles.color = _target.Circles[i].IsStartingCircle ? _creator.StartCircleColor : _creator.EndCircleColor; 
            Vector2 newPos = Handles.FreeMoveHandle(_target.Circles[i].Position, Quaternion.identity, _creator.CircleSize, Vector2.zero, Handles.CylinderHandleCap);
            bool isStart = _target.Circles[i].IsStartingCircle;
            if (_target.Circles[i].Position != newPos)
            {
                Undo.RecordObject(_creator, "Move Circle");
                if (i % 3 == 0)
                {
                    _creator.UpdateAllAnchorPositions();
                }
                _target.Circles[i].MoveCircle(newPos);
                MoveEndPoints(newPos, isStart);
            }
        }
    }

    private void MoveEndPoints(Vector2 pos, bool start)
    {
        if (start)
        {
            _target.Spline.MovePoint(0, pos);
        }
        else
        {
            _target.Spline.MovePoint(_target.Spline.Points.Count - 1, pos);
        }
    }

    private void DrawBezierCurve()
    {
        for (int i = 0; i < _target.Spline.NumSegments; i++)
        {
            Vector2[] points = _target.Spline.GetPointsInSegment(i);
            if (_creator.DisplayControlPoints)
            {
                Handles.color = Color.black;
                Handles.DrawLine(points[1], points[0]);
                Handles.DrawLine(points[2], points[3]);
            }

            Color segmentColor = (i == _selectedSegmentIndex && Event.current.shift) ? _creator.SelectedSegmentColor : _creator.SegmentColor;
            Handles.DrawBezier(points[0], points[3], points[1], points[2], segmentColor, null, 2);
        }
    }
    #endregion


    #region inspector_functions
    private void ToggleAutoHandlesInInspector()
    {
        bool autoSetHandlePoints = GUILayout.Toggle(_target.Spline.AutoSetHandlePoints, "Auto set control points");
        if (autoSetHandlePoints != _target.Spline.AutoSetHandlePoints)
        {
            Undo.RecordObject(_creator, "Toggle auto set controls");
            _target.Spline.AutoSetHandlePoints = autoSetHandlePoints;
        }
    }

    private void CreateNewSplineInInspector()
    {
        if (GUILayout.Button("Create new"))
        {
            Undo.RecordObject(_creator, "Create new");
            _creator.Create();
        }
    }

    private void SaveAsPrefab()
    {
        if (GUILayout.Button("Save as Prefab"))
        {
            if (_creator.CurrentTarget.Spline != null || _creator.CurrentTarget.Circles.Length > 0)
            {
                Undo.RecordObject(_creator, "Save prefab");
                _creator.Save();
            }
        }
    }
    #endregion


    #region input_functions
    private void HandleSelectingSegment(Event OnGuiEvent, Vector2 mousePos)
    {
        if (OnGuiEvent.type == EventType.MouseMove)
        {
            float minDstToSegment = _segmentSelectDstThreshold;
            int newSelectedSegmentIndex = -1;

            for (int i = 0; i < _target.Spline.NumSegments; i++)
            {
                Vector2[] points = _target.Spline.GetPointsInSegment(i);
                float dst = HandleUtility.DistancePointBezier(mousePos, points[0], points[3], points[1], points[2]);
                if (dst < minDstToSegment)
                {
                    minDstToSegment = dst;
                    newSelectedSegmentIndex = i;
                }
            }

            if (newSelectedSegmentIndex != _selectedSegmentIndex)
            {
                _selectedSegmentIndex = newSelectedSegmentIndex;
                HandleUtility.Repaint();
            }
        }
    }

    private void HandleDeletingSegments(Event OnGuiEvent, Vector2 mousePos)
    {
        if (OnGuiEvent.type == EventType.MouseDown && OnGuiEvent.button == 1)
        {
            float minDstToAnchor = _creator.AnchorDiameter * 0.5f;
            int closestAnchorIndex = -1;

            for (int i = 0; i < _target.Spline.NumPoints; i += 3)
            {
                float dst = Vector2.Distance(mousePos, _target.Spline[i]);
                if (dst < minDstToAnchor)
                {
                    minDstToAnchor = dst;
                    closestAnchorIndex = i;
                }
            }

            if (closestAnchorIndex != -1)
            {
                Undo.RecordObject(_creator, "Delete segment");
                _target.Spline.DeleteSegment(closestAnchorIndex);
            }
        }
    }

    private void HandleAddingAndSplittingSegment(Event OnGuiEvent, Vector2 mousePos)
    {
        if (OnGuiEvent.type == EventType.MouseDown && OnGuiEvent.button == 0 && OnGuiEvent.shift)
        {
            if (_selectedSegmentIndex != -1)
            {
                Undo.RecordObject(_creator, "Split segment");
                _target.Spline.SplitSegment(mousePos, _selectedSegmentIndex);
                if (_target.Anchors.Contains(mousePos))
                {
                    _target.Anchors.Remove(mousePos);
                }
            }
            else
            {
                Undo.RecordObject(_creator, "Add segment");
                _target.Spline.AddSegment(mousePos);
                _target.Anchors.Add(mousePos);
                _target.Circles[1].MoveCircle(mousePos);
            }
        }
    }
    #endregion
}    
}
