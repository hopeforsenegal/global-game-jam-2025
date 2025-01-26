using System;
using MoonlitSystem.UI.Immediate;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    struct MainMenuState
    {
        public float fadeStartTime;
        public bool isFading;
    }
    MainMenuState m_MainMenuState;

    private void HandleMainMenu()
    {
        ImmediateStyle.CanvasGroup("/Canvas/MainMenubf45");

        if (!m_MainMenuState.isFading) ImmediateStyle.CanvasGroup("/Canvas/MainMenua071");
        if (m_MainMenuState.isFading) ImmediateStyle.CanvasGroup("/Canvas/MainMenua071", (cg) => { cg.alpha = Mathf.Lerp(1, 0, Time.time - m_MainMenuState.fadeStartTime); });

        if (ImmediateStyle.Button("/Canvas/Image/Button (Legacy)c95e").IsMouseDown) {
            m_MainMenuState.fadeStartTime = Time.time - 0.3f;
            m_MainMenuState.isFading = true;
        }
        if (ImmediateStyle.Button("/Canvas/Image/Button (Legacy) (1)1eee").IsMouseDown) {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }






    private void HandleCore()
    {
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
            case GameScreens.Core: HandleCore(); break;
            case GameScreens.RandomEvents: HandleRandomEvents(); break;
            default: throw new ArgumentOutOfRangeException(Screen.ToString());
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) { Screen = GameScreens.MainMenu; m_MainMenuState = default; }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { Screen = GameScreens.Core; }
    }
}