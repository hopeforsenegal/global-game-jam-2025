using MoonlitSystem.UI.Immediate;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private float fadeStartTime;
    private bool isFading;

    protected void Update()
    {
        if (!isFading) ImmediateStyle.CanvasGroup("/Canvas/MainMenua071");
        if (isFading) ImmediateStyle.CanvasGroup("/Canvas/MainMenua071", (cg) => { cg.alpha = Mathf.Lerp(1, 0, Time.time - fadeStartTime); });

        if (ImmediateStyle.Button("/Canvas/Image/Button (Legacy)c95e").IsMouseDown) {
            fadeStartTime = Time.time - 0.3f;
            isFading = true;
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
