using Project.Misc;
using Project.Units;

using UnityEngine;

namespace Project.Controllers {
    public class GrenadeController : MonoBehaviour {
        [SerializeField] private Grenade _grenadePrefab = default;
        [SerializeField] private Transform _weaponHolder = default;
        [SerializeField] private float _maxThrowForce = default;
        [Header("LineRenderer Setup")]
        [SerializeField] private LineRenderer _lineRenderer = default;
        [SerializeField] private float _maxLineRendererLength = 3f;
        [SerializeField] private int _iterations = 20;
        [SerializeField] private LayerMask _grenadeCollisionsMask;

        private Unit unit;
        private Grenade _currentGrenade;

        private void Awake() {
            unit = GetComponent<Unit>();
            _lineRenderer.positionCount = _iterations;
            _lineRenderer.enabled = false;
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
            Vector3 startVelocity = (transform.forward + transform.up) * force * _maxThrowForce;
            DrawLineRenderer(startVelocity, GetAmountOfIterations(startVelocity) + 1);
        }

        private int GetAmountOfIterations(Vector3 velocity) {
            int i = 0;
            for (; i < _iterations; i++) {
                Vector3 position = CalculatePosInTime(velocity, i*_maxLineRendererLength /(float)_iterations);
                Vector3 nexPosition = CalculatePosInTime(velocity, (i + 1) * _maxLineRendererLength / (float) _iterations);
                if (Physics.Raycast(position, nexPosition - position, Vector3.Distance(position, nexPosition), _grenadeCollisionsMask)) {
                    break;
                }
            }
            return i;
        }

        private void DrawLineRenderer(Vector3 velocity, int iterations) {
            _lineRenderer.positionCount = iterations;
            for (int i = 0; i < iterations; i++) {
                Vector3 position = CalculatePosInTime(velocity, i * _maxLineRendererLength / (float) _iterations);
                _lineRenderer.SetPosition(i, position);
            }
        }

        private Vector3 CalculatePosInTime(Vector3 startVelocity, float time) {
            Vector3 resultPos = _weaponHolder.position + startVelocity * time;
            float sY = (-0.5f * Mathf.Abs(Physics.gravity.y) * time * time) + startVelocity.y * time + _weaponHolder.position.y;
            resultPos.y = sY;
            return resultPos;
        }
    }
}
