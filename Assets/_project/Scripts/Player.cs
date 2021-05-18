using Project.Managers;

using UnityEngine;

namespace Project {
    public class Player : MonoBehaviour {
        [SerializeField] float _speedMultiplyer = 1;
        
        MovementController _movementController;
        WeaponController _weaponController;
        bool _isShooting;

        void Awake() {
            _weaponController = GetComponent<WeaponController>();
            _movementController = GetComponent<MovementController>();
        }

        void Start() {
            InputManager.instance.inputDirectionChanged += OnInputDirectionChanged;
            InputManager.instance.fireButtonPressed += OnFireButtonPressed;
            InputManager.instance.fireButtonReleased += OnFireButtonReleased;
        }

        void OnDestroy() {
            InputManager.instance.inputDirectionChanged -= OnInputDirectionChanged;
            InputManager.instance.fireButtonPressed -= OnFireButtonPressed;
            InputManager.instance.fireButtonReleased -= OnFireButtonReleased;
        }

        void Update() {
            if (_isShooting)
                Shoot();
        }

        void OnInputDirectionChanged(Vector3 direction) {
            _movementController.MoveAndRotate(direction, _speedMultiplyer);
        }

        void OnFireButtonPressed() {
            _isShooting = true;
        }

        void OnFireButtonReleased() {
            _isShooting = false;
            _weaponController.FireButtonReleased();
        }

        void Shoot() {
            var aimSightPosition = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            _weaponController.ShootWeapon(aimSightPosition);
        }
    }
}