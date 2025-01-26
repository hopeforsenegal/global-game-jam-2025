using System;
using System.Collections.Generic;
using MoonlitSystem.UI.Immediate;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public enum GamePhase
{
    Event,
    EventResult,
    StartPhase,
    ResourceGathering,
    EndTurn,
}

public partial class GameManager
{
    [Header("Core")]
    [SerializeField] private DefaultPlayerSettings gameSettings;

    [Header("Turn Management")]
    [SerializeField] int currentTurn = 0;
    [SerializeField] int maxTurns = 5;
    [SerializeField] GamePhase currentPhase = GamePhase.ResourceGathering;

    [Header("Current Resources")]
    [SerializeField] int currentCitizenPopulation;
    [SerializeField] int currentFood;
    [SerializeField] int currentUranium;
    [SerializeField] int currentWater;
    [SerializeField] int currentCoin;

    [Header("Resource Consumption Rates")]
    [SerializeField] int rateFoodPerCitizen;
    [SerializeField] int rateUraniumPerCitizen;
    [SerializeField] int rateWaterPerCitizen;
    [SerializeField] int rateWaterPerUranium;
    [SerializeField] int rateCoinPerTurn;

    [Header("Resource Generation Rates")]
    [SerializeField] int foodGeneratedPerAssignedCitizen;
    [SerializeField] int coinGeneratedPerAssignedCitizen;
    [SerializeField] int waterGeneratedPerAssignedCitizen;
    [SerializeField] int uraniumGeneratedPerAssignedCitizen;

    [Header("Population Management")]
    [SerializeField] int surplusFoodToGrowOneCitizenPerTurn;
    [SerializeField] int rateCitizenDeathPerNoResource;
    [SerializeField] int rateCitizenDeathByBarrier;
    [SerializeField] int requiredUraniumForBarrier;
    [SerializeField] int increaseRequiredUraniumForBarrierPerTurn;
    bool endTurnClicked = false;

    public int CalculateRateFoodGivenCitizen(int citizen)
    {
        return foodGeneratedPerAssignedCitizen * citizen;
    }

    public int CalculateRateUraniumGivenCitizen(int citizen)
    {
        return coinGeneratedPerAssignedCitizen * citizen;
    }

    public int CalculateRateWaterGivenCitizen(int citizen)
    {
        return waterGeneratedPerAssignedCitizen * citizen;
    }

    public int CalculateRateCoinGivenCitizen(int citizen)
    {
        return uraniumGeneratedPerAssignedCitizen * citizen;
    }

    public int CitizenGrowthPerTurn()
    {
        return popGrowthByFood() - popDeathByLackResource() - popDeathByBarrier();
    }

    public int GetCurrentCitizenPopulation()
    {
        return currentCitizenPopulation;
    }

    public int GetCurrentFood()
    {
        return currentFood;
    }

    public int GetCurrentWater()
    {
        return currentWater;
    }

    public int GetCurrentUranium()
    {
        return currentUranium;
    }

    public int GetCurrentCoin()
    {
        return currentCoin;
    }

    public int GetCurrentTurn()
    {
        return currentTurn;
    }

    int foodUsedPopulationPerTurn()
    {
        return -rateFoodPerCitizen * currentCitizenPopulation;
    }

    int uraniumUsedPopulationPerTurn()
    {
        return -rateUraniumPerCitizen * currentCitizenPopulation;
    }

    int waterUsedPopulationPerTurn()
    {
        return -rateWaterPerCitizen * currentCitizenPopulation;
    }

    int waterUsedForUraniumPerTurn()
    {
        return rateWaterPerUranium * math.min(currentUranium, requiredUraniumForBarrier);
    }
    int coinGrowthPerTurn()
    {
        return rateCoinPerTurn;
    }

    int popDeathByBarrier()
    {
        if (currentUranium > requiredUraniumForBarrier) {
            return 0;
        }
        return (requiredUraniumForBarrier - currentUranium) * rateCitizenDeathByBarrier;
    }

    int popGrowthByFood()
    {
        if (currentFood < currentCitizenPopulation) {
            return 0;
        }
        return Math.Max(currentFood - currentCitizenPopulation, 0) / surplusFoodToGrowOneCitizenPerTurn;
    }

