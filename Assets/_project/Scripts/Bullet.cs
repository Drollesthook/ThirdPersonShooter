using UnityEngine;

namespace Project {
    public class Bullet : MonoBehaviour {
        [SerializeField] private float _bulletSpeed = 20;
        [SerializeField] private float _timeToLive = 3f;

        private void Update() {
            Fly();
            CheckForDestroy();
        }

        private void OnTriggerEnter(Collider other) {
            Destroy(gameObject);
        }

        public void SetFlyDirection(Vector3 direction) {
            transform.LookAt(direction);
        }

        private void Fly() {
            transform.Translate(Vector3.forward * _bulletSpeed);
        }

        private void CheckForDestroy() {
            _timeToLive -= Time.deltaTime;
            if (_timeToLive <= 0)
                Destroy(gameObject);
        }
    }
}