using System;
using MoonlitSystem.UI.Immediate;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    public int currentTurn = 0;
    public int maxTurns = 5;
    public int currentCitizenPopulation = 100;
    public int currentFood = 1000;
    public int currentUranium = 100;
    public int currentWater = 0;

    public int rateFoodPerCitizen = 1;
    public int rateUraniumPerCitizen = 1;
    public int rateWaterPerCitizen = 1;
    public int rateWaterPerUranium = 1;

    public int surplusFoodToGrowOneCitizenPerTurn = 10;

    public int rateCitizenDeathPerNoFood = 1;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    int citizenGrowthPerTurn(){
        if (currentFood >= currentCitizenPopulation) {
            return Math.Max(currentFood-currentCitizenPopulation,0)/surplusFoodToGrowOneCitizenPerTurn;
        }
        return (currentFood - currentCitizenPopulation) * rateCitizenDeathPerNoFood;
    }

    int foodGrowthPerTurn(){
        return -rateFoodPerCitizen*currentCitizenPopulation;
    }

    int uraniumGrowthPerTurn(){
        return -rateUraniumPerCitizen*currentCitizenPopulation;
    }

    // Update is called once per frame
    void Update()
    {
        ImmediateStyle.Text("/Canvas/FoodTexte6fc", "Food:" + currentFood + "(" + foodGrowthPerTurn() + "/turn)");
        ImmediateStyle.Text("/Canvas/FoodText (1)4f43", "Population:" + currentCitizenPopulation + "(" + citizenGrowthPerTurn() + "/turn)");
        ImmediateStyle.Text("/Canvas/UraniumTextc4b3", "Uranium:" + currentUranium + "(" + uraniumGrowthPerTurn() + "/turn)");
        ImmediateStyle.Text("/Canvas/TurnText366e", "Turn:" + currentTurn);

    
        if (currentTurn == maxTurns) {
            if (currentCitizenPopulation > 0) {
                ImmediateStyle.Text("/Canvas/EndText82ef", "You Win");

            } else {
                ImmediateStyle.Text("/Canvas/EndText82ef", "GameOver");

            }
            return;
        }


        if (Input.GetKeyDown(KeyCode.Space)) {
            int citizenGrowth = citizenGrowthPerTurn();
            int foodGrowth = foodGrowthPerTurn();
            int uraniumGrowth = uraniumGrowthPerTurn();
            currentCitizenPopulation = math.max(citizenGrowth + currentCitizenPopulation, 0);
            currentFood = math.max(foodGrowth + currentFood, 0);
            currentUranium = math.max(uraniumGrowth + currentUranium, 0);

            currentTurn = currentTurn + 1;
        }
    }
}
