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
        [SerializeField] private List<fractionUnits> _listOfFractions = new List<fractionUnits>();

        [Serializable]
        public class fractionUnits {
            public List<Unit> _listOfBotUnits = new List<Unit>();
            public List<Unit> _listOfDummyUnits = new List<Unit>();
        }
        public static UnitsHolderManager instance => _instance;

        public int playerFractionId => _playerFractionId;

        private List<Unit> _spawnedUnits = new List<Unit>();
        private int _playerFractionId;

        private void Awake() {
            _instance = this;
            _playerFractionId = _playerPrefab.fractionIdentifier;
        }

        public void UnitSpawned(Unit unit) {
            _spawnedUnits.Add(unit);
            unitSpawned?.Invoke(unit.fractionIdentifier);
        }

        public void UnitDied(Unit unit) {
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

        public Unit GetPlayerPrefab() {
            return _playerPrefab;
        }

        public Unit GetRandomBotByFractionId(int fractionId) {
            if (fractionId < _listOfFractions.Count)
                return _listOfFractions[fractionId]
                    ._listOfBotUnits[Random.Range(0, _listOfFractions[fractionId]._listOfBotUnits.Count)];

            Debug.LogError("There is no spawn position for " + fractionId + " fractions bots");
            return null;

        }

        public Unit GetRandomDummyByFractionId(int fractionId) {
            if (fractionId < _listOfFractions.Count)
            return _listOfFractions[fractionId]
                ._listOfDummyUnits[Random.Range(0, _listOfFractions[fractionId]._listOfDummyUnits.Count)];
            
            Debug.LogError("There is no spawn position for " + fractionId + " fractions dummies");
            return null;
        }
    }
}
