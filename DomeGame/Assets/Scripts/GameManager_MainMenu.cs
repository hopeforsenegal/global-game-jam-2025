using MoonlitSystem.UI.Immediate;
using UnityEngine;

public partial class GameManager 
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
}