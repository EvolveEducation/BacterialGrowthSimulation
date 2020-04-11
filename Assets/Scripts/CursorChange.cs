using UnityEngine;
using UnityEngine.EventSystems;

public class CursorChange : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D openHandCursor;
    public Texture2D closedHandCursor;
    public Texture2D clickCursor;
    private CursorMode mode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("OnMouseEnter");
    }

    public void Enter()
    {
        Cursor.SetCursor(openHandCursor, hotSpot, mode);
    }

    public void Exit()
    {
        Cursor.SetCursor(defaultCursor, hotSpot, mode);
    }

    public void Drag()
    {
        Cursor.SetCursor(closedHandCursor, hotSpot, mode);
    }

    public void Over()
    {
        Cursor.SetCursor(clickCursor, hotSpot, mode);
    }
}
