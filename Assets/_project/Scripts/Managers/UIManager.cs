using UnityEngine;

namespace Project.Managers {
    public class UIManager : MonoBehaviour {
        [SerializeField] private GameObject _gamePlayUI = default;
        [SerializeField] private GameObject _mainMenuUI = default;
        [SerializeField] private GameObject _deathScreenUI = default;

        private void Start() {
            _deathScreenUI.SetActive(false);
            _gamePlayUI.SetActive(false);
            _mainMenuUI.SetActive(true);
            GameManager.instance.gamePlayStarted += OnGamePlayStarted;
            GameManager.instance.playerDied += OnPlayerDied;
            GameManager.instance.playerRespawned += OnPlayerRespawned;
        }

        private void OnPlayerDied() {
            _deathScreenUI.SetActive(true);
            _gamePlayUI.SetActive(false);
            _mainMenuUI.SetActive(false);
        }

        private void OnGamePlayStarted() {
            _deathScreenUI.SetActive(false);
            _gamePlayUI.SetActive(true);
            _mainMenuUI.SetActive(false);
        }

        private void OnPlayerRespawned() {
            _deathScreenUI.SetActive(false);
            _gamePlayUI.SetActive(true);
            _mainMenuUI.SetActive(false);
        }
    }
}
