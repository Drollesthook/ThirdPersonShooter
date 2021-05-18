using UnityEngine;

namespace Project {
    public class MovementController : MonoBehaviour {
        [SerializeField] private float _playerSpeed = 6f;
        [SerializeField] private float _turnSmoothTime = 0.1f;
        [SerializeField] private float _turnSmoothVelocity;
        [SerializeField] private Transform _weaponHolder = default;

        private CharacterController _characterController;
        private Transform _mainCameraTransform;
        private Camera _mainCamera;
        
        private void Awake() {
            _mainCamera = Camera.main;
            _characterController = GetComponent<CharacterController>();
            _mainCameraTransform = Camera.main.transform;
        }

        private void Update() {
            Rotate();
            WeaponHolderRotation();
        }

        public void MoveAndRotate(Vector3 direction, float speedMultiplier) {
            float targetAngle = _mainCameraTransform.eulerAngles.y;

            Vector3 moveDirection =
                Quaternion.Euler(0, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + targetAngle, 0) *
                Vector3.forward;

            _characterController.Move(moveDirection * _playerSpeed * speedMultiplier * Time.deltaTime);
        }

        private void Rotate() {
            float targetAngle = _mainCameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                                                _turnSmoothTime);

            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
        
        private void WeaponHolderRotation() {
            float targetAngle = _mainCamera.transform.eulerAngles.x;
            _weaponHolder.localRotation = Quaternion.Euler(targetAngle, 0, 0);
        }
    }
}