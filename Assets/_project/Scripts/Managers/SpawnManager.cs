using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Managers {
    public class SpawnManager : MonoBehaviour {
        [SerializeField] private List<SpawnPoint> _spawnPoints = new List<SpawnPoint>();
        [SerializeField] private List<Enemy> _enemyPrefabs = new List<Enemy>();
        [SerializeField] private Player _playerPrefab = default;
        [SerializeField] private float _checkForSpawnTimer = 3f;

        private void Awake() {
            StartCoroutine(CheckForNeedToSpawnWithDelay());
        }

        private void CheckForNeedToSpawn() {
            foreach (SpawnPoint spawnPoint in _spawnPoints) {
                if (!spawnPoint.IsAvailableToSpawn) 
                    continue;
                Enemy enemy = Instantiate(GetRandomEnemyFromList(), spawnPoint.transform.position, Quaternion.identity);
                enemy.SetSpawnPoint(spawnPoint);
                spawnPoint.IsAvailableToSpawn = false;
            }
        }

        private Enemy GetRandomEnemyFromList() {
            return _enemyPrefabs[Random.Range(0, _enemyPrefabs.Count - 1)];
        }

        private IEnumerator CheckForNeedToSpawnWithDelay() {
            while (true) {
                CheckForNeedToSpawn();
                yield return new WaitForSeconds(_checkForSpawnTimer);
            }
        }
    }
}
