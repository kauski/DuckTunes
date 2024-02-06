using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UpdateUI : MonoBehaviour
{
    public SaveState savestate;
    public TextMeshProUGUI DisplayScore;
    private int _Score;
    void Start()
    {
        _Score = 0;
    }



   public void Updatescore(int Newscore)
    {
        
        _Score += Newscore;
        DisplayScore.text = "Score: " + _Score.ToString();
        savestate.Tomato += Newscore;
    }

}
