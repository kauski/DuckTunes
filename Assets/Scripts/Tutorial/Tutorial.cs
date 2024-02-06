using System.Collections;
using UnityEngine;
using DG.Tweening;
using DuckTunes.CharacterFunctionality;
using DuckTunes.Systems;
using DuckTunes.Systems.Music;
using DuckTunes.Targets;
using UnityEngine.SceneManagement;
using System;

namespace DuckTunes.Tutorial
{
    public enum TutorialStep
    {
        Tomato,
        Spline,
        cleared,
    }

    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private float _startDelay;
        [SerializeField] private float _spawnDelay;

        [SerializeField] private float _duration;

        private SplineTarget _tutorialSpline;
        private GameObject _tutorialTomato;
        
        [SerializeField] private GameObject _tutorialFinger;
        [SerializeField] private GameObject _tutorialFingerHolder;

        [SerializeField] private GameObject _cheerMeterUI;
        [SerializeField] private Player _player;
        
        [SerializeField] private Conductor _conductor;

        [SerializeField] private TutorialStep _step;

        private int _index = 0;
        private int _successIndex = 0;

        
#region Public functions

        public void StartTutorial()
        {
            StartCoroutine(Initialize());
        }

#endregion


#region EventFunctions

        public void GoToNextStep()
        {
            if (_step == TutorialStep.cleared) { return; }

            _successIndex++;

            if (_successIndex >= 3)
            {
                CloseStep(_step);

                int s = (int)_step;
                s++;
                _step = (TutorialStep)s;
                _index = 0;
                _successIndex = 0;

                StartCoroutine(ActivateStep(_step));
            }
            else
            {
                RestartStep();
            }
        }

        public void RestartStep()
        {
            if (_step == TutorialStep.cleared) { return; }

            _player.ResetHP();
            
            switch (_step)
            {
                case TutorialStep.Tomato:
                    CloseStep(TutorialStep.Tomato);
                    StartCoroutine(ActivateStep(_step));
                    break;
                case TutorialStep.Spline:
                    CloseStep(TutorialStep.Spline);
                    StartCoroutine(ActivateStep(_step));
                    break;
                case TutorialStep.cleared:
                            
                    break;
            }
        }

        public void LoadMainMenu()
        {
            StartCoroutine(LoadAfterTime());
        }

        private IEnumerator LoadAfterTime()
        {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("Kakaraloota");
        }

        #endregion


        #region Private functions

        private void Start()
        {
            _conductor.StopSpawning();
            _cheerMeterUI.SetActive(false);
        }

        private IEnumerator Initialize()
        {
            yield return new WaitForSeconds(_startDelay);

            StartCoroutine(ActivateStep(TutorialStep.Tomato));
            
        }

        private IEnumerator PlayStep()
        {
            switch (_step)
            {
                case TutorialStep.Tomato:

                    _tutorialFinger.transform.DOScale(0.7f, 0.2f);
                    _tutorialFingerHolder.transform.DOMove(_tutorialTomato.transform.position, 1f).onComplete += () =>
                    {
                        _tutorialFinger.transform.DOScale(0.3f, 0.7f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
                    };
                        
                    yield return null;

                    break;
                case TutorialStep.Spline:

                    _tutorialFinger.transform.DOScale(0.7f, 0.2f);
                    _tutorialFingerHolder.transform.DOMove(_tutorialSpline.Data.Points[0], 1f).onComplete += () =>
                    {
                        _tutorialFinger.transform.DOScale(0.3f, 0.7f).onComplete += () =>
                        {
                            _tutorialFingerHolder.transform.DOPath(_tutorialSpline.Data.Points, (_duration - 1.7f) * 0.5f, PathType.Linear)
                                .SetLoops(-1);
                        };
                    };

                    yield return null;
                    break;
            }
        }

        private void CloseStep(TutorialStep step)
        {
            switch (step)
            {
                case TutorialStep.Tomato:
                    DOTween.PauseAll();
                    DOTween.Clear();
                    
                    break;
                case TutorialStep.Spline:
                    DOTween.PauseAll();
                    DOTween.Clear();

                    break;
            }
        }

        private IEnumerator ActivateStep(TutorialStep tutorialStep)
        {
            switch (tutorialStep)
            {
                case TutorialStep.Tomato:
                    yield return new WaitForSeconds(_spawnDelay);

                    var tomatoes = GameManager.TargetManager.GetAllPoolObjects(SpawnObject.Tomato);
                    foreach (var tomato in tomatoes)
                    {
                        tomato.GetComponent<Target>().Duration = _duration;
                    }
                    GameManager.TargetManager.SpawnTarget(SpawnObject.Tomato);
                    _tutorialTomato = tomatoes[_index];

                    _tutorialFinger.SetActive(true);

                    _index++;
                    _index = (int)Mathf.Repeat(_index, tomatoes.Length);

                    StartCoroutine(PlayStep());
                    break;
                
                
                
                case TutorialStep.Spline:
                    yield return new WaitForSeconds(_spawnDelay);

                    //set position
                    var splines = GameManager.TargetManager.GetAllPoolObjects(SpawnObject.Spline);
                    foreach (var spline in splines)
                    {
                        spline.GetComponent<Target>().Duration = _duration;
                    }
                    GameManager.TargetManager.SpawnTarget(SpawnObject.Spline);
                    _tutorialSpline = splines[_index].GetComponent<SplineTarget>();
                    
                    _tutorialFinger.SetActive(true);

                    _index++;
                    _index = (int)Mathf.Repeat(_index, splines.Length);

                    StartCoroutine(PlayStep());
                    break;

                case TutorialStep.cleared:

                    var clearedSplines = GameManager.TargetManager.GetAllPoolObjects(SpawnObject.Spline);
                    foreach (var spline in clearedSplines)
                    {
                        spline.GetComponent<Target>().Duration = 3;
                    }

                    var clearedTomatoes = GameManager.TargetManager.GetAllPoolObjects(SpawnObject.Tomato);
                    foreach (var tomato in clearedTomatoes)
                    {
                        tomato.GetComponent<Target>().Duration = 1;
                    }

                    yield return new WaitForSeconds(0.2f);

                    _tutorialFinger.SetActive(false);
                    DOTween.KillAll();
                    _cheerMeterUI.SetActive(true);
                    _player.ResetHP();
                    _conductor.StartSpawning();

                    break;
            }
        }

#endregion

        
    }
}
