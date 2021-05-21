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
            if(_isFollowingTarget && !_isInCombat && !_isAwaiting)
                CheckForDistanceReached();
            if(_isInCombat)
                LookAtTarget();
        }

        private void GetTargetAndMove() {
            GetRandomTargetFromHolder();
            MoveToTarget();
        }

        private void GetRandomTargetFromHolder() {
            _myTarget = UnitsHolderManager.instance.GetHostileUnitsTransformsByUnitId(_unit.fractionIdentifier);
        }


        private void MoveToTarget() {
            if (_myTarget == null) {
                _isAwaiting = true;
                return;
            }

            _navMeshAgent.SetDestination(_myTarget.position);
            _isFollowingTarget = true;
            //_navMeshAgent.isStopped = true;
        }

        private void LookAtTarget() {
            transform.LookAt(_myTarget);
        }

        private void SearchForTarget() {
        }

        private void CheckForDistanceReached() {
            if (_navMeshAgent.remainingDistance == Mathf.Infinity && _navMeshAgent.remainingDistance > _shootDistance)
                return;

            _isFollowingTarget = false;
            Debug.Log("I AM HERE");
            if (_isTargetAlive()) {
                if (_isTargetNearMe())
                    StartCoroutine(ShootWithDelay());
                else
                    MoveToTarget();
            }
            else 
                GetTargetAndMove();
            
        }

        private bool _isTargetAlive() {
            return _myTarget != null;
        }

        private bool _isTargetNearMe() {
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
                Shoot();
                yield return new WaitForSeconds(_shootDelay);
                if (_isTargetAlive() && _isTargetNearMe()) 
                    continue;
                _isInCombat = false;
                GetTargetAndMove();
            }
        }
            // добавить missOffset, чтобы не стрелял чётко в цель,также в зависимости от оружия время нажатия на спуск после которого отжатие
        //joggle
            //двигаться влево вправо разными интервалами (и скоростью) имитировать попытку увернутсья от пуль врага
            
            
        
    }
}
