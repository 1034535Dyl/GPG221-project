using Anthill.AI;
using Managers.antAI;
using UnityEngine;

public class Placehoder : AntAIState
{
    public override void Create(GameObject aGameObject)
    {
        base.Create(aGameObject);
        aGameObject.GetComponent<IsenseMyGuy>().searchCargo = true;
    }
}
