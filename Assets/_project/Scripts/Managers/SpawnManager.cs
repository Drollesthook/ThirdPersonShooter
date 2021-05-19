using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Units;

namespace Project.Managers {
    public class SpawnManager : MonoBehaviour {
        [SerializeField] private List<fractionsSpawnPos> _listOfFractionsSpawnPoses = new List<fractionsSpawnPos>();
        
        [Serializable]
        public class fractionsSpawnPos {
            public Transform _botsSpawnPointsParent;
            public Transform _dummiesSpawnPointsParent;
        }

        private void Awake() {
            SpawnUnits();
        }

        void SpawnPlayer() {
            
        }

        void SpawnUnits() {
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
