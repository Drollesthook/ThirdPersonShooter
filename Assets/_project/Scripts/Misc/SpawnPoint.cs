using System.Collections;

using Project.Managers;
using Project.Units;

using UnityEngine;

public class SpawnPoint : MonoBehaviour {
    [SerializeField] private float _respawnTimer = 5f;

    private int _myUnitsFractionId;
    private bool _isMyUnitDummy;
    private bool _isMyUnitPlayer;

    public void SpawnPlayer() {
        _isMyUnitPlayer = true;
        Unit unit = Instantiate(UnitsHolderManager.instance.GetPlayerPrefab());
        unit.SetSpawnPoint(this);
    }
    public void SpawnUnit(int fractionId, bool _isDummy) {
        _myUnitsFractionId = fractionId;
        _isMyUnitDummy = _isDummy;
        Unit unit = Instantiate(_isDummy
                                    ? UnitsHolderManager.instance.GetRandomDummyByFractionId(fractionId)
                                    : UnitsHolderManager.instance.GetRandomBotByFractionId(fractionId), transform.position, Quaternion.identity);
        unit.SetSpawnPoint(this);
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
        SpawnUnit(_myUnitsFractionId, _isMyUnitDummy);
    }
}
