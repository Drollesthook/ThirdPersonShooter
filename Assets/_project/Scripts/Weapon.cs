using UnityEngine;

namespace Project {
    public class Weapon : MonoBehaviour {
        [SerializeField] Bullet _bulletPrefab;
        [SerializeField] CFGWeaponParameters _currentWeaponParameters;
        [SerializeField] Transform _firePoint;
        
        float _bulletsInClip;
        bool _isOnShootDelay;
        bool _isWaitingOnRelease;
        Camera _mainCamera;

        int _weaponId;
        int _shooterId;
        float _reloadTime;
        float _shootCooldown;
        float _shootDelay;
        float _shootMaxDistance;
        int _amountOfBulletsInClip;
        int _amountOfProjectilesPerShoot;
        float _spreadAngle;
        float _weaponDamage;
        CFGWeaponParameters.TypeOfFire _typeOfFire;

        void Awake() {
            _mainCamera = Camera.main;
            GetCurrentWeaponsParameters();
        }

        void Update() {
            if (!_isOnShootDelay) 
                return;
            _shootCooldown -= Time.deltaTime;
            if (_shootCooldown <= 0)
                _isOnShootDelay = false;
        }

        public void Shoot(int shooterId, Vector3 shootDirection) {
            if (_isOnShootDelay || _isWaitingOnRelease)
                return;

            _shooterId = shooterId;
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

        void PlayerShooting(Vector3 shootDirection) {
            Ray ray = _mainCamera.ScreenPointToRay(shootDirection);
            RaycastHit hit;
            Vector3 newRayDirection = ApplySpreadToDirection(ray.direction);
            Bullet bullet = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);
            if (Physics.Raycast(ray.origin, newRayDirection, out hit, _shootMaxDistance)) {
                bullet.SetFlyDirection(hit.point);
                var hittable = hit.collider.GetComponent<IHittable>();
                Debug.DrawRay(ray.origin, newRayDirection * _shootMaxDistance, Color.red);
                if (hittable != null)
                    hittable.OnHit(_shooterId, _weaponId, _weaponDamage);
            } else
                bullet.SetFlyDirection(ray.direction * _shootMaxDistance + ray.origin);
        }

        void BotShooting(Vector3 shootDirection) {
            
        }

        Vector3 ApplySpreadToDirection(Vector3 shootDirection) {
            return Quaternion.Euler(Random.Range(-_spreadAngle, _spreadAngle), Random.Range(-_spreadAngle, _spreadAngle), 0) *
                shootDirection;
        }

        void GetCurrentWeaponsParameters() {
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

        void Reload() {
            _isOnShootDelay = true;
            _shootCooldown = _reloadTime;
            _bulletsInClip = _amountOfBulletsInClip;
        }
    }
}