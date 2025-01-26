using MoonlitSystem.UI.Immediate;
using UnityEngine;

public partial class GameManager
{
    public enum State { Intro1, Intro2, Intro3, }
    struct IntroState
    {
        public float fadeStartTime;
        public bool isFading;
        public State state;
    }
    IntroState m_IntroState;

    private void StartIntro()
    {
        Screen = GameScreens.Intro;
        m_IntroState = new IntroState { };
        m_IntroState.fadeStartTime = Time.time - 0.3f;
        m_IntroState.isFading = true;
        m_IntroState.state = State.Intro1;
    }

    private void HandleIntro()
    {
        ImmediateStyle.CanvasGroup("/Prefab Mode in Context/Intro09f6");

        if (!m_IntroState.isFading) ImmediateStyle.CanvasGroup("/Canvas (Environment)/Intro/Background (1)aeb8", (cg) => cg.alpha = 1);
        if (m_IntroState.isFading) ImmediateStyle.CanvasGroup("/Canvas (Environment)/Intro/Background (1)aeb8", (cg) => { cg.alpha = Mathf.Lerp(1, 0, Time.time - m_IntroState.fadeStartTime); });

        if (m_IntroState.state == State.Intro2) ImmediateStyle.Image("/Canvas (Environment)/Intro/Mask/DialougeBox (1)/CitizenToken5587");

        if (m_IntroState.state == State.Intro2) ImmediateStyle.CanvasGroup("/Canvas (Environment)/Intro/Intro39c8d");
        if (m_IntroState.state == State.Intro3) ImmediateStyle.CanvasGroup("/Canvas (Environment)/Intro/Intro27b15");

        var text = m_IntroState.state switch
        {
            State.Intro1 => gameSettings.intro1,
            State.Intro2 => gameSettings.intro2,
            State.Intro3 => gameSettings.intro3,
        };
        ImmediateStyle.Text("/Canvas (Environment)/Intro/Mask/DialougeBox (1)/Textaf0b", text);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            m_IntroState.state++;
            if (m_IntroState.state > State.Intro3) {
                StartRandomEvents();
                StartCore();
                StartRealCore();
            }
        }
    }
}