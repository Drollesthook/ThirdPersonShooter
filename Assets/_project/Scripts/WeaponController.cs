using UnityEngine;

namespace Project {
    public class WeaponController : MonoBehaviour {
        [SerializeField] Transform weaponHolder = default;
        
        Weapon _weapon;
        Unit _unit;
        void Awake() {
            _weapon = GetComponentInChildren<Weapon>();
            _unit = GetComponent<Unit>();
        }

        public void ShootWeapon(Vector3 shootDirection) {
           _weapon.Shoot(_unit.identifier, shootDirection);
        }
    }
}

