using UnityEngine;

namespace Project {
    public class WeaponController : MonoBehaviour {
        
        Weapon _weapon;
        void Awake() {
            _weapon = GetComponentInChildren<Weapon>();
        }

        public void ShootWeapon() {
           _weapon.Shoot();
        }
    }
}

