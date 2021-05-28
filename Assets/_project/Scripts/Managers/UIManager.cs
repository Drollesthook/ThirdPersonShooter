using System.Collections.Generic;

using DG.Tweening;

using Project.Controllers;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Project.Managers {
    public class UIManager : MonoBehaviour {
        private static UIManager _instance;
        
        [SerializeField] private GameObject _gamePlayUI = default;
        [SerializeField] private GameObject _mainMenuUI = default;
        [SerializeField] private GameObject _deathScreenUI = default;
        [SerializeField] private GameObject _SettingsMenuUI = default;
        [SerializeField] private GameObject _killEventsMenuUI = default;
        [SerializeField] List<Image> _weaponsIconsList = new List<Image>();
        [SerializeField] private KillInfoController _killInfoPrefab = default;

        public static UIManager instance => _instance;

        private int _playersFractionId;

        private void Awake() {
            _instance = this;
        }

        private void Start() {
            _deathScreenUI.SetActive(false);
            _gamePlayUI.SetActive(false);
            _mainMenuUI.SetActive(true);
            _SettingsMenuUI.SetActive(false);
            GameManager.instance.gamePlayStarted += OnGamePlayStarted;
            GameManager.instance.playerDied += OnPlayerDied;
            GameManager.instance.playerRespawned += OnPlayerRespawned;
            InputManager.instance.weaponSelected += OnWeaponSelected;
            _playersFractionId = UnitsHolderManager.instance.playerFractionId;
        }

        public void UnitDied(int killersId, int weaponsId, int deadGuyId, int fractionId) {
            KillInfoController newKillInfo = Instantiate(_killInfoPrefab, _killEventsMenuUI.transform);
            string killer = IdsInfoManager.instance.GetUnitsNameById(killersId);
            string weapon = IdsInfoManager.instance.GetWeaponNameById(weaponsId);
            string deadGuy = IdsInfoManager.instance.GetUnitsNameById(deadGuyId);
            newKillInfo.SetText(killer,weapon,deadGuy, fractionId == _playersFractionId);
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
            _killEventsMenuUI.SetActive(true);
            UnfadeWeaponIcons();
        }

        private void OnPlayerRespawned() {
            _deathScreenUI.SetActive(false);
            _gamePlayUI.SetActive(true);
            _mainMenuUI.SetActive(false);
            UnfadeWeaponIcons();
        }

        private void OnWeaponSelected(int weaponId) {
            for (int i = 0; i < _weaponsIconsList.Count; i++) {
                if(i == weaponId)
                    continue;
                FadeWeaponIcon(_weaponsIconsList[i]);
            }
        }

        private void FadeWeaponIcon(Image image) {
            image.DOFade(0, 1);
        }

        private void UnfadeWeaponIcons() {
            foreach (Image image in _weaponsIconsList) {
                image.DOFade(1, 0);
            }
        }
    }
}
