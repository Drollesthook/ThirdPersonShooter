using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project {

    public class Weapon : MonoBehaviour {

        [SerializeField] Transform _firePoint = default;
        [SerializeField] Transform _aimSight = default;
        [SerializeField] Transform _weaponHolder = default;
        [SerializeField] Bullet _bulletPrefab = default;
        [SerializeField] CFGWeaponParameters _currentWeaponParameters;
        
        Camera _mainCamera;
        bool _isReloading, _isOnDelay;
        float _spreadAngle = default;
        float _shootMaxDistance = default;
        int _weaponId, _shooterId;
        float _weaponDamage;
        float _reloadTime;
        float _fireRate;
        int _amountOfBulletsInClip, _amountOfProjectilesPerShoot;
        CFGWeaponParameters.TypeOfFire _typeOfFire;

        void Awake() {
            _mainCamera = Camera.main;
            GetCurrentWeaponsParameters();
        }

        void Update() {
            WeaponHolderRotation();
        }

        public void Shoot(int shooterId, Vector3 shootDirection) {
            if(_isReloading || _isOnDelay) 
                return;
            _shooterId = shooterId;
            if(_shooterId == 0)
                PlayerShooting(shootDirection);
            else {
                BotShooting(shootDirection);
            }
        }

        void PlayerShooting(Vector3 shootDirection) {
            Ray ray = _mainCamera.ScreenPointToRay(shootDirection);
            RaycastHit hit;
            print(ray.direction);
            Vector3 newRay = ApplySpreadToDirection(ray.direction);
            print(newRay);
            Bullet bullet = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);
            if (Physics.Raycast(ray.origin, newRay, out hit, _shootMaxDistance)) {
                bullet.SetFlyDirection(hit.point);
                IHittable hittable = hit.collider.GetComponent<IHittable>();
                Debug.DrawRay(ray.origin, newRay * _shootMaxDistance, Color.red);
                if(hittable != null)
                    hittable.OnHit(_shooterId, _weaponId, _weaponDamage);
            }
            else 
                bullet.SetFlyDirection(ray.direction * _shootMaxDistance + ray.origin);
        }

        void BotShooting(Vector3 shootDirection) {
            
        }

        Vector3 ApplySpreadToDirection(Vector3 shootDirection) {
            return Quaternion.Euler(Random.Range(-_spreadAngle, _spreadAngle), Random.Range(-_spreadAngle, _spreadAngle), 0) * shootDirection;
        }

        void SetCurrentWeapon() {
            GetCurrentWeaponsParameters();
        }

        void GetCurrentWeaponsParameters() {
            _weaponId = _currentWeaponParameters.identifier;
            _shootMaxDistance = _currentWeaponParameters.shootMaxDistance;
            _spreadAngle = _currentWeaponParameters.spreadAngle;
            _weaponDamage = _currentWeaponParameters.damage;
            _typeOfFire = _currentWeaponParameters.typeOfFire;
            _fireRate = _currentWeaponParameters.fireRate;
            _reloadTime = _currentWeaponParameters.reloadTime;
            _amountOfBulletsInClip = _currentWeaponParameters.amountOfBulletsInClip;
            _amountOfProjectilesPerShoot = _currentWeaponParameters.amountOfProjectilesPerShot;
        }

        void WeaponHolderRotation() {
            float targetAngle = _mainCamera.transform.eulerAngles.x;
            _weaponHolder.localRotation = Quaternion.Euler(targetAngle,0,0);
        }

        void Reload() {
            
        }
    }
    
    
}

