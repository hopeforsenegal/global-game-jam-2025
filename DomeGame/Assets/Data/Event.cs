using System;
using UnityEngine;

[Serializable]
public class DialogPart
{
    public String dialogText;
    public Choice[] choices;
}

[Serializable]
public class Choice
{
    public String choiceText;
    public DialogPart[] dialogIfSelected;
    // TODO: Extra credit
    // public Choice[] subChoices;
}

[CreateAssetMenu(fileName = "Event", menuName = "Event", order = 1)]
public class Event : ScriptableObject
{
    public DialogPart[] dialog;
}
