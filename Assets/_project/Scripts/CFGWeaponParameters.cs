using UnityEngine;

namespace Project {
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapon")]
    public class CFGWeaponParameters : ScriptableObject {

        [SerializeField] private int _amountOfBulletsInClip;
        [SerializeField] private int _amountOfProjectilesPerShot;
        [SerializeField] private float _damage;
        [SerializeField] private int _identifier;
        [SerializeField] private float _reloadTime;
        [SerializeField] private float _shootDelay;
        [SerializeField] private float _shootMaxDistance;
        [SerializeField] private float _spreadAngle;
        [SerializeField] private TypeOfFire _typeOfFire;
        
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