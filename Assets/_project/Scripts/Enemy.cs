using UnityEngine;

namespace Project {
    public class Enemy : MonoBehaviour {
        private Unit _unit;
        private SpawnPoint _mySpawnPoint;

        private void Awake() {
            _unit = GetComponent<Unit>();
        }

        private void Start() {
            _unit.unitDied += OnUnitDied;
        }

        private void OnDestroy() {
            _unit.unitDied -= OnUnitDied;
        }

        public void SetSpawnPoint(SpawnPoint spawnPoint) {
            _mySpawnPoint = spawnPoint;
        }

        private void OnUnitDied() {
            if (_mySpawnPoint != null)
                _mySpawnPoint.IsAvailableToSpawn = true;
            Destroy(gameObject);
        }
    }
}
