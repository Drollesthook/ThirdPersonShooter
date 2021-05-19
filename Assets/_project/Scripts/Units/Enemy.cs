using UnityEngine;

namespace Project.Units {
    public class Enemy : MonoBehaviour {
        private Unit _unit;

        private void Awake() {
            _unit = GetComponent<Unit>();
        }

        private void Start() {
            _unit.unitDied += OnUnitDied;
        }

        private void OnDestroy() {
            _unit.unitDied -= OnUnitDied;
        }

        private void OnUnitDied() {
            Destroy(gameObject);
        }
    }
}
