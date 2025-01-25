using System;
using System.Collections;
using System.Collections.Generic;
using MoonlitSystem.UI.Immediate;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class RandomEvents : MonoBehaviour
{
    public Event[] eventsData;
    private Event selectedEvent;
    private TypingEffect typingEffect;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");

        selectedEvent = eventsData[UnityEngine.Random.Range(0, eventsData.Length)];
        typingEffect = new TypingEffect();
        typingEffect.fullText = selectedEvent.description;
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space)) {
        //     Debug.Log("Hit space");
        // }

        TypingEffect.HandleTypingEffect(ref typingEffect, 0.1f);
        ImmediateStyle.Text("/Canvas/Event Description4f6d", typingEffect.currentText);

        if (selectedEvent.choices.Length > 1)
        {
            ImmediateStyle.Text("/Canvas/Button A/Text A6222", selectedEvent.choices[0].text);
            if (ImmediateStyle.Button("/Canvas/Button A3acd").IsMouseDown)
            {
                Debug.Log("Button 1");
            }
            ImmediateStyle.Text("/Canvas/Button B/Text Bf90d", selectedEvent.choices[1].text);
            if (ImmediateStyle.Button("/Canvas/Button Bd2b9").IsMouseDown)
            {
                Debug.Log("Button 2");
            }

            if (selectedEvent.choices.Length > 2)
            {
                ImmediateStyle.Text("/Canvas/Button C/Text Cc803", selectedEvent.choices[2].text);
                if (ImmediateStyle.Button("/Canvas/Button C2345").IsMouseDown)
                {
                    Debug.Log("Button 3");
                }
            }
        }
    }
}
