using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class GameManager : MonoBehaviour
{
    public enum GameScreens { MainMenu, Intro, Core, RandomEvents, }
    private GameScreens Screen;

    [SerializeField] AudioClip clickSound;

    protected void Start()
    {
        StartIntro();
        StartRealCore();
        StartCore();
        StartRandomEvents();
        StartMainMenu();
    }

    protected void Update()
    {
        switch (Screen) {
            case GameScreens.MainMenu: HandleMainMenu(); break;
            case GameScreens.Intro: HandleIntro(); break;
            case GameScreens.Core: HandleCore(); HandleRealCore(); break;
            case GameScreens.RandomEvents: HandleRandomEvents(); break;
            default: throw new ArgumentOutOfRangeException(Screen.ToString());
        }

        if (Screen != GameScreens.MainMenu && Input.GetKeyDown(KeyCode.Escape)) {
            Start();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.W)) { Debug.Log("W"); StartCore(); }
        if (Input.GetKeyDown(KeyCode.E)) { Debug.Log("E"); StartRealCore(); }
        if (Input.GetKeyDown(KeyCode.R)) { Debug.Log("R"); StartRandomEvents(); }
#endif
    }
}