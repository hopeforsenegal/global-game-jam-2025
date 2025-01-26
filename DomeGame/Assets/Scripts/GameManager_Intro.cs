
public partial class GameManager
{
    private void StartIntro()
    {
        Screen = GameScreens.Intro; 
    }

    private void HandleIntro()
    {
        Screen = GameScreens.Core;
    }
}