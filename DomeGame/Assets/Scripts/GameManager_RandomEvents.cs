using System;
using System.Collections.Generic;
using System.Linq;
using MoonlitSystem.UI.Immediate;
using UnityEngine;
using UnityEngine.UIElements;

public partial class GameManager
{
    [Space]
    [Header("Random Events")]
    public Event[] randomEventsData;
    private Event[] selectableEvents;
    // private List<Event> selectableEvents;
    private HashSet<Event> alreadyViewedEvents = new HashSet<Event>();
    private Event selectedEvent;
    private TypingEffect typingEffect;
    private uint dialogIndex;
    private Choice selectedChoice;
    private uint choiceDialogIndex;

    private bool eventFinished;

    // Extra credit: follow-up events
    private List<Event> followUpEvents = new List<Event>();
    private bool isFollowUpEventsEnabled = false;

    // Start is called before the first frame update
    void StartRandomEvents()
    {
        Screen = GameScreens.RandomEvents;
        if (currentTurn == 0){
            alreadyViewedEvents.Clear();
            selectedEvent = null;
            selectedChoice = null;
            choiceDialogIndex = 0;
            dialogIndex = 0;
            eventFinished = false;
        }
        selectableEvents = randomEventsData
            .Where(eventData => !alreadyViewedEvents.Contains(eventData))
            .ToArray<Event>();
        if (selectableEvents.Length == 0)
        //     .ToList<Event>();
        // if (selectableEvents.Count == 0)
        {
            Screen = GameScreens.Core;
            eventFinished = true;
        }
        else
        {
            dialogIndex = 0;
            selectedChoice = null;
            choiceDialogIndex = 0;
            eventFinished = false;
            selectedEvent = selectableEvents[
                UnityEngine.Random.Range(0, selectableEvents.Length)];
                // UnityEngine.Random.Range(0, selectableEvents.Count)];
            typingEffect = new TypingEffect();
            typingEffect.fullText =
                processText(selectedEvent.dialog[dialogIndex].dialogText);
        }
    }

    bool isChoiceRequirementsMet(EventEffects effect) {
        if (effect == null) {
            return false;
        }
        if (effect.coinsRequired > currentCoin) {
            return false;
        }
        if (effect.foodRequired > currentFood) {
            return false;
        }
        if (effect.uraniumRequired > currentUranium) {
            return false;
        }
        if (effect.waterRequired > currentWater) {
            return false;
        }
        if (effect.populationRequired > 0 && effect.populationRequired >= currentCitizenPopulation) {
            return false;
        }
        return true;
    }

    // Update is called once per frame
    void HandleRandomEvents()
    {
        ImmediateStyle.CanvasGroup("/Prefab Mode in Context/RandomEvents97e6");

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (dialogIndex < selectedEvent.dialog.Length && selectedEvent.dialog[dialogIndex].choices.Length == 0)
            {
                handleRenderNextDialog();
            }
            else if (selectedChoice != null)
            { 
                choiceDialogIndex++;

                if (choiceDialogIndex < selectedChoice.dialogIfSelected.Length) {
                    typingEffect = new TypingEffect();
                    typingEffect.fullText =
                        processText(selectedChoice.dialogIfSelected[choiceDialogIndex].dialogText);
                } else {
                    handleRenderNextDialog();
                }
            }
        }

