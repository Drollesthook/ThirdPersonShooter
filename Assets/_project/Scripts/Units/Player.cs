using Project.Managers;
using Project.Controllers;
using UnityEngine;

namespace Project.Units {
    public class Player : MonoBehaviour {
        [SerializeField] private float _speedMultiplier = 1;
        
        private MovementController _movementController;
        private WeaponController _weaponController;
        private bool _isShooting;
        private Unit _unit;

        private void Awake() {
            _unit = GetComponent<Unit>();
            _weaponController = GetComponent<WeaponController>();
            _movementController = GetComponent<MovementController>();
        }

        private void Start() {
            InputManager.instance.inputDirectionChanged += OnInputDirectionChanged;
            InputManager.instance.weaponSelected += OnWeaponSelected;
            InputManager.instance.fireButtonPressed += OnFireButtonPressed;
            InputManager.instance.fireButtonReleased += OnFireButtonReleased;
            _unit.unitDied += OnUnitDied;
        }

        private void OnDestroy() {
            InputManager.instance.inputDirectionChanged -= OnInputDirectionChanged;
            InputManager.instance.weaponSelected -= OnWeaponSelected;
            InputManager.instance.fireButtonPressed -= OnFireButtonPressed;
            InputManager.instance.fireButtonReleased -= OnFireButtonReleased;
            _unit.unitDied -= OnUnitDied;
        }

        private void Update() {
            if (_isShooting)
                Shoot();
        }

        private void OnInputDirectionChanged(Vector3 direction) {
            _movementController.MoveAndRotate(direction, _speedMultiplier);
        }
        private void OnWeaponSelected(int weaponId) {
            _weaponController.SelectWeapon(weaponId);
        }

        private void OnFireButtonPressed() {
            _isShooting = true;
        }

        private void OnFireButtonReleased() {
            _isShooting = false;
            _weaponController.FireButtonReleased();
        }

        private void OnUnitDied() {
            
        }

        private void Shoot() {
            var aimSightPosition = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            _weaponController.ShootWeapon(aimSightPosition);
        }
    }
}