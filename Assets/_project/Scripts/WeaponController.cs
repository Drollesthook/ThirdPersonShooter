using UnityEngine;

namespace Project {
    public class WeaponController : MonoBehaviour {
        
        Weapon _weapon;
        void Awake() {
            _weapon = GetComponentInChildren<Weapon>();
        }

        void Update() {
            if (Input.GetMouseButton(0)) {
                ShootWeapon();
            }
        }

        void ShootWeapon() {
            
           _weapon.Shoot();
        }
    }
}

