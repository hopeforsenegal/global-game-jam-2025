using MoonlitSystem.UI.Immediate;
using UnityEngine;
using TMPro;

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

        coinTooltipText.text = defaultCoinTooltipString;
        foodTooltipText.text = defaultFoodTooltipString;
        uraniumTooltipText.text = defaultUraniumTooltipString;
        waterTooltipText.text = defaultWaterTooltipString;

        numUnassignedCitizensText.text = (unusedPopulation / citizenUnit).ToString();
    }

    protected void Update()
    {
        var hasDropped = ImmediateStyle.DragDrop("/Canvas/Bottom Bar/Unassigned/Citizen1a3e", out var component).IsMouseUp;

        if (component.IsDragging) {
            ImmediateStyle.FollowCursor(component.transform);
        }
        if (hasDropped) {
            var unassignedSlot = Reference.Find<RectTransform>(this, "/Canvas/Bottom Bar/Unassignedcc19");

            var coinResourceSlot = Reference.Find<RectTransform>(this, "/Canvas/City/CoinResource3e73");
            var foodResourceSlot = Reference.Find<RectTransform>(this, "/Canvas/City/FoodResourcea194");
            var uraniumResourceSlot = Reference.Find<RectTransform>(this, "/Canvas/City/UraniumResource8ffd");
            var waterResourceSlot = Reference.Find<RectTransform>(this, "/Canvas/City/WaterResourceeb63");

            if (RectTransformUtility.RectangleContainsScreenPoint(unassignedSlot, component.transform.position)) {
                component.PinnedPosition = unassignedSlot.position;
                Debug.Log("Unassigning 1 citizen");
                UpdateUnassignedCitizens(-citizenUnit);

                // TODO: Fix this to be correct logic depending on what allocation moved
                // (We need the start point to know where the initial allocation was)
                coinTooltipText.text = defaultCoinTooltipString;
                numCoinAssignments -= citizenUnit;

                foodTooltipText.text = defaultFoodTooltipString;
                numFoodAssignments = 0;
                // numFoodAssignments -= citizenUnit;

                uraniumTooltipText.text = defaultUraniumTooltipString;
                numUraniumAssignments = 0;
                // numUraniumAssignments -= citizenUnit;

                waterTooltipText.text = defaultWaterTooltipString;
                numWaterAssignments = 0;
                // numWaterAssignments -= citizenUnit;
            }

            // Coin resource logic
            if (RectTransformUtility.RectangleContainsScreenPoint(coinResourceSlot, component.transform.position)) {
                component.PinnedPosition = coinResourceSlot.position;
                UpdateCoinAllocation();
            }

            // Food resource logic
            if (RectTransformUtility.RectangleContainsScreenPoint(foodResourceSlot, component.transform.position)) {
                component.PinnedPosition = foodResourceSlot.position;
                UpdateFoodAllocation();
            }

            // Uranium resource logic
            if (RectTransformUtility.RectangleContainsScreenPoint(uraniumResourceSlot, component.transform.position)) {
                component.PinnedPosition = uraniumResourceSlot.position;
                UpdateUraniumAllocation();
            }

            // Water resource logic
            if (RectTransformUtility.RectangleContainsScreenPoint(waterResourceSlot, component.transform.position)) {
                component.PinnedPosition = waterResourceSlot.position;
                UpdateWaterAllocation();
            }

            component.transform.position = component.PinnedPosition;
        }
    }

    void UpdateCoinAllocation()
    {
        Debug.Log("Assigning 1 citizen to mine bubble coin");
        numCoinAssignments += citizenUnit;
        UpdateUnassignedCitizens(citizenUnit);

        int coinRate = playerScript.CalculateRateCoinGivenCitizen(numCoinAssignments);

        coinTooltipText.text = numCoinAssignments + " people allocated to mining BubbleCoin\n\nCoin rate = " + coinRate;
    }

    void UpdateFoodAllocation()
    {
        Debug.Log("Assigning 1 citizen to farm food");
        numFoodAssignments += citizenUnit;
        UpdateUnassignedCitizens(citizenUnit);

        int foodRate = playerScript.CalculateRateFoodGivenCitizen(numFoodAssignments);

        foodTooltipText.text = numFoodAssignments + " people allocated to farming\n\nFood rate = " + foodRate;
    }

    void UpdateUraniumAllocation()
    {
        Debug.Log("Assigning 1 citizen to collect uranium");
        numUraniumAssignments += citizenUnit;
        UpdateUnassignedCitizens(citizenUnit);

        int uraniumRate = playerScript.CalculateRateUraniumGivenCitizen(numUraniumAssignments);

        uraniumTooltipText.text = numUraniumAssignments + " people allocated to collecting uranium\n\nUranium rate = " + uraniumRate;
    }

    void UpdateWaterAllocation()
    {
        Debug.Log("Assigning 1 citizen to collect water");
        numWaterAssignments += citizenUnit;
        UpdateUnassignedCitizens(citizenUnit);

        int waterRate = playerScript.CalculateRateWaterGivenCitizen(numWaterAssignments);

        waterTooltipText.text = numWaterAssignments + " people allocated to collecting water\n\nWater rate = " + waterRate;
    }

    void UpdateUnassignedCitizens(int numAssigned)
    {
        unusedPopulation -= numAssigned;
        Debug.Log("Still have " + unusedPopulation + " people left to allocate");

        numUnassignedCitizensText.text = (unusedPopulation / citizenUnit).ToString();
    }
}
