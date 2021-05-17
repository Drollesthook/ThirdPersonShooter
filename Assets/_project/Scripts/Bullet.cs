using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project {
    public class Bullet : MonoBehaviour {
        [SerializeField] float _bulletSpeed = 20;
        [SerializeField] float _timeToLive = 3f;
        


        void Update() {
            Fly();
            CheckForDestroy();
        }
        
        public void SetFlyDirection(Vector3 direction) {
            transform.LookAt(direction);
        }

        void Fly() {
            transform.Translate(Vector3.forward * _bulletSpeed);
        }

        void CheckForDestroy() {
            _timeToLive -= Time.deltaTime;
            if(_timeToLive <=0) 
                Destroy(gameObject);
        }
    }
}
