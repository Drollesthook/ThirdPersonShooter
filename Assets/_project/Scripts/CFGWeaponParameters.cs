using UnityEngine;

namespace Project {


    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapon")]
    public class CFGWeaponParameters : ScriptableObject {

        [SerializeField] TypeOfFire _typeOfFire = default;
        [SerializeField] int _identifier = default;
        [SerializeField] float _fireRate = default;
        [SerializeField] float _reloadTime = default;
        [SerializeField] float _damage = default;
        [SerializeField] int _amountOfProjectilesPerShot = default;
        [SerializeField] int _amountOfBulletsInClip = default;
        [SerializeField] float _spreadAngle = default;
        [SerializeField] float _shootMaxDistance = default;

        public enum TypeOfFire {
            SingleShot,
            Automatic
        }

        public TypeOfFire typeOfFire => _typeOfFire;

        public int identifier => _identifier;

        public float reloadTime => _reloadTime;

        public float fireRate => _fireRate;

        public float damage => _damage;

        public int amountOfProjectilesPerShot => _amountOfProjectilesPerShot;

        public int amountOfBulletsInClip => _amountOfBulletsInClip;

        public float spreadAngle => _spreadAngle;

        public float shootMaxDistance => _shootMaxDistance;
    }
}
