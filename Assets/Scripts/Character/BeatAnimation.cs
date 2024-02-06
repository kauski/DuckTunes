using System;
using UnityEngine;
using DG.Tweening;
using DuckTunes.Systems;
using DuckTunes.Systems.Music;

namespace DuckTunes.CharacterFunctionality
{
    public enum BeatAmount
    {
        Four,
        Eight,
        Sixteen,
    }

    public enum AnimationType
    {
        None,
        ResetToOriginal,
        
        ScaleYUp,
        ScaleYDown,
        ScaleXUp,
        ScaleXDown,
        ScaleUp,
        ScaleDown,
        
        RotateLeft,
        RotateRight,
        
        MoveLeft,
        MoveRight,
        MoveUp,
        MoveDown,
    }

    [Serializable]
    public class AnimationAction
    {
        public AnimationType Type;
        public float DurationInBeats;
    }
    
     public class BeatAnimation : MonoBehaviour
     {
         public BeatAmount BeatAmountMode;
         public AnimationAction[] Animations;

         [Header("Settings")]
         [SerializeField] private float _yMinScale = 0.5f;
         [SerializeField] private float _yMaxScale = 1f;
         [SerializeField] private float _xMinScale = 0.5f;
         [SerializeField] private float _xMaxScale= 1f;
         [SerializeField] private float _minUniformScale = 0.5f;
         [SerializeField] private float _maxUniformScale= 1f;
         
         [SerializeField] private float _xMove = 0.5f;
         [SerializeField] private float _yMove = 0.5f;
         
         
         
         [SerializeField] private Ease _sqashEase;

         [SerializeField] private float _rotationAmount = 5f;
         
         private Conductor _conductor;

         private int _beatCounter;
         private Vector3 _originalScale;
         private Vector3 _originalRotate;
         private Vector3 _originalPosition;
        
