using System.Collections;

using Project.Controllers;
using Project.Managers;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Units {
    public class BotAI : MonoBehaviour {
        [SerializeField] private float _shootDistance = 5f;
        [SerializeField] private float _shootDelay = 2f;
        
        private Transform _myTarget;
        private WeaponController _weaponController;
        private NavMeshAgent _navMeshAgent;
        private Unit _unit;
        private bool _isFollowingTarget;
        private bool _isInCombat;
        private bool _isAwaiting;

        private void Awake() {
            _weaponController = GetComponent<WeaponController>();
            _unit = GetComponent<Unit>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Start() {
            GetTargetAndMove();
            UnitsHolderManager.instance.unitSpawned += OnUnitSpawned;
        }

        private void OnDestroy() {
            UnitsHolderManager.instance.unitSpawned -= OnUnitSpawned;
        }

        private void Update() {
            if(_isFollowingTarget && !_isInCombat && !_isAwaiting && IsDistanceReached())
                OnDistanceReached();
            if(_isInCombat)
                LookAtTarget();
        }

        private void GetTargetAndMove() {
            GetRandomTargetFromUnitHolder();
            MoveToTarget();
        }

        private void GetRandomTargetFromUnitHolder() {
            _myTarget = UnitsHolderManager.instance.GetHostileUnitsTransformsByUnitId(_unit.fractionIdentifier);
        }


        private void MoveToTarget() {
            if (_myTarget == null) {
                _isAwaiting = true;
                return;
            }

            _navMeshAgent.SetDestination(_myTarget.position);
            _isFollowingTarget = true;
        }

        private void LookAtTarget() {
            transform.LookAt(_myTarget);
        }

        private bool IsDistanceReached() {
            return _navMeshAgent.remainingDistance != 0 && _navMeshAgent.remainingDistance < _shootDistance;
        }

        private void OnDistanceReached() {
            _isFollowingTarget = false;
            if (IsTargetAlive()) {
                if (IsTargetNearMe())
                    StartCoroutine(ShootWithDelay());
                else
                    MoveToTarget();
            }
            else 
                GetTargetAndMove();
            
        }

        private bool IsTargetAlive() {
            return _myTarget != null;
        }

        private bool IsTargetNearMe() {
            return Vector3.Distance(transform.position, _myTarget.position) <= _shootDistance;
        }

        private void Shoot() {
            _weaponController.ShootWeapon(_myTarget.position);
            _weaponController.FireButtonReleased();
        }

        void OnUnitSpawned(int fractionId) {
            if(!_isAwaiting) 
                return;

            if (fractionId == _unit.fractionIdentifier) 
                return;
            _isAwaiting = false;
            GetTargetAndMove();

        }

        IEnumerator ShootWithDelay() {
            _isInCombat = true;
            while (_isInCombat) {
                yield return new WaitForSeconds(_shootDelay);
                if (IsTargetAlive() && IsTargetNearMe()) {
                    Shoot();
                    continue;
                }

                _isInCombat = false;
                GetTargetAndMove();
            }
        } 
    }
}
