using System;
using System.Collections;
using System.Collections.Generic;
using MoonlitSystem.UI.Immediate;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class RandomEvents : MonoBehaviour
{
    public Event[] eventDatum;
    private Event eventData;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        eventData = eventDatum[UnityEngine.Random.Range(0, eventDatum.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Hit space");
        }

        ImmediateStyle.Text("/Canvas/Text (Legacy)263a", eventData.description);

        ImmediateStyle.Text("/Canvas/Button (Legacy)/Text (Legacy)bbb2", eventData.choices[0].text);
        ImmediateStyle.Text("/Canvas/Button (Legacy) (1)/Text (Legacy)c201", eventData.choices[1].text);

        if (eventData.choices.Length == 2) {
            // Do something about Button 3
        } else {
            ImmediateStyle.Text("/Canvas/Button (Legacy) (2)/Text (Legacy)2f07", eventData.choices[2].text);
            if (ImmediateStyle.Button("/Canvas/Button (Legacy) (2)cb6b").IsMouseDown) {
                Debug.Log("Button 3");
            }
        }

        if (ImmediateStyle.Button("/Canvas/Button (Legacy)8e95").IsMouseDown) {
            Debug.Log("Button 1");
        }
        if (ImmediateStyle.Button("/Canvas/Button (Legacy) (1)83f7").IsMouseDown) {
            Debug.Log("Button 2");
        }
    }
}
