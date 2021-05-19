using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Misc;
using Project.Units;
using Random = UnityEngine.Random;

namespace Project.Managers {
    public class SpawnManager : MonoBehaviour {
        private static SpawnManager _instance;
        public event Action<Unit> PlayerSpawned;
        [SerializeField] private Transform _playerSpawnPointsParent = default;
        [SerializeField] private List<fractionsSpawnPos> _listOfFractionsSpawnPoses = new List<fractionsSpawnPos>();

        public static SpawnManager instance => _instance;
        
        [Serializable]
        public class fractionsSpawnPos {
            public Transform _botsSpawnPointsParent;
            public Transform _dummiesSpawnPointsParent;
        }

        
        private void Awake() {
            _instance = this;
        }

        private void Start() {
            SpawnPlayer();
            SpawnUnits();
        }

        public void SpawnPlayer() {
            SpawnPoint[] spawnPoints = _playerSpawnPointsParent.GetComponentsInChildren<SpawnPoint>();
            PlayerSpawned?.Invoke(spawnPoints[Random.Range(0, spawnPoints.Length - 1)].SpawnPlayer());
        }

        private void SpawnUnits() {
            for (int i = 0; i < _listOfFractionsSpawnPoses.Count; i++) {
                SpawnPoint[] spawnPoints = _listOfFractionsSpawnPoses[i]
                                           ._botsSpawnPointsParent.GetComponentsInChildren<SpawnPoint>();

                foreach (SpawnPoint spawnPoint in spawnPoints) {
                    spawnPoint.SpawnUnit(i,false);
                }
                spawnPoints = _listOfFractionsSpawnPoses[i]
                                           ._dummiesSpawnPointsParent.GetComponentsInChildren<SpawnPoint>();

                foreach (SpawnPoint spawnPoint in spawnPoints) {
                    spawnPoint.SpawnUnit(i,true);
                }
            }
        }
    }
}
