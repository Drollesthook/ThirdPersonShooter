using UnityEngine;

namespace Project.Misc {
    public class Bullet : MonoBehaviour {
        [SerializeField] private float _bulletSpeed = 20;

        private float _timeToLive;
        private void Update() {
            Fly();
            CheckForDestroy();
        }

        public void SetFlyDirection(Vector3 direction) {
            transform.LookAt(direction);
            float maxTravelDistance = Vector3.Distance(transform.position, direction);
            _timeToLive = maxTravelDistance / _bulletSpeed;
        }

        private void Fly() {
            transform.Translate(Vector3.forward * _bulletSpeed * Time.deltaTime);
        }

        private void CheckForDestroy() {
            _timeToLive -= Time.deltaTime;
            if (_timeToLive <= 0)
                Destroy(gameObject);
        }
    }
}