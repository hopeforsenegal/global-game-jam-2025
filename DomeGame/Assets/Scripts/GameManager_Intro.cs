using MoonlitSystem.UI.Immediate;
using UnityEngine;

public partial class GameManager
{
    struct IntroState
    {
        public float fadeStartTime;
        public bool isFading;
    }
    IntroState m_IntroState;

    private void StartIntro()
    {
        Screen = GameScreens.Intro;
        m_IntroState = new IntroState { };
        m_IntroState.fadeStartTime = Time.time - 0.3f;
        m_IntroState.isFading = true;
    }

    private void HandleIntro()
    {
        ImmediateStyle.CanvasGroup("/Prefab Mode in Context/Intro09f6");

        if (!m_IntroState.isFading) ImmediateStyle.CanvasGroup("/Canvas (Environment)/Intro/Background (1)aeb8", (cg) => cg.alpha = 1);
        if (m_IntroState.isFading) ImmediateStyle.CanvasGroup("/Canvas (Environment)/Intro/Background (1)aeb8", (cg) => { cg.alpha = Mathf.Lerp(1, 0, Time.time - m_IntroState.fadeStartTime); });

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            Screen = GameScreens.Core;
        }
    }
}