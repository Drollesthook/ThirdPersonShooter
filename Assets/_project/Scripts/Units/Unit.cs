using System;
using UnityEngine;
using Project.Interfaces;
using Project.Managers;
using Project.Misc;

namespace Project.Units {
    public class Unit : MonoBehaviour, IHittable {
        public event Action unitDied;
        public event Action<float> hPAmountChanged;

        [SerializeField] private int _fractionIdentifier = default;
        [SerializeField] private int _unitIdentifier = default;
        [SerializeField] private float _maxHP = 20;
        
        public int unitIdentifier => _unitIdentifier;
        public int fractionIdentifier => _fractionIdentifier;

        private float _currentHPAmount;
        private int _lastShootersId;
        private int _lastWeaponsId;
        private bool _isDead;
        private SpawnPoint _mySpawnPoint;
        

        private void Awake() {
            _currentHPAmount = _maxHP;
        }

        public void OnHit(int shooterId, int shooterFractionId, int weaponId, float damage) {
            if (shooterFractionId == _fractionIdentifier) {
                Debug.LogWarning("WATCH YOUR FIRE! >_<");
                return;
            }
            if (_isDead) 
                return;
            _lastShootersId = shooterId;
            _lastWeaponsId = weaponId;
            ImplementDamage(damage);
        }
        
        public void SetSpawnPoint(SpawnPoint spawnPoint) {
            _mySpawnPoint = spawnPoint;
        }

        private void ImplementDamage(float damage) {
            _currentHPAmount -= damage;
            hPAmountChanged?.Invoke(_currentHPAmount/_maxHP);
            CheckHPAmount();
        }

        private void CheckHPAmount() {
            if(_currentHPAmount <= 0)
                Death();
        }

        private void Death() {
            _isDead = true;
            _mySpawnPoint.UnitDied();
            UnitsHolderManager.instance.UnitDied(this);
            Debug.Log(_lastShootersId + " killed " + _unitIdentifier + " using " + _lastWeaponsId);
            unitDied?.Invoke();
        }
    }
}