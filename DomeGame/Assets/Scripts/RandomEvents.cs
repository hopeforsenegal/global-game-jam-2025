using System;
using System.Collections;
using System.Collections.Generic;
using MoonlitSystem.UI.Immediate;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class RandomEvents : MonoBehaviour
{
    public Event[] randomEventsData;
    private Event selectedEvent;
    private TypingEffect typingEffect;
    private uint dialogIndex = 0;
    private uint choiceDialogIndex;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");

        selectedEvent = randomEventsData[UnityEngine.Random.Range(0, randomEventsData.Length)];
        typingEffect = new TypingEffect();
        typingEffect.fullText =
            selectedEvent.dialog[dialogIndex].dialogText.Replace("\\n", System.Environment.NewLine);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && selectedEvent.dialog[dialogIndex].choices.Length == 0) {
            Debug.Log("Hit space");
            dialogIndex++;

            if (dialogIndex < selectedEvent.dialog.Length)
            {
                typingEffect.fullText =
                    selectedEvent.dialog[dialogIndex].dialogText.Replace("\\n", System.Environment.NewLine);
            }
            else
            {
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
            }
        }

        TypingEffect.HandleTypingEffect(ref typingEffect, 0.1f);
        ImmediateStyle.Text("/Canvas/Event Description Background/Event Descriptiond52a", typingEffect.currentText);

        if (selectedEvent.dialog[dialogIndex].choices.Length > 1)
        {
            // TODO: After implementing more than 1 Event Sprite
            // ImmediateStyle.Image("/Canvas/Event Sprite5b8c", selectedEvent.sprite);

            ImmediateStyle.Text("/Canvas/Button A/Text A6222", selectedEvent.dialog[dialogIndex].choices[0].choiceText);
            if (ImmediateStyle.Button("/Canvas/Button A3acd").IsMouseDown)
            {
                Debug.Log("Button 1");
            }

            ImmediateStyle.Text("/Canvas/Button B/Text Bf90d", selectedEvent.dialog[dialogIndex].choices[1].choiceText);
            if (ImmediateStyle.Button("/Canvas/Button Bd2b9").IsMouseDown)
            {
                Debug.Log("Button 2");
            }

            if (selectedEvent.dialog[dialogIndex].choices.Length > 2)
            {
                ImmediateStyle.Text("/Canvas/Button C/Text Cc803", selectedEvent.dialog[dialogIndex].choices[2].choiceText);
                if (ImmediateStyle.Button("/Canvas/Button C2345").IsMouseDown)
                {
                    Debug.Log("Button 3");
                }

                if (selectedEvent.dialog[dialogIndex].choices.Length == 4)
                {
                    ImmediateStyle.Text("/Canvas/Button D/Text D965b", selectedEvent.dialog[dialogIndex].choices[3].choiceText);
                    if (ImmediateStyle.Button("/Canvas/Button D3661").IsMouseDown)
                    {
                        Debug.Log("Button 4");
                    }
                }
            }
        }
    }

    // private void handleSelectedChoice(Choice selectedChoice)
    // {
    //     choiceDialogIndex = 0;

    //     if (Input.GetKeyDown(KeyCode.Space) && selectedEvent.dialog[dialogIndex].choices.Length == 0) {
    //         Debug.Log("Hit space");
    //         dialogIndex++;

    //         if (dialogIndex < selectedEvent.dialog.Length)
    //         {
    //             typingEffect.fullText =
    //                 selectedEvent.dialog[dialogIndex].dialogText.Replace("\\n", System.Environment.NewLine);
    //         }
    //         else
    //         {
    //             #if UNITY_EDITOR
    //                 UnityEditor.EditorApplication.isPlaying = false;
    //             #else
    //                 Application.Quit();
    //             #endif
    //         }
    //     }
    // }
}
