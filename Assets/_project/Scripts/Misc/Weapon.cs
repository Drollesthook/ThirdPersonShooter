using UnityEngine;
using Project.Interfaces;
using Project.ScriptableObjects;
using Project.Units;

namespace Project.Misc {
    public class Weapon : MonoBehaviour {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private CFGWeaponParameters _currentWeaponParameters;
        [SerializeField] private Transform _firePoint;
        
        private float _bulletsInClip;
        private bool _isOnShootDelay;
        private bool _isWaitingOnRelease;
        private Camera _mainCamera;

        private int _weaponId;
        private int _shooterId;
        private int _shootersFractionId;
        private float _reloadTime;
        private float _shootCooldown;
        private float _shootDelay;
        private float _shootMaxDistance;
        private int _amountOfBulletsInClip;
        private int _amountOfProjectilesPerShoot;
        private float _spreadAngle;
        private float _weaponDamage;
        private CFGWeaponParameters.TypeOfFire _typeOfFire;
        private Unit _myUnit;
        
        private void Awake() {
            _myUnit = GetComponentInParent<Unit>();
            _shooterId = _myUnit.unitIdentifier;
            _shootersFractionId = _myUnit.fractionIdentifier;
            _mainCamera = Camera.main;
            GetCurrentWeaponsParameters();
        }

        private void Update() {
            if (!_isOnShootDelay) 
                return;
            _shootCooldown -= Time.deltaTime;
            if (_shootCooldown <= 0)
                _isOnShootDelay = false;
        }

        public void Shoot(Vector3 shootDirection) {
            if (_isOnShootDelay || _isWaitingOnRelease)
                return;
            for (var i = 1; i <= _amountOfProjectilesPerShoot; i++) {
                if (_shooterId == 0)
                    PlayerShooting(shootDirection);
                else
                    BotShooting(shootDirection);
            }

            _isOnShootDelay = true;
            _shootCooldown = _shootDelay;
            if (_typeOfFire == CFGWeaponParameters.TypeOfFire.SingleShot)
                _isWaitingOnRelease = true;

            _bulletsInClip--;
            if (_bulletsInClip <= 0)
                Reload();
        }

        public void FireButtonReleased() {
            _isWaitingOnRelease = false;
        }

        private void PlayerShooting(Vector3 shootDirection) {
            Ray ray = _mainCamera.ScreenPointToRay(shootDirection);
            RaycastHit hit;
            Vector3 newRayDirection = ApplySpreadToDirection(ray.direction);
            Bullet bullet = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);
            if (Physics.Raycast(ray.origin, newRayDirection, out hit, _shootMaxDistance)) {
                bullet.SetFlyDirection(hit.point);
                var hittable = hit.collider.GetComponent<IHittable>();
                Debug.DrawRay(ray.origin, newRayDirection * _shootMaxDistance, Color.red);
                if (hittable != null)
                    hittable.OnHit(_shooterId, _shootersFractionId, _weaponId, _weaponDamage);
            } else
                bullet.SetFlyDirection(ray.direction * _shootMaxDistance + ray.origin);
        }

        private void BotShooting(Vector3 shootDirection) {
            
        }

        private Vector3 ApplySpreadToDirection(Vector3 shootDirection) {
            return Quaternion.Euler(Random.Range(-_spreadAngle, _spreadAngle), Random.Range(-_spreadAngle, _spreadAngle), 0) *
                shootDirection;
        }

        private void GetCurrentWeaponsParameters() {
            _weaponId = _currentWeaponParameters.identifier;
            _shootMaxDistance = _currentWeaponParameters.shootMaxDistance;
            _spreadAngle = _currentWeaponParameters.spreadAngle;
            _weaponDamage = _currentWeaponParameters.damage;
            _typeOfFire = _currentWeaponParameters.typeOfFire;
            _shootDelay = _currentWeaponParameters.shootDelay;
            _reloadTime = _currentWeaponParameters.reloadTime;
            _amountOfBulletsInClip = _currentWeaponParameters.amountOfBulletsInClip;
            _amountOfProjectilesPerShoot = _currentWeaponParameters.amountOfProjectilesPerShot;

            _bulletsInClip = _amountOfBulletsInClip;
        }

        private void Reload() {
            _isOnShootDelay = true;
            _shootCooldown = _reloadTime;
            _bulletsInClip = _amountOfBulletsInClip;
        }
    }
}