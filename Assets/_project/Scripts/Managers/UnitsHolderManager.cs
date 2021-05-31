using System;
using System.Collections;
using System.Collections.Generic;

using Project.Units;

using UnityEngine;

using Random = UnityEngine.Random;

namespace Project.Managers {
    public class UnitsHolderManager : MonoBehaviour {
        public event Action<int> unitSpawned; 
        private static UnitsHolderManager _instance;
        [SerializeField] private Unit _playerPrefab = default;
        [SerializeField] private List<fractionUnits> _listOfFractionsUnitsPrefabs = new List<fractionUnits>();

        [Serializable]
        public class fractionUnits {
            public List<Unit> _listOfBotUnits = new List<Unit>();
            public List<Unit> _listOfDummyUnits = new List<Unit>();
        }

        private class spawnedFractions {
            public List<Unit> _listOfUnits = new List<Unit>();
        }
        public static UnitsHolderManager instance => _instance;

        public int playerFractionId => _playerFractionId;

        private List<Unit> _spawnedUnits = new List<Unit>();
        private List<Unit>[] _listOfSpawnedFractions;
        private int _playerFractionId;

        private void Awake() {
            _instance = this;
            _playerFractionId = _playerPrefab.fractionIdentifier;
        }

        private void Start() {
            _listOfSpawnedFractions = new List<Unit>[_listOfFractionsUnitsPrefabs.Count];
            for (int i = 0; i < _listOfSpawnedFractions.Length; i++) {
                _listOfSpawnedFractions[i] = new List<Unit>();
            }
        }

        public void UnitSpawned(Unit unit) {
            _spawnedUnits.Add(unit);
            AddUnitToFractionList(unit);
            unitSpawned?.Invoke(unit.fractionIdentifier);
        }

        public void UnitDied(Unit unit) {
            RemoveUnitFromFractionList(unit);
            _spawnedUnits.Remove(unit);
        }

        public Transform GetHostileUnitsTransformsByUnitId(int id) {
            List<Unit> listOfHostileUnits = new List<Unit>();
            foreach (Unit unit in _spawnedUnits) {
                if (unit.fractionIdentifier == id)
                    continue;
                listOfHostileUnits.Add(unit);
            }
            return listOfHostileUnits.Count == 0 ? null : listOfHostileUnits[Random.Range(0, listOfHostileUnits.Count - 1)].transform;
        }

        public Transform[] GetAllUnitsTransforms() {
            Transform[] unitsTransforms = new Transform[_spawnedUnits.Count];
            for (int i = 0; i < unitsTransforms.Length; i++) {
                unitsTransforms[i] = _spawnedUnits[i].transform;
            }
            return unitsTransforms;
        }

        public Unit[] GetSpawnedUnitsByFractionId(int fractionId) {
            if (_listOfSpawnedFractions.Length <= fractionId) {
                Debug.LogWarning("There are no fraction with " + fractionId + "id");
                return null;
            }
            Unit[] units = new Unit[_listOfSpawnedFractions[fractionId].Count];
            for (int i = 0; i < units.Length; i++) {
                units[i] = _listOfSpawnedFractions[fractionId][i];
            }
            return units;
        }

        public Unit GetPlayerPrefab() {
            return _playerPrefab;
        }

        public Unit GetRandomBotByFractionId(int fractionId) {
            if (fractionId < _listOfFractionsUnitsPrefabs.Count)
                return _listOfFractionsUnitsPrefabs[fractionId]
                    ._listOfBotUnits[Random.Range(0, _listOfFractionsUnitsPrefabs[fractionId]._listOfBotUnits.Count)];

            Debug.LogError("There is no spawn position for " + fractionId + " fractions bots");
            return null;

        }

        public Unit GetRandomDummyByFractionId(int fractionId) {
            if (fractionId < _listOfFractionsUnitsPrefabs.Count)
            return _listOfFractionsUnitsPrefabs[fractionId]
                ._listOfDummyUnits[Random.Range(0, _listOfFractionsUnitsPrefabs[fractionId]._listOfDummyUnits.Count)];
            
            Debug.LogError("There is no spawn position for " + fractionId + " fractions dummies");
            return null;
        }

        private void AddUnitToFractionList(Unit unit) {
            int fractionId = unit.fractionIdentifier;
            if (_listOfSpawnedFractions.Length <= fractionId) {
                Debug.LogWarning("There are no fraction with " + fractionId + "id");
                return;
            }
            _listOfSpawnedFractions[fractionId].Add(unit);
        }

        private void RemoveUnitFromFractionList(Unit unit) {
            int fractionId = unit.fractionIdentifier;
            if (_listOfSpawnedFractions.Length <= fractionId) {
                Debug.LogWarning("There are no fraction with " + fractionId + "id");
                return;
            }

            _listOfSpawnedFractions[fractionId].Remove(unit);
        }
    }
}
