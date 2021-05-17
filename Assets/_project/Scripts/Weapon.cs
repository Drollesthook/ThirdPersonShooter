using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project {

    public class Weapon : MonoBehaviour {

        [SerializeField] Transform _firePoint = default;
        [SerializeField] Transform _aimSight = default;
        [SerializeField] Transform _weaponHolder = default;
        [SerializeField] GameObject _bulletPrefab = default;
        [SerializeField] float _spreadAngle = default;
        [SerializeField] List<CFGWeaponParameters> _weapons= new List<CFGWeaponParameters>();


        CFGWeaponParameters _currentWeapon;
        Camera _mainCamera;
        bool _isReloading, _isOnDelay;

        void Awake() {
            _mainCamera = Camera.main;
        }

        void Update() {
            WeaponHolderRotation();
        }

        public void Shoot() {
            if(_isReloading || _isOnDelay) 
                return;
            Ray ray = _mainCamera.ScreenPointToRay(_aimSight.position);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            Vector3 direction = hit.point;
            
            Vector3 rightDirection = Quaternion.Euler(0, _spreadAngle, 0) * (direction - _firePoint.position);
            Debug.DrawRay(_firePoint.position, rightDirection);
            Vector3 leftDirection = Quaternion.Euler(0, -_spreadAngle, 0) * (direction - _firePoint.position);
            Debug.DrawRay(_firePoint.position, leftDirection);
            Vector3 upDirection = Quaternion.Euler(_spreadAngle, 0, 0) * (direction - _firePoint.position);
            Debug.DrawRay(_firePoint.position, upDirection);
            Vector3 downDirection = Quaternion.Euler(-_spreadAngle, 0, 0) * (direction - _firePoint.position);
            Debug.DrawRay(_firePoint.position, downDirection);
        }

        void SetCurrentWeapon() {
            _currentWeapon = _weapons[Random.Range(0, _weapons.Count - 1)];
        }

        void WeaponHolderRotation() {
            Ray ray = _mainCamera.ScreenPointToRay(_aimSight.position);
            float targetAngle = _mainCamera.transform.eulerAngles.x;
            _weaponHolder.localRotation = Quaternion.Euler(targetAngle,0,0);
        }

        void Reload() {
            
        }

        void OnDrawGizmos() {
            Vector3 rightDirection = Quaternion.Euler(0, _spreadAngle, 0) * transform.forward;
            Gizmos.DrawRay(_firePoint.position, rightDirection);
            Vector3 leftDirection = Quaternion.Euler(0, -_spreadAngle, 0) * transform.forward;
            Gizmos.DrawRay(_firePoint.position, leftDirection);
            Vector3 upDirection = Quaternion.Euler(_spreadAngle, 0, 0) * transform.forward;
            Gizmos.DrawRay(_firePoint.position, upDirection);
            Vector3 downDirection = Quaternion.Euler(-_spreadAngle, 0, 0) * transform.forward;
            Gizmos.DrawRay(_firePoint.position, downDirection);
        }
    }
    
    
}

