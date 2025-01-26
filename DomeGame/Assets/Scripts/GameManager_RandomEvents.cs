using System;
using MoonlitSystem.UI.Immediate;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public partial class GameManager
{
    [Space]
    [Header("Random Events")]
    public Event[] randomEventsData;
    private Event selectedEvent;
    private TypingEffect typingEffect;
    private uint dialogIndex = 0;
    private Choice selectedChoice;
    private uint choiceDialogIndex;

    // Start is called before the first frame update
    void StartRandomEvents()
    {
        Debug.Log("Start");

        selectedEvent = randomEventsData[UnityEngine.Random.Range(0, randomEventsData.Length)];
        typingEffect = new TypingEffect();
        typingEffect.fullText =
            selectedEvent.dialog[dialogIndex].dialogText.Replace("\\n", System.Environment.NewLine);
    }

    // Update is called once per frame
    void HandleRandomEvents()
    {
        ImmediateStyle.CanvasGroup("/Prefab Mode in Context/RandomEvents97e6");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Hit space");

            if (selectedEvent.dialog[dialogIndex].choices.Length == 0)
            {
                handleRenderNextDialog();
            }
            else if (selectedChoice != null)
            {
                choiceDialogIndex++;

                if (choiceDialogIndex < selectedChoice.dialogIfSelected.Length) {
                    typingEffect = new TypingEffect();
                    typingEffect.fullText =
                        selectedChoice.dialogIfSelected[choiceDialogIndex].dialogText.Replace("\\n", System.Environment.NewLine);
                } else {
                    handleRenderNextDialog();
                }
            }
        }

        TypingEffect.HandleTypingEffect(ref typingEffect, 0.01f);
        ImmediateStyle.Text("/Canvas/Event Description Background/Event Descriptiond52a", typingEffect.currentText);

        if (selectedChoice == null && selectedEvent.dialog[dialogIndex].choices.Length > 1) {
            // TODO: After implementing more than 1 Event Sprite
            // ImmediateStyle.Image("/Canvas/Event Sprite5b8c", selectedEvent.sprite);

            ImmediateStyle.Text("/Canvas/Button A/Text A6222", selectedEvent.dialog[dialogIndex].choices[0].choiceText);
            if (ImmediateStyle.Button("/Canvas/Button A3acd").IsMouseDown) {
                Debug.Log("Button 1");

                handleSelectedChoice(selectedEvent.dialog[dialogIndex].choices[0]);
            }

            ImmediateStyle.Text("/Canvas/Button B/Text Bf90d", selectedEvent.dialog[dialogIndex].choices[1].choiceText);
            if (ImmediateStyle.Button("/Canvas/Button Bd2b9").IsMouseDown) {
                Debug.Log("Button 2");

                handleSelectedChoice(selectedEvent.dialog[dialogIndex].choices[1]);
            }

            if (selectedEvent.dialog[dialogIndex].choices.Length > 2) {
                ImmediateStyle.Text("/Canvas/Button C/Text Cc803", selectedEvent.dialog[dialogIndex].choices[2].choiceText);
                if (ImmediateStyle.Button("/Canvas/Button C2345").IsMouseDown) {
                    Debug.Log("Button 3");

                    handleSelectedChoice(selectedEvent.dialog[dialogIndex].choices[2]);
                }

                if (selectedEvent.dialog[dialogIndex].choices.Length == 4) {
                    ImmediateStyle.Text("/Canvas/Button D/Text D965b", selectedEvent.dialog[dialogIndex].choices[3].choiceText);
                    if (ImmediateStyle.Button("/Canvas/Button D3661").IsMouseDown) {
                        Debug.Log("Button 4");

                        handleSelectedChoice(selectedEvent.dialog[dialogIndex].choices[3]);
                    }
                }
            }
        }
    }

    private void handleRenderNextDialog()
    {
        dialogIndex++;

        if (dialogIndex < selectedEvent.dialog.Length) {
            typingEffect = new TypingEffect();
            typingEffect.fullText =
                selectedEvent.dialog[dialogIndex].dialogText.Replace("\\n", System.Environment.NewLine);
        } else {
            // TODO: Switch back to Core Scene, and pass it the selected Choice
            Screen = GameScreens.Core;
        }
    }

    private void handleSelectedChoice(Choice selectedChoice)
    {
        this.selectedChoice = selectedChoice;
        choiceDialogIndex = 0;
        typingEffect = new TypingEffect();
        typingEffect.fullText =
            selectedChoice.dialogIfSelected[choiceDialogIndex].dialogText.Replace("\\n", System.Environment.NewLine);
    }
}
