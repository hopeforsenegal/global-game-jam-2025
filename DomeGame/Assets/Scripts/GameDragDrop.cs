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
            var slot = Reference.Find<RectTransform>(this, "/Canvas/City/Image701f");

            if (RectTransformUtility.RectangleContainsScreenPoint(slot, component.transform.position)) {
                component.PinnedPosition = slot.position;
            }

            component.transform.position = component.PinnedPosition;
        }
    }
}
