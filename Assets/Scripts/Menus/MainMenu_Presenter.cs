using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu_Presenter : MonoBehaviour
{
    
    void Awake()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        root.Q<Button>("PlayButton").clicked += () => Debug.Log("Play button clicked");    
        root.Q<Button>("Settings").clicked += () => Debug.Log("Settings button clicked");    
        root.Q<Button>("Quit").clicked += () => Debug.Log("Quit button clicked");
        root.Q<Button>("Makers").clicked += () => Debug.Log("Makers button clicked");    
    }


    void Update()
    {
        
    }
}
