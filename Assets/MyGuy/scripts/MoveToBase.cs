using System.Collections;
using UnityEngine;

namespace Managers.antAI
{
    public class MoveToBase : MonoBehaviour
    {
        [SerializeField] private PathFinder pathFinder;
        [SerializeField] private IsenseMyGuy sense;
        [SerializeField] private Transform moverRoot;
        [SerializeField] private RandomNavMeshWanderer movementController;
        [SerializeField] private string baseTag = "base";
        [SerializeField] private float dropOffDistance = 0.6f;
        [SerializeField] private float deliveryResetDelaySeconds = 3f;

        private Transform _targetBase;
        private Transform _previousPathTarget;
        private Coroutine _deliveryResetRoutine;

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

            if (movementController == null)
            {
                movementController = moverRoot.GetComponent<RandomNavMeshWanderer>();
            }
        }

        private void Update()
        {
            if (sense == null)
            {
                return;
            }

            if (!sense.hasCargo)
            {
                return;
            }

            if (_targetBase == null)
            {
                TrySetBaseTarget();
            }

            if (_targetBase == null)
            {
                return;
            }

            OverridePathToBase();

            if (Vector3.Distance(moverRoot.position, _targetBase.position) <= dropOffDistance)
            {
                DeliverCargo();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_targetBase != null && other.transform == _targetBase)
            {
                DeliverCargo();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_targetBase != null && collision.transform == _targetBase)
            {
                DeliverCargo();
            }
        }

        private void TrySetBaseTarget()
        {
            GameObject baseObject = GameObject.FindGameObjectWithTag(baseTag);
            if (baseObject == null)
            {
                return;
            }

            _targetBase = baseObject.transform;
            OverridePathToBase();
        }

        private void OverridePathToBase()
        {
            if (_targetBase == null)
            {
                return;
            }

            if (pathFinder != null && pathFinder.targetTransform != _targetBase)
            {
                _previousPathTarget = pathFinder.targetTransform;
                pathFinder.targetTransform = _targetBase;
            }

            if (movementController != null)
            {
                movementController.SetFollowTarget(_targetBase);
            }
        }

        private void DeliverCargo()
        {
            if (sense != null)
            {
                sense.hasCargo = false;
                sense.isCargoDelivered = true;
                sense.nearbase = true;
                sense.pickupCargo = false;
            }

            if (movementController != null)
            {
                movementController.ClearFollowTarget();
            }

            if (pathFinder != null && pathFinder.targetTransform == _targetBase)
            {
                pathFinder.targetTransform = _previousPathTarget;
            }

            if (_deliveryResetRoutine != null)
            {
                StopCoroutine(_deliveryResetRoutine);
            }

            _deliveryResetRoutine = StartCoroutine(ResetDeliveryFlagAfterDelay());

            _previousPathTarget = null;
            _targetBase = null;
        }

        private IEnumerator ResetDeliveryFlagAfterDelay()
        {
            yield return new WaitForSeconds(deliveryResetDelaySeconds);

            if (sense != null)
            {
                sense.isCargoDelivered = false;
                sense.nearbase = false;
                Debug.Log("MoveToBase: Delivery reset flag after delay.");
            }

            _deliveryResetRoutine = null;
        }
    }
}


