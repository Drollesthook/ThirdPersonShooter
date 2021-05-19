using System.Collections;
using System.Collections.Generic;

using Cinemachine;

using Project.Units;

using UnityEngine;

namespace Project.Managers {
    public class CameraManager : MonoBehaviour {
        [SerializeField] private CinemachineFreeLook _camera = default;
        
        private void Start() {
            SpawnManager.instance.PlayerSpawned += OnPlayerSpawned;
        }

        private void OnDestroy() {
            SpawnManager.instance.PlayerSpawned -= OnPlayerSpawned;
        }

        void OnPlayerSpawned(Unit player) {
            _camera.LookAt = player.transform;
            _camera.Follow = player.transform;
        }
    }
}