        private void Awake()
        {
            _conductor = GameManager.Conductor;
            _originalScale = gameObject.transform.localScale;
            _originalRotate = gameObject.transform.eulerAngles;
            _originalPosition = gameObject.transform.position;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            switch (BeatAmountMode)
            {
                case BeatAmount.Four:
                    if (Animations.Length != 4 || Animations == null)
                    {
                        Array.Resize(ref Animations, 4);
                    }
                    break;

                case BeatAmount.Eight:
                    if (Animations.Length != 8 || Animations == null)
                    {
                        Array.Resize(ref Animations, 8);
                    }
                    break;

                case BeatAmount.Sixteen:
                    if (Animations.Length != 16 || Animations == null)
                    {
                        Array.Resize(ref Animations, 16);
                    } 
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
#endif
        
        public void OnBeat()
        {
            var beatAmount = Animations.Length;
            _beatCounter++;
            var beatDur = _conductor.GetBeatDuration();

             switch (_beatCounter)
             {
                 case 1:
                     DoAnimation(Animations[0], beatDur);
                     break;
                 case 2:
                     DoAnimation(Animations[1], beatDur);
                     break;
                 case 3:
                     DoAnimation(Animations[2], beatDur);
                     break;
                 case 4:
                     DoAnimation(Animations[3], beatDur);
                     CheckBeatCounter(beatAmount, _beatCounter);
                     break;
                 case 5:
                     DoAnimation(Animations[4], beatDur);
                     break;
                 case 6:
                     DoAnimation(Animations[5], beatDur);
                     break;
                 case 7:
                     DoAnimation(Animations[6], beatDur);
                     break;
                 case 8:
                     DoAnimation(Animations[7], beatDur);
                     CheckBeatCounter(beatAmount, _beatCounter);
                     break;
                 case 9:
                     DoAnimation(Animations[8], beatDur);
                     break;
                 case 10:
                     DoAnimation(Animations[9], beatDur);
                     break;
                 case 11:
                     DoAnimation(Animations[10], beatDur);
                     break;
                 case 12:
                     DoAnimation(Animations[11], beatDur);
                     break;
                 case 13:
                     DoAnimation(Animations[12], beatDur);
                     break;
                 case 14:
                     DoAnimation(Animations[13], beatDur);
                     break;
                 case 15:
                     DoAnimation(Animations[14], beatDur);
                     break;
                 case 16:
                     DoAnimation(Animations[15], beatDur);
                     CheckBeatCounter(beatAmount, _beatCounter);
                     break;
                 default:
                     throw new OverflowException();
            }
        }

        private void CheckBeatCounter(int beatAmount, int beatCounter)
        {
            if (beatCounter == beatAmount)
            {
                _beatCounter = 0;
            }
        }

        private void DoAnimation(AnimationAction animationAction, float beatDur)
        {
            switch (animationAction.Type)
            {
                case AnimationType.None:
                    break;
                case AnimationType.ResetToOriginal:
                    gameObject.transform.DOMove(_originalPosition, beatDur * animationAction.DurationInBeats).SetEase(_sqashEase);
                    gameObject.transform.DOScale(_originalScale, beatDur * animationAction.DurationInBeats)
                        .SetEase(_sqashEase);
                    gameObject.transform.DORotate(_originalRotate, beatDur * animationAction.DurationInBeats)
                        .SetEase(_sqashEase);
                    break;
                case AnimationType.ScaleYUp:
                    gameObject.transform.DOScaleY(_yMaxScale, beatDur * animationAction.DurationInBeats).SetEase(_sqashEase);
                    break;
                case AnimationType.ScaleYDown:
                    gameObject.transform.DOScaleY(_yMinScale, beatDur * animationAction.DurationInBeats).SetEase(_sqashEase);
                    break;
                case AnimationType.ScaleXUp:
                    gameObject.transform.DOScaleX(_xMaxScale, beatDur * animationAction.DurationInBeats).SetEase(_sqashEase);
                    break;
                case AnimationType.ScaleXDown:
                    gameObject.transform.DOScaleX(_xMinScale, beatDur * animationAction.DurationInBeats).SetEase(_sqashEase);
                    break;
                case AnimationType.ScaleUp:
                    gameObject.transform.DOScale(_maxUniformScale, beatDur * animationAction.DurationInBeats).SetEase(_sqashEase);
                    break;
                case AnimationType.ScaleDown:
                    gameObject.transform.DOScale(_minUniformScale, beatDur * animationAction.DurationInBeats).SetEase(_sqashEase);
                    break;
                case AnimationType.RotateLeft:
                    gameObject.transform
                        .DORotate(
                            new Vector3(_originalRotate.x, _originalRotate.y, _originalRotate.z + _rotationAmount),
                            beatDur * animationAction.DurationInBeats).SetEase(_sqashEase);
                    break;
                case AnimationType.RotateRight:
                    gameObject.transform
                        .DORotate(
                            new Vector3(_originalRotate.x, _originalRotate.y, _originalRotate.z - _rotationAmount),
                            beatDur * animationAction.DurationInBeats).SetEase(_sqashEase);
                    break;
                case AnimationType.MoveLeft:
                    gameObject.transform
                        .DOMoveX(_originalPosition.x - _xMove, beatDur * animationAction.DurationInBeats)
                        .SetEase(_sqashEase);
                    break;
                case AnimationType.MoveRight:
                    gameObject.transform
                        .DOMoveX(_originalPosition.x + _xMove, beatDur * animationAction.DurationInBeats)
                        .SetEase(_sqashEase);
                    break;
                case AnimationType.MoveUp:
                    gameObject.transform
                        .DOMoveY(_originalPosition.y + _yMove, beatDur * animationAction.DurationInBeats)
                        .SetEase(_sqashEase);
                    break;
                case AnimationType.MoveDown:
                    gameObject.transform
                        .DOMoveY(_originalPosition.y - _yMove, beatDur * animationAction.DurationInBeats)
                        .SetEase(_sqashEase);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(animationAction), animationAction, null);
            }
        }

        public void ClearTweens()
        {
            DOTween.KillAll();
        }
     }
}
