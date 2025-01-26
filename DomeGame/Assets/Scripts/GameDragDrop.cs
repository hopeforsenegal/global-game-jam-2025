using MoonlitSystem.UI.Immediate;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

public class GameDragDrop : MonoBehaviour
{
    Player playerScript;

    int totalPopulation;
    int unusedPopulation;

    // One citizen dragged equals this much of the population.
    int citizenUnit = 100;

    int numCoinAssignments = 0;
    int numFoodAssignments = 0;
    int numUraniumAssignments = 0;
    int numWaterAssignments = 0;

    bool isMovingFromUnallocated;
    bool isMovingFromCoin;
    bool isMovingFromFood;
    bool isMovingFromUranium;
    bool isMovingFromWater;

    [SerializeField] GameObject[] citizenObjects;

    public TMP_Text numUnassignedCitizensText;
    
    public TMP_Text coinTooltipText;
    public TMP_Text foodTooltipText;
    public TMP_Text uraniumTooltipText;
    public TMP_Text waterTooltipText;

    string defaultCoinTooltipString = "BubbleCoin can be used during random events.\n\nPlace a citizen here to mine BubbleCoin.";
    string defaultFoodTooltipString = "Food is necessary for your population to survive. Every 10 surplus of food you have creates an extra person.\n\nPlace a citizen here to collect food.";
    string defaultUraniumTooltipString = "Uranium is used to power your bubble. You need 1 uranium per person in your population.\n\nPlace a citizen here to collect Uranium.";
    string defaultWaterTooltipString = "Water is necessary for your population to survive.\n\nPlace a citizen here to collect water.";

    void Start()
    {
        playerScript = FindObjectOfType<Player>();

        totalPopulation = playerScript.GetCurrentCitizenPopulation();
        // totalPopulation = 100;
        unusedPopulation = totalPopulation;
        SetUpUsableCitizenObjects();

        coinTooltipText.text = defaultCoinTooltipString;
        foodTooltipText.text = defaultFoodTooltipString;
        uraniumTooltipText.text = defaultUraniumTooltipString;
        waterTooltipText.text = defaultWaterTooltipString;

        numUnassignedCitizensText.text = (unusedPopulation / citizenUnit).ToString();
    }

    void SetUpUsableCitizenObjects()
    {
        int numCitizenObjectsNeeded = totalPopulation / citizenUnit;
        for (int i = 0; i < numCitizenObjectsNeeded; i++)
        {
            citizenObjects[i].SetActive(true);
        }
    }

    void Update()
    {
        string[] allCitizenGUIDs = new[] {
            "/Canvas/Bottom Bar/Unassigned/Citizen1a3e",
            "/Canvas/Bottom Bar/Unassigned/Citizen29af0",
            "/Canvas/Bottom Bar/Unassigned/Citizen352d5",
            "/Canvas/Bottom Bar/Unassigned/Citizen4b866",
            "/Canvas/Bottom Bar/Unassigned/Citizen58c27",
            "/Canvas/Bottom Bar/Unassigned/Citizen6ba83",
            "/Canvas/Bottom Bar/Unassigned/Citizen7405f",
            "/Canvas/Bottom Bar/Unassigned/Citizen8efa3",
            "/Canvas/Bottom Bar/Unassigned/Citizen9ce93"
        };

        foreach (var s in allCitizenGUIDs) {
            var dragDropObject = ImmediateStyle.DragDrop(s, out var component);

            var unassignedSlot = Reference.Find<RectTransform>(this, "/Canvas/Bottom Bar/Unassignedcc19");
            var coinResourceSlot = Reference.Find<RectTransform>(this, "/Canvas/City/CoinResource3e73");
            var foodResourceSlot = Reference.Find<RectTransform>(this, "/Canvas/City/FoodResourcea194");
            var uraniumResourceSlot = Reference.Find<RectTransform>(this, "/Canvas/City/UraniumResource8ffd");
            var waterResourceSlot = Reference.Find<RectTransform>(this, "/Canvas/City/WaterResourceeb63");
            
            var startedDrag = component.IsMouseDown;
            var hasDropped = dragDropObject.IsMouseUp;

            if (startedDrag)
            {
                Debug.Log("Started drag");
                isMovingFromUnallocated = false;
                isMovingFromCoin = false;
                isMovingFromFood = false;
                isMovingFromUranium = false;
                isMovingFromWater = false;

                if (RectTransformUtility.RectangleContainsScreenPoint(unassignedSlot, component.transform.position)) {
                    Debug.Log("From unallocated citizens area");
                    isMovingFromUnallocated = true;
                }
                else if (RectTransformUtility.RectangleContainsScreenPoint(coinResourceSlot, component.transform.position)) {
                    Debug.Log("From coin area");
                    isMovingFromCoin = true;
                }
                else if (RectTransformUtility.RectangleContainsScreenPoint(foodResourceSlot, component.transform.position)) {
                    Debug.Log("From food area");
                    isMovingFromFood = true;
                }
                else if (RectTransformUtility.RectangleContainsScreenPoint(uraniumResourceSlot, component.transform.position)) {
                    Debug.Log("From uranium area");
                    isMovingFromUranium = true;
                }
                else if (RectTransformUtility.RectangleContainsScreenPoint(waterResourceSlot, component.transform.position)) {
                    Debug.Log("From water area");
                    isMovingFromWater = true;
                }
            }

            // if (component.IsDragging)
            // {
            //     ImmediateStyle.FollowCursor(component.transform);
            // }

            if (hasDropped) {
                if (RectTransformUtility.RectangleContainsScreenPoint(unassignedSlot, component.transform.position)) {
                    component.PinnedPosition = unassignedSlot.position;
                    UpdateUnallocatedCitizenAllocation();
                    UpdatePreviousResourceAllocation();
                }

                // Coin resource logic
                if (RectTransformUtility.RectangleContainsScreenPoint(coinResourceSlot, component.transform.position)) {
                    component.PinnedPosition = coinResourceSlot.position;
                    UpdateCoinAllocation();
                    UpdatePreviousResourceAllocation();
                }

                // Food resource logic
                if (RectTransformUtility.RectangleContainsScreenPoint(foodResourceSlot, component.transform.position)) {
                    component.PinnedPosition = foodResourceSlot.position;
                    UpdateFoodAllocation();
                    UpdatePreviousResourceAllocation();
                }

                // Uranium resource logic
                if (RectTransformUtility.RectangleContainsScreenPoint(uraniumResourceSlot, component.transform.position)) {
                    component.PinnedPosition = uraniumResourceSlot.position;
                    UpdateUraniumAllocation();
                    UpdatePreviousResourceAllocation();
                }

                // Water resource logic
                if (RectTransformUtility.RectangleContainsScreenPoint(waterResourceSlot, component.transform.position)) {
                    component.PinnedPosition = waterResourceSlot.position;
                    UpdateWaterAllocation();
                    UpdatePreviousResourceAllocation();
                }

                component.transform.position = component.PinnedPosition;

                // Reset all start drag bools
                Debug.Log("Reset all start drag bools");
                isMovingFromUnallocated = false;
                isMovingFromCoin = false;
                isMovingFromFood = false;
                isMovingFromUranium = false;
                isMovingFromWater = false;

                UpdateTooltips();
            }
        }
    }

