using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project {
    public class Bullet : MonoBehaviour {
        [SerializeField] float _bulletSpeed = 20;
        [SerializeField] float _timeToLive = 3f;
        
        Vector3 _flyDirection;
        float _deathDistance;


        void Update() {
            Fly();
            CheckForDestroy();
        }
        
        public void SetFlyDirection(Vector3 direction) {
            _flyDirection = direction - transform.position;
            _deathDistance = Vector3.Distance(_flyDirection, transform.position);
        }

        void Fly() {
            transform.Translate(_flyDirection * _bulletSpeed);
        }

        void CheckForDestroy() {
            _timeToLive -= Time.deltaTime;
            if(_timeToLive <=0) 
                Destroy(gameObject);
        }
    }
}
