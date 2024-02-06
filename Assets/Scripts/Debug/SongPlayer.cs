using DuckTunes.ScriptableObjects;
using DuckTunes.Systems;
using System.Collections;
using DuckTunes.Tutorial;
using UnityEngine;

public class SongPlayer : MonoBehaviour
{
    public TrackInfo TrackToPlay;
    public float startDelay = 0f;

    private Tutorial _tutorial;

    public void StartPlay()
    {
        _tutorial = FindObjectOfType<Tutorial>();
        if (_tutorial != null)
        {
            _tutorial.StartTutorial();
        }
        
        StartCoroutine(PlayAfterTime(startDelay));
    }

    private IEnumerator PlayAfterTime(float startDelay)
    {
        yield return new WaitForSeconds(startDelay);

        GameManager.Conductor.Play(TrackToPlay);
    }
}
