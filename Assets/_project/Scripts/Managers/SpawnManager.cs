using System;
using System.Collections.Generic;
using UnityEngine;
using Project.Misc;
using Project.Units;
using Random = UnityEngine.Random;

namespace Project.Managers {
    public class SpawnManager : MonoBehaviour {
        private static SpawnManager _instance;
        public event Action<Unit> PlayerSpawned;
        [SerializeField] private List<SpawnPoint> _playerSpawnPoints = new List<SpawnPoint>();
        [SerializeField] private List<fractionsSpawnPos> _listOfFractionsSpawnPoses = new List<fractionsSpawnPos>();
        [SerializeField] private float _respawnTimer = 5f;

        public static SpawnManager instance => _instance;
        
        [Serializable]
        public class fractionsSpawnPos {
            public List<SpawnPoint> _listOfBotsSpawnPoints = new List<SpawnPoint>();
            public List<SpawnPoint> _listOfDummiesSpawnPoints = new List<SpawnPoint>();
        }

        
        private void Awake() {
            _instance = this;
        }

        private void Start() {
            SpawnPlayer();
            SpawnUnits();
        }

        public void SpawnPlayer() {
            PlayerSpawned?.Invoke(_playerSpawnPoints[Random.Range(0, _playerSpawnPoints.Count - 1)].SpawnPlayer());
        }

        private void SpawnUnits() {
            for (int i = 0; i < _listOfFractionsSpawnPoses.Count; i++) {
                foreach (SpawnPoint spawnPoint in _listOfFractionsSpawnPoses[i]._listOfBotsSpawnPoints) {
                    spawnPoint.SpawnUnit(i,false, _respawnTimer);
                }
                foreach (SpawnPoint spawnPoint in _listOfFractionsSpawnPoses[i]._listOfDummiesSpawnPoints) {
                    spawnPoint.SpawnUnit(i,true, _respawnTimer);
                }
            }
        }
    }
}
