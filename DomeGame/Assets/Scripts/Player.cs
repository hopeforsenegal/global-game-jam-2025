using System;
using MoonlitSystem.UI.Immediate;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GamePhase 
{
    Event,
    StartPhase,
    ResourceGathering,
    EndTurn,
}

public class Player : MonoBehaviour
{

    [SerializeField] int currentTurn = 0;
    [SerializeField] int maxTurns = 5;

    [SerializeField] GamePhase currentPhase = GamePhase.ResourceGathering;


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
        return foodGeneratedPerAssignedCitizen * citizen;
    }

    public int CalculateRateUraniumGivenCitizen(int citizen) {
        return coinGeneratedPerAssignedCitizen * citizen;
    }

    public int CalculateRateWaterGivenCitizen(int citizen) {
        return waterGeneratedPerAssignedCitizen * citizen;
    }

    public int CalculateRateCoinGivenCitizen(int citizen) {
        return uraniumGeneratedPerAssignedCitizen * citizen;
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

    int foodUsedPopulationPerTurn(){
        return -rateFoodPerCitizen*currentCitizenPopulation;
    }

    int uraniumUsedPopulationPerTurn(){
        return -rateUraniumPerCitizen*currentCitizenPopulation;
    }

    int waterUsedPopulationPerTurn(){
        return rateWaterPerCitizen*currentCitizenPopulation;
    }

    int waterUsedForUraniumPerTurn(){
        return rateWaterPerUranium*currentUranium;
    }

    int waterGrowthPerTurn(){
        return -waterUsedPopulationPerTurn() - waterUsedForUraniumPerTurn();
    }
    int coinGrowthPerTurn(){
        return rateCoinPerTurn;
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
        if (currentFood < currentCitizenPopulation && currentWater < currentCitizenPopulation) {
            return "food and water";
        }
        if (currentFood < currentCitizenPopulation) {
            return "food";
        }
        if (currentWater < currentCitizenPopulation) {
            return "water";
        }
        return "";
    }

    // Update is called once per frame
    void Update()
    
    {
        ImmediateStyle.Text("/Canvas/PopulationText084a", $"Population: {currentCitizenPopulation}");
        ImmediateStyle.Text("/Canvas/Uranium Required508f", $"Uranium Required for Barrier: {requiredUraniumForBarrier}");
        ImmediateStyle.Text("/Canvas/PhaseTextc7bd", currentPhase.ToString());
        ImmediateStyle.Text("/Canvas/TurnText366e", $"Turn: {currentTurn}/{maxTurns}");

        ImmediateStyle.Text("/Canvas/CoinTotalText924e", $"{currentCoin}");
        ImmediateStyle.Text("/Canvas/FoodTotalTexte155", $"{currentFood}");
        ImmediateStyle.Text("/Canvas/UraniumTotalText68fd", $"{currentUranium}");
        ImmediateStyle.Text("/Canvas/WaterTotalTexta1c3", $"{currentWater}");



        if (currentCitizenPopulation <= 0) {
            ImmediateStyle.Text("/Canvas/EndText82ef", "GameOver");
            if (Input.GetKeyDown(KeyCode.Space)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            return;
        }

    
        if (currentTurn == maxTurns) {
            if (currentCitizenPopulation > 0) {
                ImmediateStyle.Text("/Canvas/EndText82ef", $"You Win\n{GetCurrentCitizenPopulation()} Citizens Survived");

            } else {
                ImmediateStyle.Text("/Canvas/EndText82ef", "GameOver");

            }
            if (Input.GetKeyDown(KeyCode.Space)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            return;
        }
        if (currentPhase == GamePhase.StartPhase) {
            ImmediateStyle.CanvasGroup("/Canvas/ResourceUpdate9d7b");
            ImmediateStyle.Text("/Canvas/ResourceUpdate/StartTurnText9bc7", "Resources consumed");
            ImmediateStyle.Text("/Canvas/CoinText4142", $"Coins +{coinGrowthPerTurn()}");
            ImmediateStyle.Text("/Canvas/FoodTexte6fc", $"Food {foodUsedPopulationPerTurn()}");
            ImmediateStyle.Text("/Canvas/UraniumTextc4b3", $"Uranium {uraniumUsedPopulationPerTurn()}");
            ImmediateStyle.Text("/Canvas/WaterText798e", $"Water {waterUsedPopulationPerTurn()}");
            if (Input.GetKeyDown(KeyCode.Space)) {
                int foodGrowth = foodUsedPopulationPerTurn();
                int uraniumGrowth = uraniumUsedPopulationPerTurn();
                int waterGrowth = waterGrowthPerTurn();
                int coinGrowth = coinGrowthPerTurn();
                currentFood = math.max(foodGrowth + currentFood, 0);
                currentUranium = math.max(uraniumGrowth + currentUranium, 0);
                currentWater = math.max(waterGrowth + currentWater, 0);
                currentCoin = math.max(coinGrowth + currentCoin, 0);
                currentPhase = GamePhase.ResourceGathering;
            }
            return;  
        }
        if (currentPhase == GamePhase.ResourceGathering) {
            ImmediateStyle.CanvasGroup("/Canvas/ResourceUpdate9d7b");
            ImmediateStyle.Text("/Canvas/CoinText4142", $"Coin: {currentCoin} +{CalculateRateCoinGivenCitizen(assignedCoin)}");
            ImmediateStyle.Text("/Canvas/FoodTexte6fc", $"Food: {currentFood} +{CalculateRateFoodGivenCitizen(assignedFood)}");
            ImmediateStyle.Text("/Canvas/UraniumTextc4b3", $"Uranium: {currentUranium} +{CalculateRateUraniumGivenCitizen(assignedUranium)}");
            ImmediateStyle.Text("/Canvas/WaterText798e", $"Water: {currentWater} +{CalculateRateWaterGivenCitizen(assignedWater)}");
            if (Input.GetKeyDown(KeyCode.Space)) {
                currentPhase = GamePhase.EndTurn;
                int foodGrowth = CalculateRateFoodGivenCitizen(assignedFood);
                int uraniumGrowth = CalculateRateUraniumGivenCitizen(assignedUranium);
                int waterGrowth = CalculateRateWaterGivenCitizen(assignedWater);
                int coinGrowth = CalculateRateCoinGivenCitizen(assignedCoin);
                currentFood = math.max(foodGrowth + currentFood, 0);
                currentUranium = math.max(uraniumGrowth + currentUranium, 0);
                currentWater = math.max(waterGrowth + currentWater, 0);
                currentCoin = math.max(coinGrowth + currentCoin, 0); 
                return;
            }
        } else if (currentPhase == GamePhase.EndTurn) {
            ImmediateStyle.CanvasGroup("/Canvas/EndTurnText8f05");
            if (popDeathByBarrier() > 0) {
                ImmediateStyle.Text("/Canvas/BubbleDeathTextfddb", $"Due to lack of uranium, your barrier was underpowered, and {popDeathByBarrier()} people have died");
            } else {
                ImmediateStyle.Text("/Canvas/BubbleDeathTextfddb", "Your dome barrier held strong.");
            }

            if (popGrowthByFood() > 0) {
                ImmediateStyle.Text("/Canvas/PopulationGrowthText21e3", $"Thanks to your surplus food, your population has grown by {popGrowthByFood()!}.");
            }
            if (popDeathByLackResource() > 0) {
                ImmediateStyle.Text("/Canvas/ResourceDeathTexta4fa", $"{popDeathByLackResource()} people have died from lack of {generateLackResourceMessage()}.");
            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                int citizenGrowth = CitizenGrowthPerTurn();
                currentCitizenPopulation = math.max(citizenGrowth + currentCitizenPopulation, 0);
                currentTurn = currentTurn + 1;
                currentPhase = GamePhase.StartPhase;
            }
            return;
        }
    }
}
