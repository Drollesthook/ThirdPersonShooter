using Project.Interfaces;

using UnityEngine;

namespace Project.Misc {
    public class Grenade : MonoBehaviour {
        [SerializeField] private float _explodeDelay = 3f;
        [SerializeField] private float _explodeRadius = 5f;
        [SerializeField] private float _damage = 30f;
        [SerializeField] private int _grenadeId = 6;
        [SerializeField] private ParticleSystem _explodePS = default;
        [SerializeField] private LayerMask _wallMask = default;

        private bool _isExploded;
        private bool _isReleased;
        private int _shooterId;
        private int _shooterFractionId;
        private Rigidbody _rigidbody;
        private Collider _collider;

        private void Awake() {
            _collider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update() {
            if(!_isReleased)
                return;
            _explodeDelay -= Time.deltaTime;
            if (_explodeDelay < 0 && !_isExploded)
                Explode();
        }

        public void SetShooterParameters(int shooterId, int shooterFractionId) {
            _shooterId = shooterId;
            _shooterFractionId = shooterFractionId;
        }

        public void Throw(Vector3 velocity) {
            _isReleased = true;
            _collider.enabled = true;
            _rigidbody.isKinematic = false;
            _rigidbody.velocity = velocity;
        }

        private void Explode() {
            _isExploded = true;
            Collider[] hittedColliders = Physics.OverlapSphere(transform.position, _explodeRadius);
            foreach (Collider collider in hittedColliders) {
                IHittable hittable = collider.GetComponent<IHittable>();
                if (hittable != null) {
                    if (IsHittableBehindTheWall(collider))
                        continue;

                    hittable.OnHit(_shooterId, _shooterFractionId, _grenadeId, _damage);
                }

            }
            _explodePS.transform.SetParent(null);
            _explodePS.Play();
            Destroy(gameObject);
        }

        private bool IsHittableBehindTheWall(Collider collider) {
            return Physics.Raycast(transform.position, collider.transform.position, Vector3.Distance(transform.position, collider.transform.position), _wallMask);
            //что-то не то
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _explodeRadius);
        }
    }
}
