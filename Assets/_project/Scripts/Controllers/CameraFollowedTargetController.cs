﻿using Project.Managers;

using UnityEngine;

namespace Project.Controllers {
    public class CameraFollowedTargetController : MonoBehaviour {
        [SerializeField] private Transform _followTarget = default;

        public Transform followTarget => _followTarget;

        private void Start() {
            InputManager.instance.inputCameraDirectionChanged += OnCameraInputChanged;
        }

        private void OnDestroy() {
            InputManager.instance.inputCameraDirectionChanged -= OnCameraInputChanged;
        }

        private void OnCameraInputChanged(Vector3 direction) {
            //_followTarget.Rotate(direction);
            Vector3 verticalDirection = direction;
            Vector3 horizontalDirection = direction;
            verticalDirection.y = 0;
            verticalDirection.z = 0;
            horizontalDirection.x = 0;
            horizontalDirection.z = 0;
            
            _followTarget.Rotate(verticalDirection);
            transform.Rotate(horizontalDirection);
            
            // _followTarget.rotation = Quaternion.AngleAxis(direction.x, Vector3.up);
            // _followTarget.rotation = Quaternion.AngleAxis(direction.y, Vector3.right);
            //
            Vector3 angles = followTarget.localRotation.eulerAngles;
            angles.z = 0;
            angles.y = 0;
            float angle = angles.x;
            angle = angle > 180 ? Mathf.Clamp(angles.x, 320, 360) : Mathf.Clamp(angles.x, 0, 40);
            angles.x = angle;
            _followTarget.localRotation = Quaternion.Euler(angles);
            // if (angle > 180 && angle < 340)
            // {
            //     angles.x = 340;
            // }
            // else if(angle < 180 && angle > 40)
            // {
            //     angles.x = 40;
            // }
            //_followTarget.localEulerAngles = angles;
        }
}
}