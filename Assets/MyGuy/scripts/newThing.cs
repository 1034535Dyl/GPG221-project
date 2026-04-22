using UnityEngine;
using Anthill.AI;


public partial class UnitSense : MonoBehaviour, ISense
{
    public bool hasCargo;
    public bool nearBase;


    void ISense.CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
    {
        aWorldState.Set(DeliveryBot.HasCargo, hasCargo);
        aWorldState.Set(DeliveryBot.NearBase, nearBase);
     
        // HINT: When you have finished the AI Scenario, just export all conditions
        // as enum and use it to set conditions from the code.
    }
}