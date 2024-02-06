using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using DuckTunes.Systems;

public class MenuController : MonoBehaviour
{
    public VisualElement Options;
    public VisualElement MainMenu;
    public VisualElement Credits;
    public float Volume;
    public void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        Slider MusicVolumeSlider = root.Q<Slider>("MusicVolumeSlider");
        Slider SFXSlider = root.Q<Slider>("SFXVolumeSlider");

        Options = root.Q<VisualElement>("M_Settings");
        MainMenu = root.Q<VisualElement>("MainMenu");
        Credits = root.Q<VisualElement>("Makers");
        Button tut = root.Q<Button>("tutorial");
        Button PlayButton = root.Q<Button>("PlayButton");
        Button Settings = root.Q<Button>("Settings");
        Button makers = root.Q<Button>("makers");
        Button quit = root.Q<Button>("quit");
        Button Return = root.Q<Button>("back");
        Button Returnm = root.Q<Button>("back_m");
        Settings.clicked += this.SettingsButtonClicked;
        PlayButton.clicked += this.PlayButtonOnClicked;

        makers.clicked += this.MakersButtonClicked;
        quit.clicked += this.QuitButtonOnClicked;
        Return.clicked += this.ReturnButtonClicked;
        Returnm.clicked += this.ReturnmButtonClicked;
        tut.clicked += this.TutorialLoad;


        MusicVolumeSlider.RegisterValueChangedCallback(SliderValue =>
        {
            if (SliderValue != null)
            {
                if (SliderValue.newValue <= 0f)
                {
                    this.Volume = 0.0001f;
                }
                else
                {
                    this.Volume = SliderValue.newValue;
                }

                GameManager.VolumeControl.SetMusicVolume(Volume);
                Debug.Log(this.Volume);
            }
        });

        SFXSlider.RegisterValueChangedCallback(SliderValue =>
        {
            if (SliderValue != null)
            {
                if (SliderValue.newValue <= 0f)
                {
                    this.Volume = 0.0001f;
                }
                else
                {
                    this.Volume = SliderValue.newValue;
                }

                GameManager.VolumeControl.SetEffectsVolume(Volume);
                Debug.Log(this.Volume);
            }
        });
    }

    private void TutorialLoad()
    {
        SceneManager.LoadScene("Tutorial");
    }
    private void Start()
    {
        MainMenu.style.display = DisplayStyle.Flex;

    }
    private void ReturnmButtonClicked()
    {
        MainMenu.style.display = DisplayStyle.Flex;
        Options.style.display = DisplayStyle.None;
    }





    private void PlayButtonOnClicked()
    {
        
        SceneManager.LoadScene("MainScene2");
    }

    private void SettingsButtonClicked()
    {
        Options.style.display = DisplayStyle.Flex;
        MainMenu.style.display = DisplayStyle.None;
        
    }

    private void MakersButtonClicked()
    {
        MainMenu.style.display = DisplayStyle.None;
        Credits.style.display = DisplayStyle.Flex;



    }
    private void ReturnButtonClicked()
    {
        MainMenu.style.display = DisplayStyle.Flex;
       
        Credits.style.display = DisplayStyle.None;
    }

    private void QuitButtonOnClicked() 
    {
        Application.Quit();
    }

   


}
