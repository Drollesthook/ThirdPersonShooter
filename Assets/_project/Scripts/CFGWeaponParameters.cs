using UnityEngine;

namespace Project {
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapon")]
    public class CFGWeaponParameters : ScriptableObject {

        [SerializeField] int _amountOfBulletsInClip;
        [SerializeField] int _amountOfProjectilesPerShot;
        [SerializeField] float _damage;
        [SerializeField] int _identifier;
        [SerializeField] float _reloadTime;
        [SerializeField] float _shootDelay;
        [SerializeField] float _shootMaxDistance;
        [SerializeField] float _spreadAngle;
        [SerializeField] TypeOfFire _typeOfFire;
        
        public enum TypeOfFire {
            SingleShot,
            Automatic
        }

        public TypeOfFire typeOfFire => _typeOfFire;

        public int identifier => _identifier;

        public float reloadTime => _reloadTime;

        public float shootDelay => _shootDelay;

        public float damage => _damage;

        public int amountOfProjectilesPerShot => _amountOfProjectilesPerShot;

        public int amountOfBulletsInClip => _amountOfBulletsInClip;

        public float spreadAngle => _spreadAngle;

        public float shootMaxDistance => _shootMaxDistance;
    }
}