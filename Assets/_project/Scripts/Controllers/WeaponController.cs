using System.Collections.Generic;
using Project.Units;
using Project.Misc;
using UnityEngine;

namespace Project.Controllers {
    public class WeaponController : MonoBehaviour {
        [SerializeField] private Transform _weaponHolder;
        [SerializeField] private List<Weapon> _weapons = new List<Weapon>();
        
        private Unit _unit;
        private Weapon _weapon;

        private void Awake() {
            _weapon = GetComponentInChildren<Weapon>();
            _unit = GetComponent<Unit>();
            if(_unit.unitIdentifier != 0 )
                SelectRandomWeapon();
        }

        public void SelectWeapon(int weaponId) {
            if (_weapon != null) {
                Destroy(_weapon.gameObject);
            }

            if (weaponId < 0 || weaponId >= _weapons.Count) {
                print("There is no " + weaponId + 1 + " weapon");
                return;
            }
            _weapon = Instantiate(_weapons[weaponId], _weaponHolder);
        }


        public void ShootWeapon(Vector3 shootDirection) {
            if(_weapon == null)
                return;
            _weapon.Shoot(shootDirection);
        }

        public void FireButtonReleased() {
            if(_weapon == null)
                return;
            _weapon.FireButtonReleased();
        }
        
        private void SelectRandomWeapon() {
            if (_weapon != null) {
                Destroy(_weapon.gameObject);
            }

            int weaponId = Random.Range(0, _weapons.Count - 1);
            _weapon = Instantiate(_weapons[weaponId], _weaponHolder);
        }
    }
}