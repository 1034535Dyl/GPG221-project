using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Managers.antAI
{
    public class CargoRespawner : MonoBehaviour
    {
        [SerializeField] private IsenseMyGuy sense;
        [SerializeField] private GameObject cargoPrefab;
        [SerializeField] private Transform spawnCenter;
        [SerializeField] private float respawnDelaySeconds = 3f;
        [SerializeField] private float spawnRadius = 20f;
        [SerializeField] private float sampleRadius = 5f;
        [SerializeField] private int maxAttempts = 10;

        private Coroutine _respawnRoutine;
        private bool _hasLoggedMissingPrefab;

        private void Awake()
        {
            if (sense == null)
            {
                sense = transform.root.GetComponent<IsenseMyGuy>();
            }

            if (spawnCenter == null)
            {
                spawnCenter = transform;
            }
        }

        private void Update()
        {
            if (sense == null || !sense.isCargoDelivered || _respawnRoutine != null)
            {
                return;
            }

            if (cargoPrefab == null)
            {
                if (!_hasLoggedMissingPrefab)
                {
                    Debug.LogWarning("CargoRespawner: Cargo prefab is not assigned.");
                    _hasLoggedMissingPrefab = true;
                }
                return;
            }

            Debug.Log("CargoRespawner: Cargo delivered detected. Starting respawn countdown...");
            _respawnRoutine = StartCoroutine(RespawnAfterDelay());
        }

        private IEnumerator RespawnAfterDelay()
        {
            yield return new WaitForSeconds(respawnDelaySeconds);

            if (TrySpawnCargoAtRandomLocation())
            {
                Debug.Log("CargoRespawner: Cargo respawned successfully.");
            }
            else
            {
                Debug.LogWarning("CargoRespawner: Failed to find a valid NavMesh spawn point.");
            }

            _respawnRoutine = null;
        }

        private bool TrySpawnCargoAtRandomLocation()
        {
            Vector3 origin = spawnCenter != null ? spawnCenter.position : transform.position;

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                Vector3 randomPoint = origin + Random.insideUnitSphere * spawnRadius;
                randomPoint.y = origin.y;

                if (!NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, sampleRadius, NavMesh.AllAreas))
                {
                    continue;
                }

                Instantiate(cargoPrefab, hit.position, Quaternion.identity);
                Debug.Log($"CargoRespawner: Cargo instantiated at {hit.position}");
                return true;
            }

            return false;
        }
    }
}
