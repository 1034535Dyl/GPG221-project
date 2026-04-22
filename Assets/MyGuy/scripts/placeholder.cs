using Anthill.AI;
using UnityEngine;

namespace MyGuy.scripts
{
    public class Placehoder : AntAIState
    {
        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);
            aGameObject.GetComponent<IsenseMyGuy>().searchCargo = true;
        }
    }
}
