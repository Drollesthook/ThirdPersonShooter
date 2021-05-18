using System;

using UnityEngine;

namespace Project {
    public class Unit : MonoBehaviour, IHittable {
        public event Action unitDied;
        
        [SerializeField] private int _identifier;
        [SerializeField] private float _maxHP = 20;

        private float _currentHPAmount;
        private int _lastShootersId;
        private int _lastWeaponsId;
        private bool _isDead;
        
        public int identifier => _identifier;

        private void Awake() {
            _currentHPAmount = _maxHP;
        }

        public void OnHit(int shooterId, int weaponId, float damage) {
            if (_isDead) 
                return;
            _lastShootersId = shooterId;
            _lastWeaponsId = weaponId;
            ImplementDamage(damage);
        }

        private void ImplementDamage(float damage) {
            _currentHPAmount -= damage;
            CheckHPAmount();
        }

        private void CheckHPAmount() {
            if(_currentHPAmount <= 0)
                Death();
        }

        private void Death() {
            _isDead = true;
            unitDied?.Invoke();
            print(_lastShootersId + " killed " + _identifier + " using " + _lastWeaponsId);
        }
    }
}