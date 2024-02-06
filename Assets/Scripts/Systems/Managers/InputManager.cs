using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using DuckTunes.Utility;
using System.Collections.Generic;
using DuckTunes.Targets;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace DuckTunes.Systems.InputHandling
{
    public class InputManager : MonoBehaviour
    {
        private Dictionary<int, Target[]> _targets = new Dictionary<int, Target[]>();

        private Camera _cam;

        private void Awake()
        {
            Input.multiTouchEnabled = true;
            _cam = Camera.main;
        }

        private void Update()
        {
            if (GameManager.Instance.Paused) { return; }

            foreach (Touch touch in Touch.activeTouches)
            {
                if (TouchPhaseEnded(touch)) continue;

                if (TouchPhaseBegan(touch)) continue;
                
                NormalTouch(touch);

#if UNITY_EDITOR
                Debug.DrawLine(Vector3.zero, Utilities.GetWorldPositionFromScreenPosition(touch.screenPosition, _cam),
                Color.green);
#endif
            }
        }
        private bool TouchPhaseBegan(Touch touch)
        {
            Target[] targets;
            if (touch.phase == TouchPhase.Began)
            {
                if (_targets.ContainsKey(touch.touchId))
                {
                    foreach (Target t in _targets[touch.touchId])
                    {
                        t.Tap();
                    }
                    return true;
                }
                else
                {
                    targets = TryGetTargets(touch);

                    if (targets != null)
                    {
                        AddTargetsToList(targets, touch);
                        
                        foreach (Target t in _targets[touch.touchId])
                        {
                            t.Tap();
                        }
                    }
                }
            }

            return false;
        }

        private void NormalTouch(Touch touch)
        {
            Target[] targets = null;

            if (_targets.ContainsKey(touch.touchId))
            {
                foreach (Target t in _targets[touch.touchId])
                {
                    t.CurrentTouchPos = Utilities.GetWorldPositionFromScreenPosition(touch.screenPosition, _cam);
                    t.Tick();
                }
            }
            else
            {
                targets = TryGetTargets(touch);

                AddTargetsToList(targets, touch);

                if (targets != null)
                {
                    for (int i = 0; i < targets.Length; i++)
                    {
                        targets[i].CurrentTouchPos = Utilities.GetWorldPositionFromScreenPosition(touch.screenPosition, _cam);
                        targets[i].Tick();
                    }
                }
            }
        }

        private bool TouchPhaseEnded(Touch touch)
        {
            if (touch.phase is TouchPhase.Ended or TouchPhase.Canceled)
            {
                if (_targets.ContainsKey(touch.touchId))
                {
                    foreach (Target target in _targets[touch.touchId])
                    {
                        target.DisableTarget();
                        return true;
                    }
                }
            }
            return false;
        }

        private void AddTargetsToList(Target[] targets, Touch touch)
        {
            if (targets != null)
            {
                if (!_targets.ContainsKey(touch.touchId))
                {
                    _targets.Add(touch.touchId, targets);

                }
            }
        }

        private Target[] TryGetTargets(Touch touch)
        {
            Target[] targets = null;
            Transform[] hitObj =
                GameManager.SelectionManager.Select(
                    Utilities.GetWorldPositionFromScreenPosition(touch.screenPosition, _cam), false);
            if (hitObj != null)
            {
                int splineCount = 0;
                List<Target> tempList = new List<Target>(); //for making sure no more than 1 spline is selected

                targets = Utilities.TryGetComponentsTFromGameObjectArray<Target>(hitObj);
                tempList.AddRange(targets);

                for (int i = tempList.Count - 1; i >= 0; i--)
                {
                    if (tempList[i] is SplineTarget)
                    {
                        splineCount++;
                        if (splineCount > 1)
                        {
                            tempList.Remove(tempList[i]);
                        }
                    }
                }
                
                // foreach (Target t in tempList)
                // {
                //     if (t is SplineTarget)
                //     {
                //         splineCount++;
                //         if (splineCount > 1)
                //         {
                //             tempList.Remove(t);
                //         }
                //     }
                // }

                targets = tempList.ToArray();
            }

            return targets;
        }

        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
        }

        private void OnDisable()
        {
            EnhancedTouchSupport.Disable();
        }
    }
}

