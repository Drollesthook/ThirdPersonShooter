using System;
using UnityEngine;

namespace Project.Managers {
    public class GameManager : MonoBehaviour {
        private static GameManager _instance;
        public event Action gamePlayStarted;
        public event Action playerDied;
        public event Action playerRespawned;

        public static GameManager instance => _instance;

        private void Awake() {
            _instance = this;
        }

        public void PlayerDeath() {
            playerDied?.Invoke();
        }

        public void StartGamePlay() {
            gamePlayStarted?.Invoke();
        }

        public void RespawnPlayer() {
            playerRespawned?.Invoke();
        }
    }
}
