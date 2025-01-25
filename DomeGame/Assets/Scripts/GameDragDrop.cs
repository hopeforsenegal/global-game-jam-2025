using MoonlitSystem.UI.Immediate;
using UnityEngine;

public class GameDragDrop : MonoBehaviour
{
    protected void Update()
    {
        var hasDropped = ImmediateStyle.DragDrop("/Canvas/Bottom Bar/Population7855", out var component).IsMouseUp;

        if (component.IsDragging) {
            ImmediateStyle.FollowCursor(component.transform);
        }
        if (hasDropped) {
            var unassignedSlot = Reference.Find<RectTransform>(this, "/Canvas/Bottom Bar5962");

            var coinResourceSlot = Reference.Find<RectTransform>(this, "/Canvas/City/CoinResource3e73");
            var foodResourceSlot = Reference.Find<RectTransform>(this, "/Canvas/City/FoodResourcea194");
            var uraniumResourceSlot = Reference.Find<RectTransform>(this, "/Canvas/City/UraniumResource8ffd");
            var waterResourceSlot = Reference.Find<RectTransform>(this, "/Canvas/City/WaterResourceeb63");

            if (RectTransformUtility.RectangleContainsScreenPoint(unassignedSlot, component.transform.position)) {
                component.PinnedPosition = unassignedSlot.position;
                // TODO: Logic to assign to unassign that population unit
                Debug.Log("Unassigning 100 people");
            }

            // Coin resource logic
            if (RectTransformUtility.RectangleContainsScreenPoint(coinResourceSlot, component.transform.position)) {
                component.PinnedPosition = coinResourceSlot.position;
                // TODO: Logic to assign to coin (update tool tip info, etc)
                Debug.Log("Assigning 100 people to mine bubble coin");
            }

            // Food resource logic
            if (RectTransformUtility.RectangleContainsScreenPoint(foodResourceSlot, component.transform.position)) {
                component.PinnedPosition = foodResourceSlot.position;
                // TODO: Logic to assign to food resource (update tool tip info, etc)
                Debug.Log("Assigning 100 people to farm food");
            }

            // Uranium resource logic
            if (RectTransformUtility.RectangleContainsScreenPoint(uraniumResourceSlot, component.transform.position)) {
                component.PinnedPosition = uraniumResourceSlot.position;
                // TODO: Logic to assign to uranium resource (update tool tip info, etc)
                Debug.Log("Assigning 100 people to mine uranium");
            }

            // Water resource logic
            if (RectTransformUtility.RectangleContainsScreenPoint(waterResourceSlot, component.transform.position)) {
                component.PinnedPosition = waterResourceSlot.position;
                // TODO: Logic to assign to water resource (update tool tip info, etc)
                Debug.Log("Assigning 100 people to gather water");
            }

            component.transform.position = component.PinnedPosition;
        }
    }
}
