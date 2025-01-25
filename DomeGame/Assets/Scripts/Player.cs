using System.Collections;
using System.Collections.Generic;
using MoonlitSystem.UI.Immediate;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int population = 1;
    public int unitFood = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ImmediateStyle.Text("/Canvas/FoodTexte6fc", "Food: " + unitFood);
        ImmediateStyle.Text("/Canvas/FoodText (1)4f43", "Population: " + population);
    }
}
