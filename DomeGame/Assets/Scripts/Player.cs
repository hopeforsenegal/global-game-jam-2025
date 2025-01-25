using System;
using MoonlitSystem.UI.Immediate;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] int currentTurn = 0;
    [SerializeField] int maxTurns = 5;
    [SerializeField] int currentCitizenPopulation = 100;
    [SerializeField] int currentFood = 1000;
    [SerializeField] int currentUranium = 100;
    [SerializeField] int currentWater = 0;
    [SerializeField] int currentCoin = 0;

    [SerializeField] int rateFoodPerCitizen = 1;
    [SerializeField] int rateUraniumPerCitizen = 1;
    [SerializeField] int rateWaterPerCitizen = 1;
    [SerializeField] int rateWaterPerUranium = 1;
    [SerializeField] int rateCoinPerTurn = 0;

    [SerializeField] int foodGeneratedPerAssignedCitizen = 100;
    [SerializeField] int coinGeneratedPerAssignedCitizen = 100;
    [SerializeField] int waterGeneratedPerAssignedCitizen = 100;

    [SerializeField] int uraniumGeneratedPerAssignedCitizen = 100;


    [SerializeField] int surplusFoodToGrowOneCitizenPerTurn = 10;

    [SerializeField] int rateCitizenDeathPerNoFood = 1;

    public int CalculateRateFoodGivenCitizen(int citizen) {
        return rateFoodPerCitizen * citizen + foodGrowthPerTurn();
    }

    public int CalculateRateUraniumGivenCitizen(int citizen) {
        return rateUraniumPerCitizen * citizen + uraniumGrowthPerTurn();
    }

    public int CalculateRateWaterGivenCitizen(int citizen) {
        return rateWaterPerCitizen * citizen + waterGrowthPerTurn();
    }

    public int CalculateRateCoinGivenCitizen(int citizen) {
        return citizen + coinGrowthPerTurn();
    }

    public int CitizenGrowthPerTurn(){
        if (currentFood >= currentCitizenPopulation) {
            return Math.Max(currentFood-currentCitizenPopulation,0)/surplusFoodToGrowOneCitizenPerTurn;
        }
        return (currentFood - currentCitizenPopulation) * rateCitizenDeathPerNoFood;
    }

    public int GetCurrentCitizenPopulation(){
        return currentCitizenPopulation;
    }

    public int GetCurrentFood(){
        return currentFood;
    }

    public int GetCurrentWater(){
        return currentWater;
    }

    public int GetCurrentUranium(){
        return currentUranium;
    }

    public int GetCurrentCoin(){
        return currentCoin;
    }

    public int GetCurrentTurn() {
        return currentTurn;
    }

    int foodGrowthPerTurn(){
        return -rateFoodPerCitizen*currentCitizenPopulation;
    }

    int uraniumGrowthPerTurn(){
        return -rateUraniumPerCitizen*currentCitizenPopulation;
    }

    int waterGrowthPerTurn(){
        return -rateWaterPerCitizen*currentCitizenPopulation + -rateWaterPerUranium*currentUranium;
    }

    int coinGrowthPerTurn(){
        return rateCoinPerTurn;
    }

    // Update is called once per frame
    void Update()
    {
        ImmediateStyle.Text("/Canvas/CoinText4142", "Coin: " + currentCoin + "(" + coinGrowthPerTurn() + "/turn)");
        ImmediateStyle.Text("/Canvas/FoodTexte6fc", "Food:" + currentFood + "(" + foodGrowthPerTurn() + "/turn)");
        ImmediateStyle.Text("/Canvas/FoodText (1)4f43", "Population:" + currentCitizenPopulation + "(" + CitizenGrowthPerTurn() + "/turn)");
        ImmediateStyle.Text("/Canvas/UraniumTextc4b3", "Uranium:" + currentUranium + "(" + uraniumGrowthPerTurn() + "/turn)");
        ImmediateStyle.Text("/Canvas/WaterText798e", "Water:" + currentWater + "(" + waterGrowthPerTurn() + "/turn)");
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
            int citizenGrowth = CitizenGrowthPerTurn();
            int foodGrowth = foodGrowthPerTurn();
            int uraniumGrowth = uraniumGrowthPerTurn();
            int waterGrowth = waterGrowthPerTurn();
            int coinGrowth = coinGrowthPerTurn();
            currentCitizenPopulation = math.max(citizenGrowth + currentCitizenPopulation, 0);
            currentFood = math.max(foodGrowth + currentFood, 0);
            currentUranium = math.max(uraniumGrowth + currentUranium, 0);
            currentWater = math.max(waterGrowth + currentWater, 0);
            currentCoin = math.max(coinGrowth + currentCoin, 0);    
            currentTurn = currentTurn + 1;
        }
    }
}
