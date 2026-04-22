using UnityEngine;

namespace Managers.antAI
{
    public class CollectCargo : MonoBehaviour
    {
        [SerializeField] private PathFinder pathFinder;
        [SerializeField] private IsenseMyGuy sense;
        [SerializeField] private Transform moverRoot;
        [SerializeField] private SeeCargo seeCargoSensor;
        [SerializeField] private RandomNavMeshWanderer movementController;
        [SerializeField] private string cargoTag = "cargo";
        [SerializeField] private float pickupDistance = 0.6f;

        private Transform _targetCargo;
        private Transform _previousPathTarget;

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

            if (seeCargoSensor == null)
            {
                seeCargoSensor = moverRoot.GetComponentInChildren<SeeCargo>();
            }

            if (movementController == null)
            {
                movementController = moverRoot.GetComponent<RandomNavMeshWanderer>();
            }
        }

        private void Update()
        {
            if (sense == null || sense.hasCargo)
            {
                return;
            }

            if (_targetCargo == null && sense.seeCargo && seeCargoSensor != null)
            {
                TrySetCargoTarget(seeCargoSensor.VisibleCargo);
            }

            if (_targetCargo == null)
            {
                return;
            }

            OverridePathToCargo();

            if (Vector3.Distance(moverRoot.position, _targetCargo.position) <= pickupDistance)
            {
                PickupCargo();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            TrySetCargoTarget(other.transform);

            if (_targetCargo != null && other.transform == _targetCargo)
            {
                PickupCargo();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            TrySetCargoTarget(collision.transform);

            if (_targetCargo != null && collision.transform == _targetCargo)
            {
                PickupCargo();
            }
        }

        private void OverridePathToCargo()
        {
            if (_targetCargo == null)
            {
                return;
            }

            if (pathFinder != null && pathFinder.targetTransform != _targetCargo)
            {
                _previousPathTarget = pathFinder.targetTransform;
                pathFinder.targetTransform = _targetCargo;
            }

            if (movementController != null)
            {
                movementController.SetFollowTarget(_targetCargo);
            }
        }

        private void TrySetCargoTarget(Transform candidate)
        {
            if (candidate == null || _targetCargo != null || (sense != null && sense.hasCargo))
            {
                return;
            }

            if (!candidate.CompareTag(cargoTag) && !candidate.name.ToLowerInvariant().Contains(cargoTag))
            {
                return;
            }

            _targetCargo = candidate;

            if (sense != null)
            {
                sense.seeCargo = true;
                sense.searchCargo = false;
            }

            OverridePathToCargo();
        }

        private void PickupCargo()
        {
            if (sense != null)
            {
                sense.hasCargo = true;
                sense.pickupCargo = true;
                sense.seeCargo = false;
            }

            if (movementController != null)
            {
                movementController.ClearFollowTarget();
            }

            if (_targetCargo != null)
            {
                Destroy(_targetCargo.gameObject);
            }

            if (pathFinder != null && pathFinder.targetTransform == _targetCargo)
            {
                pathFinder.targetTransform = _previousPathTarget;
            }

            _previousPathTarget = null;
            _targetCargo = null;
        }
    }
}
