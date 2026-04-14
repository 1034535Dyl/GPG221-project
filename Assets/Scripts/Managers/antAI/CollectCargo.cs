using UnityEngine;

namespace Managers.antAI
{
    public class CollectCargo : MonoBehaviour
    {
        [SerializeField] private PathFinder pathFinder;
        [SerializeField] private IsenseMyGuy sense;
        [SerializeField] private Transform moverRoot;
        [SerializeField] private string cargoTag = "cargo";
        [SerializeField] private float pickupDistance = 0.6f;
        [SerializeField] private bool destroyCargoOnPickup = true;

        private Transform targetCargo;

        private void Awake()
        {
            if (moverRoot == null)
            {
                moverRoot = transform.root;
            }

            if (pathFinder == null)
            {
                pathFinder = moverRoot.GetComponent<PathFinder>();
            }

            if (sense == null)
            {
                sense = moverRoot.GetComponent<IsenseMyGuy>();
            }
        }

        private void Update()
        {
            if (targetCargo == null || sense == null || sense.hasCargo)
            {
                return;
            }

            if (Vector3.Distance(moverRoot.position, targetCargo.position) <= pickupDistance)
            {
                PickupCargo();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            TrySetCargoTarget(other.transform);
        }

        private void OnCollisionEnter(Collision collision)
        {
            TrySetCargoTarget(collision.transform);
        }

        private void TrySetCargoTarget(Transform candidate)
        {
            if (candidate == null || targetCargo != null || (sense != null && sense.hasCargo))
            {
                return;
            }

            if (!candidate.CompareTag(cargoTag) && !candidate.name.ToLowerInvariant().Contains(cargoTag))
            {
                return;
            }

            targetCargo = candidate;

            if (pathFinder != null)
            {
                pathFinder.targetTransform = targetCargo;
            }

            if (sense != null)
            {
                sense.seeCargo = true;
                sense.searchCargo = false;
            }
        }

        private void PickupCargo()
        {
            if (sense != null)
            {
                sense.hasCargo = true;
                sense.pickupCargo = true;
                sense.seeCargo = false;
            }

            if (pathFinder != null && pathFinder.targetTransform == targetCargo)
            {
                pathFinder.targetTransform = null;
            }

            if (targetCargo != null)
            {
                if (destroyCargoOnPickup)
                {
                    Destroy(targetCargo.gameObject);
                }
                else
                {
                    targetCargo.SetParent(moverRoot);
                    targetCargo.localPosition = Vector3.up * 0.6f;
                }
            }

            targetCargo = null;
        }
    }
}
