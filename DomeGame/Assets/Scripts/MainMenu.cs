using MoonlitSystem.UI.Immediate;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    protected void Update()
    {
        ImmediateStyle.CanvasGroup("/Canvas/MainMenua071");
        if (ImmediateStyle.Button("/Canvas/Image/Button (Legacy)c95e").IsMouseDown) {

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
