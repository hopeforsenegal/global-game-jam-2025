using System;
using UnityEngine;

[Serializable]
public class Choice
{
    public String text;
}

[CreateAssetMenu(fileName = "Event", menuName = "Event", order = 1)]
public class Event : ScriptableObject
{
    public Sprite sprite;
    public String description;
    public Choice[] choices;
}
