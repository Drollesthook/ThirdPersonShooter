using System;
using System.Collections;
using System.Collections.Generic;

using Project.Units;

using UnityEngine;

using Random = UnityEngine.Random;

namespace Project.Managers {
    public class UnitsHolderManager : MonoBehaviour {
        private static UnitsHolderManager _instance;
        [SerializeField] private Unit _playerPrefab = default;
        [SerializeField] private List<fractionUnits> _listOfFractions = new List<fractionUnits>();

        [Serializable]
        public class fractionUnits {
            public List<Unit> _listOfBotUnits = new List<Unit>();
            public List<Unit> _listOfDummyUnits = new List<Unit>();
        }
        public static UnitsHolderManager instance => _instance;
        private List<Unit> _spawnedUnits = new List<Unit>();

        private void Awake() {
            _instance = this;
        }

        public void UnitSpawned(Unit unit) {
            _spawnedUnits.Add(unit);
        }

        public void UnitDied(Unit unit) {
            _spawnedUnits.Remove(unit);
        }

        public Unit GetPlayerPrefab() {
            return _playerPrefab;
        }

        public Unit GetRandomBotByFractionId(int fractionId) {
            if (fractionId < _listOfFractions.Count)
                return _listOfFractions[fractionId]
                    ._listOfBotUnits[Random.Range(0, _listOfFractions[fractionId]._listOfBotUnits.Count)];

            print("There is no spawn position for " + fractionId + " fractions bots");
            return null;

        }

        public Unit GetRandomDummyByFractionId(int fractionId) {
            if (fractionId < _listOfFractions.Count)
            return _listOfFractions[fractionId]
                ._listOfDummyUnits[Random.Range(0, _listOfFractions[fractionId]._listOfDummyUnits.Count)];
            
            print("There is no spawn position for " + fractionId + " fractions dummies");
            return null;
        }
    }
}