    int popDeathByLackResource()
    {
        int maxResourceDeath = 0;
        if (currentFood < currentCitizenPopulation) {
            maxResourceDeath = Math.Max(currentCitizenPopulation - currentFood, 0);
        }
        if (currentWater < currentCitizenPopulation) {
            maxResourceDeath = Math.Max(currentCitizenPopulation - currentWater, maxResourceDeath);
        }
        return maxResourceDeath * rateCitizenDeathPerNoResource;
    }

    string generateLackResourceMessage()
    {
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

    void StartRealCore()
    {
        Screen = GameScreens.Core;
        currentPhase = GamePhase.ResourceGathering;
        currentTurn = 0;
        endTurnClicked = false;
        currentCitizenPopulation = gameSettings.CurrentCitizenPopulation;
        currentFood = gameSettings.CurrentFood;
        currentUranium = gameSettings.CurrentUranium;
        currentWater = gameSettings.CurrentWater;
        currentCoin = gameSettings.CurrentCoin;
        rateFoodPerCitizen = gameSettings.RateFoodPerCitizen;
        rateUraniumPerCitizen = gameSettings.RateUraniumPerCitizen;
        rateWaterPerCitizen = gameSettings.RateWaterPerCitizen;
        rateWaterPerUranium = gameSettings.RateWaterPerUranium;
        rateCoinPerTurn = gameSettings.RateCoinPerTurn;
        foodGeneratedPerAssignedCitizen = gameSettings.FoodGeneratedPerAssignedCitizen;
        coinGeneratedPerAssignedCitizen = gameSettings.CoinGeneratedPerAssignedCitizen;
        waterGeneratedPerAssignedCitizen = gameSettings.WaterGeneratedPerAssignedCitizen;
        uraniumGeneratedPerAssignedCitizen = gameSettings.UraniumGeneratedPerAssignedCitizen;
        surplusFoodToGrowOneCitizenPerTurn = gameSettings.SurplusFoodToGrowOneCitizenPerTurn;
        rateCitizenDeathPerNoResource = gameSettings.RateCitizenDeathPerNoResource;
        rateCitizenDeathByBarrier = gameSettings.RateCitizenDeathByBarrier;
        requiredUraniumForBarrier = gameSettings.RequiredUraniumForBarrier;
        maxTurns = gameSettings.MaxTurns;
        increaseRequiredUraniumForBarrierPerTurn = gameSettings.IncreaseRequiredUraniumForBarrierPerTurn;
    }

    bool hasResourcesGained(EventEffects effect) {
        if (effect.coinsGained != 0) {
            return true;
        }
        if (effect.populationGained != 0) {
            return true;
        }
        if (effect.foodGained != 0) {
            return true;
        }
        if (effect.waterGained != 0) {
            return true;
        }
        if (effect.uraniumGained !=0) {
            return true;
        }
        return false;
    }

    void HandleRealCore()
    {
        ImmediateStyle.CanvasGroup("/Canvas/Core4cc0");

        ImmediateStyle.Text("/Canvas/PopulationText084a", $"Population: {currentCitizenPopulation}");
        ImmediateStyle.Text("/Canvas/Uranium Required508f", $"{requiredUraniumForBarrier} Uranium to power the barrier");
        ImmediateStyle.Text("/Canvas/TurnText366e", $"Turn: {currentTurn}/{maxTurns}");

        ImmediateStyle.Text("/Canvas/CoinTotalText924e", $"{currentCoin}");
        ImmediateStyle.Text("/Canvas/FoodTotalTexte155", $"{currentFood}");
        ImmediateStyle.Text("/Canvas/UraniumTotalText68fd", $"{currentUranium}");
        ImmediateStyle.Text("/Canvas/WaterTotalTexta1c3", $"{currentWater}");



        if (currentCitizenPopulation <= 0) {
            ImmediateStyle.Text("/Canvas/EndText82ef", "GameOver");
            if (Input.GetKeyDown(KeyCode.Escape)) {
                StartMainMenu();
            }
            return;
        }


        if (currentTurn == maxTurns) {
            ImmediateStyle.CanvasGroup("/Prefab Mode in Context/Core/GameObject4b5d");

            if (currentCitizenPopulation > 0) {
                ImmediateStyle.Text("/Canvas/EndText82ef", $"You Win\n{GetCurrentCitizenPopulation()} Citizens Survived");

            } else {
                ImmediateStyle.Text("/Canvas/EndText82ef", "GameOver");

            }
            if (Input.GetKeyDown(KeyCode.Escape)) {
                StartMainMenu();
            }
            return;
        }
        if (currentPhase == GamePhase.StartPhase) {
            ImmediateStyle.CanvasGroup("/Canvas/ResourceUpdate9d7b");
            ImmediateStyle.Text("/Prefab Mode in Context/ResourceGathering/EndTurnButton/EndTurnButtonText7b4c", "Next");
            ImmediateStyle.Text("/Canvas/ResourceUpdate/StartTurnText9bc7", "Resources consumed");
            ImmediateStyle.Text("/Canvas/FoodTexte6fc", $"Food {foodUsedPopulationPerTurn()}");
            ImmediateStyle.Text("/Canvas/UraniumTextc4b3", $"Uranium {uraniumUsedPopulationPerTurn()}");
            ImmediateStyle.Text("/Canvas/WaterText798e", $"Water {waterUsedPopulationPerTurn()}");
            if (coinGrowthPerTurn() > 0) {
                ImmediateStyle.Text("/Canvas/CoinText4142", $"Coins +{coinGrowthPerTurn()}");
            }

            ImmediateStyle.CanvasGroup("/Canvas/Core/ResourceGatherb068");
            ImmediateStyle.Text("/Canvas (Environment)/Core/ResourceGather/FoodTextc4c0", $"{foodUsedPopulationPerTurn().ToString("+#;-#")}");
            ImmediateStyle.Text("/Canvas (Environment)/Core/ResourceGather/UraniumText0872", $"{uraniumUsedPopulationPerTurn().ToString("+#;-#")}");
            ImmediateStyle.Text("/Canvas (Environment)/Core/ResourceGather/WaterTextf830", $"{waterUsedPopulationPerTurn().ToString("+#;-#")}");

            if (endTurnClicked) {
                endTurnClicked = false;
                int foodGrowth = foodUsedPopulationPerTurn();
                int uraniumGrowth = uraniumUsedPopulationPerTurn();
                int waterGrowth = waterUsedPopulationPerTurn();
                int coinGrowth = coinGrowthPerTurn();
                currentFood = math.max(foodGrowth + currentFood, 0);
                currentUranium = math.max(uraniumGrowth + currentUranium, 0);
                currentWater = math.max(waterGrowth + currentWater, 0);
                currentCoin = math.max(coinGrowth + currentCoin, 0);
                currentPhase = GamePhase.ResourceGathering;
                // update number of citizens available for assignment;
                StartCore();
            }
            return;  
        }
        if (currentPhase == GamePhase.ResourceGathering) {
            ImmediateStyle.CanvasGroup("/Canvas/Core/ResourceGatherb068");
            ImmediateStyle.Text("/Prefab Mode in Context/ResourceGathering/EndTurnButton/EndTurnButtonText7b4c", "Confirm");

            if (CalculateRateCoinGivenCitizen(numCoinAssignments) > 0) {
                ImmediateStyle.Text("/Canvas (Environment)/Core/ResourceGather/CoinTextb6b4", $"+{CalculateRateCoinGivenCitizen(numCoinAssignments)}");
            }
            if (CalculateRateFoodGivenCitizen(numFoodAssignments) > 0) {
                ImmediateStyle.Text("/Canvas (Environment)/Core/ResourceGather/FoodTextc4c0", $"+{CalculateRateFoodGivenCitizen(numFoodAssignments)}");
            }
            if (CalculateRateUraniumGivenCitizen(numUraniumAssignments) > 0) {
                ImmediateStyle.Text("/Canvas (Environment)/Core/ResourceGather/UraniumText0872", $"+{CalculateRateUraniumGivenCitizen(numUraniumAssignments)}");
            }
            if (CalculateRateWaterGivenCitizen(numWaterAssignments) > 0) {
                ImmediateStyle.Text("/Canvas (Environment)/Core/ResourceGather/WaterTextf830", $"+{CalculateRateWaterGivenCitizen(numWaterAssignments)}");
            }
            if (EventSystem.current.IsPointerOverGameObject()) {
                GameObject hoveredUIElement = GetHoveredUIElement();
                if (hoveredUIElement != null) {
                    if (hoveredUIElement.name.Contains("Coin") && hoveredUIElement.name.Contains("Icon")) {
                        ImmediateStyle.Text("/Canvas (Environment)/Core/Resources/Tooltip0028", "Bubble Coin");
                    }
                    if (hoveredUIElement.name.Contains("Water") && hoveredUIElement.name.Contains("Icon")) {
                        ImmediateStyle.Text("/Canvas (Environment)/Core/Resources/Tooltip0028", "Water");
                    }
                    if (hoveredUIElement.name.Contains("Food") && hoveredUIElement.name.Contains("Icon")) {
                        ImmediateStyle.Text("/Canvas (Environment)/Core/Resources/Tooltip0028", "Food");
                    }
                    if (hoveredUIElement.name.Contains("Uranium") && hoveredUIElement.name.Contains("Icon")) {
                        ImmediateStyle.Text("/Canvas (Environment)/Core/Resources/Tooltip0028", "Uranium");
                    }
                    Debug.Log("Hovering over UI element: " + hoveredUIElement.name);
                }
            }
            if (endTurnClicked) {
                endTurnClicked = false;
                currentPhase = GamePhase.EndTurn;
                int foodGrowth = CalculateRateFoodGivenCitizen(numFoodAssignments);
                int uraniumGrowth = CalculateRateUraniumGivenCitizen(numUraniumAssignments);
                int waterGrowth = CalculateRateWaterGivenCitizen(numWaterAssignments);
                int coinGrowth = CalculateRateCoinGivenCitizen(numCoinAssignments);
                currentFood = math.max(foodGrowth + currentFood, 0);
                currentUranium = math.max(uraniumGrowth + currentUranium, 0);
                currentWater = math.max(waterGrowth + currentWater, 0);
                currentCoin = math.max(coinGrowth + currentCoin, 0);
                return;
            }
        }
        else if (currentPhase == GamePhase.EndTurn) {
            ImmediateStyle.CanvasGroup("/Canvas/EndTurnText8f05");
            ImmediateStyle.Text("/Prefab Mode in Context/ResourceGathering/EndTurnButton/EndTurnButtonText7b4c", "Next");
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
            if (waterUsedForUraniumPerTurn() > 0) {
                ImmediateStyle.CanvasGroup("/Canvas/Core/ResourceGatherb068");
                ImmediateStyle.Text("/Canvas (Environment)/Core/ResourceGather/WaterTextf830", $"-{waterUsedForUraniumPerTurn()}");
                ImmediateStyle.Text("/Canvas (Environment)/Core/EndTurnText/WaterUraniumUsageTextc113", $"{waterUsedForUraniumPerTurn()} water consumed cooling the uranium powered barrier.");
            }

            if (endTurnClicked) {
                endTurnClicked = false;
                int citizenGrowth = CitizenGrowthPerTurn();
                int waterUsed = waterUsedForUraniumPerTurn();
                currentCitizenPopulation = math.max(citizenGrowth + currentCitizenPopulation, 0);
                currentWater = math.max(currentWater -waterUsed, 0);
                currentTurn = currentTurn + 1;
                requiredUraniumForBarrier += increaseRequiredUraniumForBarrierPerTurn;
                if (currentCitizenPopulation <= 0) {
                    ImmediateStyle.Text("/Canvas/EndText82ef", "GameOver");
                    currentPhase = GamePhase.StartPhase;
                    if (Input.GetKeyDown(KeyCode.Escape)) {
                        StartMainMenu();
                    }
                    return;
                }

                StartRandomEvents();
                currentPhase = GamePhase.Event;
            }
            return;
        }
        else if (currentPhase == GamePhase.Event) {
            Screen = GameScreens.RandomEvents;
            if (eventFinished) {
                endTurnClicked = false;
                Screen = GameScreens.Core;
                currentPhase = GamePhase.EventResult;
                return;
            }
        } else if (currentPhase == GamePhase.EventResult) {
            ImmediateStyle.Text("/Prefab Mode in Context/ResourceGathering/EndTurnButton/EndTurnButtonText7b4c", "Next");
            if (selectedChoice == null) {
                currentPhase = GamePhase.StartPhase;
                return;
            }
            if (selectedChoice.effect == null) {
                currentPhase = GamePhase.StartPhase;
                return;
            }
            if (!hasResourcesGained(selectedChoice.effect)) {
                currentPhase = GamePhase.StartPhase;
                return;
            }
            if (selectedChoice != null && selectedChoice.effect != null && hasResourcesGained(selectedChoice.effect)) {
                ImmediateStyle.CanvasGroup("/Canvas/Core/ResourceGatherb068");
                ImmediateStyle.CanvasGroup("/Canvas (Environment)/Core/EventUpdatec14f");
                ImmediateStyle.Text("/Canvas (Environment)/Core/EventUpdate/EventTextf28b", $"Because of your decision");
                if (selectedChoice.effect.foodGained != 0) {
                    ImmediateStyle.Text("/Canvas (Environment)/Core/EventUpdate/EventFoodTextb86f", $"Food {selectedChoice.effect.foodGained.ToString("+#;-#")}");
                    ImmediateStyle.Text("/Canvas (Environment)/Core/ResourceGather/FoodTextc4c0", $"{selectedChoice.effect.foodGained.ToString("+#;-#")}");
                }
                if (selectedChoice.effect.uraniumGained != 0) {
                    ImmediateStyle.Text("/Canvas (Environment)/Core/EventUpdate/EventUraniumTextd574", $"Uranium {selectedChoice.effect.uraniumGained.ToString("+#;-#")}");
                    ImmediateStyle.Text("/Canvas (Environment)/Core/ResourceGather/UraniumText0872", $"{selectedChoice.effect.uraniumGained.ToString("+#;-#")}");
                }
                if (selectedChoice.effect.waterGained != 0) {
                    ImmediateStyle.Text("/Canvas (Environment)/Core/EventUpdate/EventWaterText2832", $"Water {selectedChoice.effect.waterGained.ToString("+#;-#")}");
                    ImmediateStyle.Text("/Canvas (Environment)/Core/ResourceGather/WaterTextf830", $"{selectedChoice.effect.waterGained.ToString("+#;-#")}");
                }
                if (selectedChoice.effect.coinsGained != 0) {
                    ImmediateStyle.Text("/Canvas (Environment)/Core/EventUpdate/EventCoinText5a8e", $"Coin {selectedChoice.effect.coinsGained.ToString("+#;-#")}");
                    ImmediateStyle.Text("/Canvas (Environment)/Core/ResourceGather/CoinTextb6b4", $"{selectedChoice.effect.coinsGained.ToString("+#;-#")}");
                }
                if (selectedChoice.effect.populationGained != 0) {
                    ImmediateStyle.Text("/Canvas (Environment)/Core/EventUpdate/EventPopText8206", $"Population {selectedChoice.effect.populationGained.ToString("+#;-#")}");
                }
                
                if (endTurnClicked) {
                    endTurnClicked = false;
                    currentPhase = GamePhase.StartPhase;
                    currentFood = math.max(selectedChoice.effect.foodGained + currentFood, 0);
                    currentUranium = math.max(selectedChoice.effect.uraniumGained + currentUranium, 0);
                    currentWater = math.max(selectedChoice.effect.waterGained + currentWater, 0);
                    currentCoin = math.max(selectedChoice.effect.coinsGained + currentCoin, 0);
                    currentCitizenPopulation = math.max(selectedChoice.effect.populationGained + currentCitizenPopulation,0);
                    // update number of citizens available for assignment;
                    StartCore();

                    if (currentCitizenPopulation <= 0) {
                        ImmediateStyle.Text("/Canvas/EndText82ef", "GameOver");
                        currentPhase = GamePhase.StartPhase;
                        if (Input.GetKeyDown(KeyCode.Escape)) {
                            StartMainMenu();
                        }
                        return;
                    }
                }
            return;  
                
            }
        }
    }
    public static GameObject GetHoveredUIElement()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0 ? results[0].gameObject : null;
    }
}