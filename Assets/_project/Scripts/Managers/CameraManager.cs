using Cinemachine;

using Project.Controllers;
using Project.Units;

using UnityEngine;

namespace Project.Managers {
    public class CameraManager : MonoBehaviour {
        [SerializeField] private CinemachineVirtualCamera _camera = default;
        
        private void Start() {
            SpawnManager.instance.PlayerSpawned += OnPlayerSpawned;
        }

        private void OnDestroy() {
            SpawnManager.instance.PlayerSpawned -= OnPlayerSpawned;
        }

        void OnPlayerSpawned(Unit playerUnit) {
            CameraFollowedTargetController followedTargetController = playerUnit.GetComponent<CameraFollowedTargetController>();
            _camera.LookAt = followedTargetController.followTarget;
            _camera.Follow = followedTargetController.followTarget;
        }
    }
}
