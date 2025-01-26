using System;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    void Start()
    {
        StartRealCore();
        StartCore();
    }

    private void HandleRandomEvents()
    {
    }

    public enum GameScreens { MainMenu, RandomEvents, Core }
    private GameScreens Screen;

    protected void Update()
    {
        switch (Screen) {
            case GameScreens.MainMenu: HandleMainMenu(); break;
            case GameScreens.Core: HandleCore(); HandleRealCore(); break;
            case GameScreens.RandomEvents: HandleRandomEvents(); break;
            default: throw new ArgumentOutOfRangeException(Screen.ToString());
        }

        if (Input.GetKeyDown(KeyCode.Q)) { Debug.Log("Q"); Screen = GameScreens.MainMenu; m_MainMenuState = default; }
        if (Input.GetKeyDown(KeyCode.W)) { Debug.Log("W"); Screen = GameScreens.Core; StartCore(); }
        if (Input.GetKeyDown(KeyCode.E)) { Debug.Log("E"); Screen = GameScreens.Core; StartRealCore(); }
    }
}