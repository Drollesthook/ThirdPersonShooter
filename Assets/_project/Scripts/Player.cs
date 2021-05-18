using Project.Managers;

using UnityEngine;

namespace Project {
    public class Player : MonoBehaviour {
        MovementController _movementController;
        [SerializeField] float _speedMultiplyer = 1;
        WeaponController _weaponController;

        void Awake() {
            _weaponController = GetComponent<WeaponController>();
            _movementController = GetComponent<MovementController>();
        }

        void Start() {
            InputManager.instance.InputDirectionChanged += InputDirectionChanged;
        }

        void OnDestroy() {
            InputManager.instance.InputDirectionChanged -= InputDirectionChanged;
        }

        void Update() {
            if (Input.GetMouseButton(0))
                Shoot();
        }

        void InputDirectionChanged(Vector3 direction) {
            _movementController.MoveAndRotate(direction, _speedMultiplyer);
        }

        void Shoot() {
            var aimSightPosition = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            _weaponController.ShootWeapon(aimSightPosition);
        }
    }
}