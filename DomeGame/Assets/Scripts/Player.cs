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

    [SerializeField] int foodGeneratedPerAssignedCitizen = 300;
    [SerializeField] int coinGeneratedPerAssignedCitizen = 300;
    [SerializeField] int waterGeneratedPerAssignedCitizen = 300;

    [SerializeField] int uraniumGeneratedPerAssignedCitizen = 300;

    [SerializeField] int assignedFood = 0;
    [SerializeField] int assignedWater = 0;
    [SerializeField] int assignedUranium = 0;
    [SerializeField] int assignedCoin = 0;


    [SerializeField] int surplusFoodToGrowOneCitizenPerTurn = 10;

    [SerializeField] int rateCitizenDeathPerNoResource = 1;
    [SerializeField] int rateCitizenDeathByBarrier = 1;
    [SerializeField] int requiredUraniumForBarrier = 100;

    public int CalculateRateFoodGivenCitizen(int citizen) {
        return foodGeneratedPerAssignedCitizen * citizen + foodGrowthPerTurn();
    }

    public int CalculateRateUraniumGivenCitizen(int citizen) {
        return coinGeneratedPerAssignedCitizen * citizen + uraniumGrowthPerTurn();
    }

    public int CalculateRateWaterGivenCitizen(int citizen) {
        return waterGeneratedPerAssignedCitizen * citizen + waterGrowthPerTurn();
    }

    public int CalculateRateCoinGivenCitizen(int citizen) {
        return uraniumGeneratedPerAssignedCitizen * citizen + coinGrowthPerTurn();
    }

    public int CitizenGrowthPerTurn(){
        return popGrowthByFood() - popDeathByLackResource() - popDeathByBarrier();
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
        return -rateFoodPerCitizen*currentCitizenPopulation + assignedFood*foodGeneratedPerAssignedCitizen;
    }

    int uraniumGrowthPerTurn(){
        return -rateUraniumPerCitizen*currentCitizenPopulation + assignedUranium*uraniumGeneratedPerAssignedCitizen;
    }

    int waterGrowthPerTurn(){
        return -rateWaterPerCitizen*currentCitizenPopulation + -rateWaterPerUranium*currentUranium + assignedWater*waterGeneratedPerAssignedCitizen;
    }

    int coinGrowthPerTurn(){
        return rateCoinPerTurn + assignedCoin*coinGeneratedPerAssignedCitizen;
    }

    int popDeathByBarrier(){
        if (currentUranium > requiredUraniumForBarrier) {
            return 0;
        }
        return (requiredUraniumForBarrier - currentUranium)*rateCitizenDeathByBarrier;
    }

    int popGrowthByFood(){
        if (currentFood < currentCitizenPopulation) {
            return 0;
        }
        return Math.Max(currentFood-currentCitizenPopulation,0)/surplusFoodToGrowOneCitizenPerTurn;
    }

    int popDeathByLackResource() {
        int maxResourceDeath = 0;
         if (currentFood < currentCitizenPopulation) {
            maxResourceDeath = Math.Max(currentCitizenPopulation - currentFood, 0);
        }
        if (currentWater < currentCitizenPopulation) {
            maxResourceDeath = Math.Max(currentCitizenPopulation - currentWater, maxResourceDeath);
        }
        return maxResourceDeath * rateCitizenDeathPerNoResource;
    }

    string generateLackResourceMessage() {
        string message = "";
        if (currentFood < currentCitizenPopulation) {
            message += "Food";
        }
        if (currentWater < currentCitizenPopulation) {
            message += " Water";
        }
        return message;
    }

    // Update is called once per frame
    void Update()
    
    {
        ImmediateStyle.Text("/Canvas/CoinText4142", "Coin: " + currentCoin + "(" + coinGrowthPerTurn() + "/turn) + " + assignedCoin + " citizens");
        ImmediateStyle.Text("/Canvas/FoodTexte6fc", "Food:" + currentFood + "(" + foodGrowthPerTurn() + "/turn) + " + assignedFood + " citizens");
        ImmediateStyle.Text("/Canvas/UraniumTextc4b3", "Uranium:" + currentUranium + "(" + uraniumGrowthPerTurn() + "/turn) + " + assignedUranium + " citizens");
        ImmediateStyle.Text("/Canvas/WaterText798e", "Water:" + currentWater + "(" + waterGrowthPerTurn() + "/turn)" + assignedWater + " citizens");
        ImmediateStyle.Text("/Canvas/PopulationText084a", "Population:" + currentCitizenPopulation + "(" + CitizenGrowthPerTurn() + "/turn)");
        ImmediateStyle.Text("/Canvas/Uranium Required508f", "Uranium Required for Barrier:" + requiredUraniumForBarrier);

        if (popDeathByBarrier() > 0) {
            ImmediateStyle.Text("/Canvas/BubbleDeathTextfddb", $"Barrier underpowered ({popDeathByBarrier()} deaths to radiation)");
        } else {
            ImmediateStyle.Text("/Canvas/BubbleDeathTextfddb", "Barrier: Ready No Deaths");
        }

        if (popDeathByLackResource() > 0) {
            ImmediateStyle.Text("/Canvas/ResourceDeathTexta4fa", $"{popDeathByLackResource()} deaths by lack of {generateLackResourceMessage()}");
        } else {
            ImmediateStyle.Text("/Canvas/ResourceDeathTexta4fa", "No Deaths from lack of resources");
        }

        if (popGrowthByFood() > 0) {
            ImmediateStyle.Text("/Canvas/PopulationGrowthText21e3", $"{popGrowthByFood()} citizens born with our surplus food");
        } else {
            ImmediateStyle.Text("/Canvas/PopulationGrowthText21e3", "No Surplus Food to Grow Population");
        }

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
