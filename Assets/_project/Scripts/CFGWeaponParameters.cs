using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project {


    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapon")]
    public class CFGWeaponParameters : ScriptableObject {

        [SerializeField] TypeOfFire _typeOfFire = default;
        [SerializeField] int _identity = default;
        [SerializeField] float _fireRate = default;
        [SerializeField] float _reloadTime = default;
        [SerializeField] float _damage = default;
        [SerializeField] int _amountOfBulletsPerShot = default;

        public enum TypeOfFire {
            SingleShot,
            Automatic
        }

        public TypeOfFire typeOfFire => _typeOfFire;

        public int identity => _identity;

        public float reloadTime => _reloadTime;

        public float fireRate => _fireRate;

        public float damage => _damage;

        public int amountOfBulletsPerShot => _amountOfBulletsPerShot;
    }
}
