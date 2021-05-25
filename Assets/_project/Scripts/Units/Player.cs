using Project.Managers;
using Project.Controllers;
using UnityEngine;

namespace Project.Units {
    public class Player : MonoBehaviour {
        [SerializeField] private float _speedMultiplier = 1;

        private MovementController _movementController;
        private WeaponController _weaponController;
        private GrenadeController _grenadeController;
        private bool _isShooting;
        private bool _isGrenadeEquipped;
        private bool _isAbleToShoot = true;
        private Unit _unit;

        private void Awake() {
            _unit = GetComponent<Unit>();
            _grenadeController = GetComponent<GrenadeController>();
            _weaponController = GetComponent<WeaponController>();
            _movementController = GetComponent<MovementController>();
        }

        private void Start() {
            InputManager.instance.inputMoveDirectionChanged += OnInputMoveDirectionChanged;
            InputManager.instance.weaponSelected += OnWeaponSelected;
            InputManager.instance.fireButtonPressed += OnFireButtonPressed;
            InputManager.instance.fireButtonReleased += OnFireButtonReleased;
            InputManager.instance.grenadeButtonPressed += OnGrenadeButtonPressed;
            InputManager.instance.grenadeButtonReleased += OnGrenadeButtonReleased;
            InputManager.instance.grenadeForceChanged += OnGrenadeForceChanged;
            _unit.unitDied += OnUnitDied;
        }

        private void OnDestroy() {
            InputManager.instance.inputMoveDirectionChanged -= OnInputMoveDirectionChanged;
            InputManager.instance.weaponSelected -= OnWeaponSelected;
            InputManager.instance.fireButtonPressed -= OnFireButtonPressed;
            InputManager.instance.fireButtonReleased -= OnFireButtonReleased;
            InputManager.instance.grenadeButtonPressed -= OnGrenadeButtonPressed;
            InputManager.instance.grenadeButtonReleased -= OnGrenadeButtonReleased;
            InputManager.instance.grenadeForceChanged -= OnGrenadeForceChanged;
            _unit.unitDied -= OnUnitDied;
        }

        private void Update() {
            if (_isShooting)
                Shoot();
        }

        private void OnInputMoveDirectionChanged(Vector3 direction) {
            _movementController.MoveAndRotate(direction, _speedMultiplier);
        }
        private void OnWeaponSelected(int weaponId) {
            if(!_isAbleToShoot)
                return;
            _weaponController.SelectWeapon(weaponId);
        }

        private void OnFireButtonPressed() {
            _isShooting = true;
        }

        private void OnFireButtonReleased() {
            _isShooting = false;
            _weaponController.FireButtonReleased();
        }
        
        private void OnGrenadeButtonPressed() {
            _isGrenadeEquipped = true;
            _isAbleToShoot = false;
            _weaponController.HolsterWeapon();
            _grenadeController.EquippGrenade();
        }

        private void OnGrenadeButtonReleased(float force) {
            _isGrenadeEquipped = false;
            _isAbleToShoot = true;
            _weaponController.EquippWeapon();
            _grenadeController.ThrowGrenade(force);
        }

        private void OnGrenadeForceChanged(float force) {
            _grenadeController.VisualizeAim(force);
        }

        private void OnUnitDied() {
            Destroy(gameObject);
        }

        private void Shoot() {
            if(!_isAbleToShoot)
                return;
            var aimSightPosition = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            _weaponController.ShootWeapon(aimSightPosition);
        }
    }
}