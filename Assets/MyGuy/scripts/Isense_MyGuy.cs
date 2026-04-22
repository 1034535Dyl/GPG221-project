using Anthill.AI;
using UnityEngine;

namespace Managers.antAI
{
    public class IsenseMyGuy : MonoBehaviour, ISense
    {
        public bool hasCargo;
        public bool isCargoDelivered;
        public bool nearbase;
        public bool seeCargo;
        public bool seeBase; 
        public bool pickupCargo;
        public bool searchCargo;
  
  

        public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
        {
            aWorldState.Set(Behaviourtree.hascargo, hasCargo);
            aWorldState.Set(Behaviourtree.iscargodelivered, isCargoDelivered);
            aWorldState.Set(Behaviourtree.nearbase, nearbase);
            aWorldState.Set(Behaviourtree.seecargo, seeCargo);
            aWorldState.Set(Behaviourtree.seebase, seeBase);
            aWorldState.Set(Behaviourtree.pickupCargo, pickupCargo);
            aWorldState.Set(Behaviourtree.searchCargo, searchCargo);

        }
    }
}
