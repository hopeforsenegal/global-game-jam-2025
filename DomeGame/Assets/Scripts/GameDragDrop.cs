using MoonlitSystem.UI.Immediate;
using UnityEngine;
using TMPro;

public class GameDragDrop : MonoBehaviour
{
    int totalPopulation;
    int unusedPopulation;

    int numCoinAssignments = 0;
    int numFoodAssignments = 0;
    int numUraniumAssignments = 0;
    int numWaterAssignments = 0;
    
    public TMP_Text coinTooltipText;
    string defaultCoinTooltipString = "BubbleCoin can be used during random events.\n\nPlace people here to mine BubbleCoin.";

    public TMP_Text foodTooltipText;

    void Start()
    {
        // TODO: Update to grab from Player script
        totalPopulation = 500;
        unusedPopulation = totalPopulation;
    }

    protected void Update()
    {
        var hasDropped = ImmediateStyle.DragDrop("/Canvas/Bottom Bar/Population7855", out var component).IsMouseUp;

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
                // TODO: Logic to assign to unassign that population unit
                Debug.Log("Unassigning 100 people");

                // Fix this to be correct logic depending on what allocation moved
                coinTooltipText.text = defaultCoinTooltipString;
                numCoinAssignments = 0;
                unusedPopulation = totalPopulation;
            }

            // Coin resource logic
            if (RectTransformUtility.RectangleContainsScreenPoint(coinResourceSlot, component.transform.position)) {
                component.PinnedPosition = coinResourceSlot.position;
                UpdateCoinAllocation();
            }

            // Food resource logic
            if (RectTransformUtility.RectangleContainsScreenPoint(foodResourceSlot, component.transform.position)) {
                component.PinnedPosition = foodResourceSlot.position;
                // TODO: Logic to assign to food resource (update tool tip info, etc)
                Debug.Log("Assigning 100 people to farm food");
                numFoodAssignments += 100;
            }

            // Uranium resource logic
            if (RectTransformUtility.RectangleContainsScreenPoint(uraniumResourceSlot, component.transform.position)) {
                component.PinnedPosition = uraniumResourceSlot.position;
                // TODO: Logic to assign to uranium resource (update tool tip info, etc)
                Debug.Log("Assigning 100 people to mine uranium");
                numUraniumAssignments += 100;
            }

            // Water resource logic
            if (RectTransformUtility.RectangleContainsScreenPoint(waterResourceSlot, component.transform.position)) {
                component.PinnedPosition = waterResourceSlot.position;
                // TODO: Logic to assign to water resource (update tool tip info, etc)
                Debug.Log("Assigning 100 people to gather water");
                numWaterAssignments += 100;
            }

            component.transform.position = component.PinnedPosition;
        }
    }

    void UpdateCoinAllocation()
    {
        Debug.Log("Assigning 100 people to mine bubble coin");
        numCoinAssignments += 100;
        unusedPopulation -= 100;
        Debug.Log("Still have " + unusedPopulation + " people left to allocate");

        coinTooltipText.text = numCoinAssignments + " people allocated to mining BubbleCoin";
    }
}