        // Make text speed super fast!
        TypingEffect.HandleTypingEffect(ref typingEffect, Single.Epsilon);
        ImmediateStyle.Text("/Canvas/Event Description Background/Event Descriptiond52a", typingEffect.currentText);
        if (selectedChoice == null && selectedEvent.dialog[dialogIndex].choices.Length > 1) {
            // TODO: After implementing more than 1 Event Sprite
            // ImmediateStyle.Image("/Canvas/Event Sprite5b8c", selectedEvent.sprite);

            var colorA = Color.white;
            if (!isChoiceRequirementsMet(selectedEvent.dialog[dialogIndex].choices[0].effect)) colorA = Color.red;
            ImmediateStyle.SetColor(colorA);

            ImmediateStyle.Text("/Canvas/Button A/Text A6222", processText(selectedEvent.dialog[dialogIndex].choices[0].choiceText));
            if (ImmediateStyle.Button("/Canvas/Button A3acd").IsMouseDown && isChoiceRequirementsMet(selectedEvent.dialog[dialogIndex].choices[0].effect)) {
                // Debug.Log("Button 1");
                handleSelectedChoice(selectedEvent.dialog[dialogIndex].choices[0]);
            }
            ImmediateStyle.ClearColor();


            var colorB = Color.white;
            if (!isChoiceRequirementsMet(selectedEvent.dialog[dialogIndex].choices[1].effect)) colorB = Color.red;
            ImmediateStyle.SetColor(colorB);

            ImmediateStyle.Text("/Canvas/Button B/Text Bf90d", processText(selectedEvent.dialog[dialogIndex].choices[1].choiceText));
            if (ImmediateStyle.Button("/Canvas/Button Bd2b9").IsMouseDown && isChoiceRequirementsMet(selectedEvent.dialog[dialogIndex].choices[1].effect)) {
                // Debug.Log("Button 2");
                handleSelectedChoice(selectedEvent.dialog[dialogIndex].choices[1]);
            }
            ImmediateStyle.ClearColor();

            if (selectedEvent.dialog[dialogIndex].choices.Length > 2) {
                var colorC = Color.white;
                if (!isChoiceRequirementsMet(selectedEvent.dialog[dialogIndex].choices[2].effect)) colorC = Color.red;
                ImmediateStyle.SetColor(colorC);

                ImmediateStyle.Text("/Canvas/Button C/Text Cc803", processText(selectedEvent.dialog[dialogIndex].choices[2].choiceText));
                if (ImmediateStyle.Button("/Canvas/Button C2345").IsMouseDown && isChoiceRequirementsMet(selectedEvent.dialog[dialogIndex].choices[2].effect)) {
                    // Debug.Log("Button 3");
                    handleSelectedChoice(selectedEvent.dialog[dialogIndex].choices[2]);
                }
                ImmediateStyle.ClearColor();

                if (selectedEvent.dialog[dialogIndex].choices.Length == 4) {
                    var colorD = Color.white;
                    if (!isChoiceRequirementsMet(selectedEvent.dialog[dialogIndex].choices[3].effect)) colorD = Color.red;
                    ImmediateStyle.SetColor(colorD);

                    ImmediateStyle.Text("/Canvas/Button D/Text D965b", processText(selectedEvent.dialog[dialogIndex].choices[3].choiceText));
                    if (ImmediateStyle.Button("/Canvas/Button D3661").IsMouseDown && isChoiceRequirementsMet(selectedEvent.dialog[dialogIndex].choices[3].effect)) {
                        // Debug.Log("Button 4");
                        handleSelectedChoice(selectedEvent.dialog[dialogIndex].choices[3]);
                    }
                    ImmediateStyle.ClearColor();
                }
            }
        }
    }

    private String processText(String unprocessedText)
    {
        return unprocessedText
            .Replace("\\n", System.Environment.NewLine)
            .Replace("\\u00A0", "\u00A0");
    }

    private void handleRenderNextDialog()
    {
        dialogIndex++;
        if (dialogIndex < selectedEvent.dialog.Length) {
            typingEffect = new TypingEffect();
            typingEffect.fullText =
                processText(selectedEvent.dialog[dialogIndex].dialogText);
        } else {
            // TODO: Switch back to Core Scene, and pass it the selected Choice
            alreadyViewedEvents.Add(selectedEvent);
            Screen = GameScreens.Core;
            eventFinished = true;
        }
    }

    private void handleSelectedChoice(Choice selectedChoice)
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(clickSound);

        this.selectedChoice = selectedChoice;
        choiceDialogIndex = 0;

        if (selectedChoice.followUpEvent != null)
        {
            followUpEvents.Add(selectedChoice.followUpEvent);
        }

        typingEffect = new TypingEffect();
        typingEffect.fullText =
            processText(selectedChoice.dialogIfSelected[choiceDialogIndex].dialogText);
    }
}