    void UpdateUnallocatedCitizenAllocation()
    {
        Debug.Log("Unassigning 1 citizen");
        unusedPopulation += citizenUnit;
    }

    void UpdateCoinAllocation()
    {
        Debug.Log("Assigning 1 citizen to mine bubble coin");
        numCoinAssignments += citizenUnit;
    }

    void UpdateFoodAllocation()
    {
        Debug.Log("Assigning 1 citizen to farm food");
        numFoodAssignments += citizenUnit;
    }

    void UpdateUraniumAllocation()
    {
        Debug.Log("Assigning 1 citizen to collect uranium");
        numUraniumAssignments += citizenUnit;
    }

    void UpdateWaterAllocation()
    {
        Debug.Log("Assigning 1 citizen to collect water");
        numWaterAssignments += citizenUnit;
    }

    void UpdatePreviousResourceAllocation()
    {
        if (isMovingFromUnallocated)
        {
            unusedPopulation -= citizenUnit;
            Debug.Log("Still have " + unusedPopulation + " people left to allocate");
        }
        else if (isMovingFromCoin)
        {
            numCoinAssignments -= citizenUnit;
        }
        else if (isMovingFromFood)
        {
            numFoodAssignments -= citizenUnit;
        }
        else if (isMovingFromUranium)
        {
            numUraniumAssignments -= citizenUnit;
        }
        else if (isMovingFromWater)
        {
            numWaterAssignments -= citizenUnit;
        }
    }

    void UpdateTooltips()
    {
        numUnassignedCitizensText.text = (unusedPopulation / citizenUnit).ToString();

        if (numCoinAssignments == 0)
        {
            coinTooltipText.text = defaultCoinTooltipString;
        }
        else
        {
            int coinRate = playerScript.CalculateRateCoinGivenCitizen(numCoinAssignments);
            coinTooltipText.text = numCoinAssignments + " people allocated to mining BubbleCoin\n\nCoin rate = " + coinRate;
        }

        if (numFoodAssignments == 0)
        {
            foodTooltipText.text = defaultFoodTooltipString;
        }
        else
        {
            int foodRate = playerScript.CalculateRateFoodGivenCitizen(numFoodAssignments);
            foodTooltipText.text = numFoodAssignments + " people allocated to farming\n\nFood rate = " + foodRate;
        }

        if (numUraniumAssignments == 0)
        {
            uraniumTooltipText.text = defaultUraniumTooltipString;
        }
        else
        {
            int uraniumRate = playerScript.CalculateRateUraniumGivenCitizen(numUraniumAssignments);
            uraniumTooltipText.text = numUraniumAssignments + " people allocated to collecting uranium\n\nUranium rate = " + uraniumRate;
        }

        if (numWaterAssignments == 0)
        {
            waterTooltipText.text = defaultWaterTooltipString;
        }
        else
        {
            int waterRate = playerScript.CalculateRateWaterGivenCitizen(numWaterAssignments);
            waterTooltipText.text = numWaterAssignments + " people allocated to collecting water\n\nWater rate = " + waterRate;
        }
    }
}
