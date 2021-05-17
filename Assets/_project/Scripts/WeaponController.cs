using UnityEngine;

namespace Project {
    public class WeaponController : MonoBehaviour {
        [SerializeField] Transform _weaponHolder = default;

        Camera _mainCamera;
        Weapon _weapon;
        Unit _unit;
        void Awake() {
            _mainCamera = Camera.main;
            _weapon = GetComponentInChildren<Weapon>();
            _unit = GetComponent<Unit>();
        }

        void Update() {
            WeaponHolderRotation();
        }

        public void ShootWeapon(Vector3 shootDirection) {
           _weapon.Shoot(_unit.identifier, shootDirection);
        }
        
        void WeaponHolderRotation() {
            float targetAngle = _mainCamera.transform.eulerAngles.x;
            _weaponHolder.localRotation = Quaternion.Euler(targetAngle,0,0);
        }
    }
}

