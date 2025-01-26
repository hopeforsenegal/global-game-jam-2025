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
    public EventEffects effect;
    public Event followUpEvent;
}

[Serializable]
public class EventEffects {
    public int coinsRequired;
    public int foodRequired;
    public int waterRequired;
    public int uraniumRequired;
    public int populationRequired;

    public int coinsGained;
    public int foodGained;
    public int waterGained;
    public int uraniumGained;
    public int populationGained;
}



[CreateAssetMenu(fileName = "Event", menuName = "Event", order = 1)]
public class Event : ScriptableObject
{
    public DialogPart[] dialog;
}
