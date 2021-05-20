using System.Collections;

using Project.Managers;
using Project.Units;

using UnityEngine;

namespace Project.Misc {
    public class SpawnPoint : MonoBehaviour {
        
        private float _respawnTimer = 5f;
        private int _myUnitsFractionId;
        private bool _isMyUnitDummy;
        private bool _isMyUnitPlayer;

        public Unit SpawnPlayer() {
            _isMyUnitPlayer = true;
            Unit unit = Instantiate(UnitsHolderManager.instance.GetPlayerPrefab(), transform.position, Quaternion.identity);
            unit.SetSpawnPoint(this);
            UnitsHolderManager.instance.UnitSpawned(unit);
            return unit;
        }

        public void SpawnUnit(int fractionId, bool _isDummy, float respawnTimer) {
            _myUnitsFractionId = fractionId;
            _isMyUnitDummy = _isDummy;
            _respawnTimer = respawnTimer;
            Unit unit = Instantiate(_isDummy
                                        ? UnitsHolderManager.instance.GetRandomDummyByFractionId(fractionId)
                                        : UnitsHolderManager.instance.GetRandomBotByFractionId(fractionId),
                                    transform.position, Quaternion.identity);

            unit.SetSpawnPoint(this);
            UnitsHolderManager.instance.UnitSpawned(unit);
        }

        public void UnitDied() {
            if (_isMyUnitPlayer)
                print("gonna notify GameManager about that");
            else {
                StartCoroutine(SpawnUnitWithDelay());
            }
        }

        IEnumerator SpawnUnitWithDelay() {
            yield return new WaitForSeconds(_respawnTimer);
            SpawnUnit(_myUnitsFractionId, _isMyUnitDummy, _respawnTimer);
        }
    }
}
