using Project.Misc;
using Project.Units;

using UnityEngine;

namespace Project.Controllers {
    public class GrenadeController : MonoBehaviour {
        [SerializeField] private Grenade _grenadePrefab = default;
        [SerializeField] private Transform _weaponHolder = default;
        [SerializeField] private float _maxThrowForce = default;
        [SerializeField] private Vector3 _defaultDirection = new Vector3(0,1,1);
        [SerializeField] private LineRenderer _lineRenderer = default;
        [SerializeField] private int _iterations = 20;

        private Unit unit;
        private Grenade _currentGrenade;

        private void Awake() {
            unit = GetComponent<Unit>();
            _lineRenderer.positionCount = _iterations;
        }
        
        public void EquippGrenade() {
            _currentGrenade = Instantiate(_grenadePrefab, _weaponHolder);
            _lineRenderer.enabled = true;
        }
        
        public void ThrowGrenade(float force) {
            _currentGrenade.transform.SetParent(null);
            _currentGrenade.SetShooterParameters(unit.unitIdentifier, unit.fractionIdentifier);
            _currentGrenade.Throw(force * _maxThrowForce * (transform.forward + transform.up));
            _lineRenderer.enabled = false;
        }

        public void VisualizeAim(float force) {
            for (int i = 0; i < _iterations; i++) {
                Vector3 position = CalculatePosInTime((transform.forward + transform.up) * force * _maxThrowForce, i /(float)_iterations);
                _lineRenderer.SetPosition(i,position);
            }
        }

        private Vector3 CalculatePosInTime(Vector3 startVelocity, float time) {
            Vector3 Vxz = startVelocity;
            Vxz.y = 0f;

            Vector3 resultPos = _weaponHolder.position + startVelocity * time;
            float sY = (-0.5f * Mathf.Abs(Physics.gravity.y) * time * time) + startVelocity.y * time + _weaponHolder.position.y;
            resultPos.y = sY;
            return resultPos;
        }

        
    }
}
