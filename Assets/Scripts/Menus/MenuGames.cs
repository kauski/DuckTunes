using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using DuckTunes.Systems;

public class MenuGames : MonoBehaviour
{

    public VisualElement Lose;
    public VisualElement Pause;
    public VisualElement Win;
    public VisualElement pauseUI;

    public void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        Win = root.Q<VisualElement>("Win");
        Lose = root.Q<VisualElement>("Lose");
        Pause = root.Q<VisualElement>("Pause");
        pauseUI = root.Q<VisualElement>("GamePause");
        Button Winmainmenu = root.Q<Button>("Winmenu");
        Button WinRetry = root.Q<Button>("WinRetry"); // retry

        Button LoseBack = root.Q<Button>("LoseRetry");
        Button LoseMenu = root.Q<Button>("LoseMenu");

        Button Pausemenu = root.Q<Button>("home");
        Button Pauseback = root.Q<Button>("back");

        Button PauseGame = root.Q<Button>("PauseBut");

        PauseGame.clicked += this.Paused;
        LoseMenu.clicked += this.MainMenuButtonClicked;
        LoseBack.clicked += this.RetryButtonClicked;
        Winmainmenu.clicked += this.MainMenuButtonClicked;
        WinRetry.clicked += this.RetryButtonClicked;

        Pausemenu.clicked += this.MainMenuButtonClicked;
        Pauseback.clicked += this.PauseReturnClicked;

    }
    private void Start()
    {
        Win.style.display = DisplayStyle.None;
        Lose.style.display = DisplayStyle.None;
        Pause.style.display = DisplayStyle.None;
        
        pauseUI.style.display = DisplayStyle.Flex;
    }

  private void PauseReturnClicked()
    {
        Pause.style.display = DisplayStyle.None;
        pauseUI.style.display = DisplayStyle.Flex;
        GameManager.Instance.UnPauseGame();

    }
    private void Paused()
    {
        Pause.style.display = DisplayStyle.Flex;
        pauseUI.style.display = DisplayStyle.None;
        Debug.Log("game is pausedasdf");
        GameManager.Instance.PauseGame();
    }

    private void MainMenuButtonClicked()
    {
        SceneManager.LoadScene("Kakaraloota");
    }

    private void RetryButtonClicked()
    {
        GameManager.Instance.UnPauseGame();
        SceneManager.LoadScene("MainScene2");
       
    }


    public void GameFail()
    {
        Lose.style.display = DisplayStyle.Flex;
        pauseUI.style.display = DisplayStyle.None;
    }
    public void GameWin()
    {
        Win.style.display = DisplayStyle.Flex;
        pauseUI.style.display = DisplayStyle.None;
    }

    public void GamePause()
    {
        Pause.style.display = DisplayStyle.Flex;
        pauseUI.style.display = DisplayStyle.None;
    }




}
