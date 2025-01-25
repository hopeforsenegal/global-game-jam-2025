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

            var foodResourceSlot = Reference.Find<RectTransform>(this, "/Canvas/City/FoodResource893b");
            var uraniumResourceSlot = Reference.Find<RectTransform>(this, "/Canvas/City/UraniumResource8ffd");

            if (RectTransformUtility.RectangleContainsScreenPoint(unassignedSlot, component.transform.position)) {
                component.PinnedPosition = unassignedSlot.position;
                // TODO: Logic to assign to unassign that population unit
            }

            if (RectTransformUtility.RectangleContainsScreenPoint(foodResourceSlot, component.transform.position)) {
                component.PinnedPosition = foodResourceSlot.position;
                // TODO: Logic to assign to food resource (update tool tip info, etc)
            }

            if (RectTransformUtility.RectangleContainsScreenPoint(uraniumResourceSlot, component.transform.position)) {
                component.PinnedPosition = uraniumResourceSlot.position;
                // TODO: Logic to assign to uranium resource (update tool tip info, etc)
            }

            component.transform.position = component.PinnedPosition;
        }
    }
}
