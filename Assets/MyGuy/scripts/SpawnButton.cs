using UnityEngine;
using UnityEngine.EventSystems; // Required for IPointerClickHandler

public class SpawnButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // Define what happens when the button is clicked
        Debug.Log("Button Clicked!");
    }
}