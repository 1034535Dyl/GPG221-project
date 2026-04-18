using UnityEngine;

namespace Managers.antAI
{
    public class SeeCargo : MonoBehaviour
    {
        [SerializeField] private IsenseMyGuy sense;
        [SerializeField] private float sightRange = 8f;
        [SerializeField] private string cargoTag = "cargo";
        [SerializeField] private LayerMask visionMask = ~0;
        [SerializeField] private Vector3 rayOriginOffset = new Vector3(0f, 0.2f, 0f);
        [SerializeField] private Color canSeeColor = Color.green;
        [SerializeField] private Color cannotSeeColor = Color.red;

        public Transform VisibleCargo { get; private set; }

        private void Awake()
        {
            if (sense == null)
            {
                sense = transform.root.GetComponent<IsenseMyGuy>();
            }
        }

        private void Update()
        {
            bool canSeeCargo = SeeCargoInSight();

            if (sense != null)
            {
                sense.seeCargo = canSeeCargo;
            }

            Vector3 origin = transform.position + transform.TransformDirection(rayOriginOffset);
            Debug.DrawRay(origin, transform.forward * sightRange, canSeeCargo ? canSeeColor : cannotSeeColor);
        }

        // True only when the first object hit in forward direction is cargo.
        public bool SeeCargoInSight()
        {
            Vector3 origin = transform.position + transform.TransformDirection(rayOriginOffset);

            if (Physics.Raycast(origin, transform.forward, out RaycastHit hit, sightRange, visionMask, QueryTriggerInteraction.Ignore))
            {
                Transform hitTransform = hit.transform;
                if (hitTransform.CompareTag(cargoTag))
                {
                    VisibleCargo = hitTransform;
                    return true;
                }

                if (hitTransform.root.CompareTag(cargoTag))
                {
                    VisibleCargo = hitTransform.root;
                    return true;
                }
            }

            VisibleCargo = null;
            return false;
        }
    }


}