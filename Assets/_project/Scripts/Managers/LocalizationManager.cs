using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

namespace Project.Managers {
    public class LocalizationManager : MonoBehaviour {
        private static LocalizationManager _instance;
        public event Action localizationLoaded;
        
        [SerializeField] private LocalizationData _currentLocalizationData;

        public static LocalizationManager instance => _instance;
        
        private Dictionary<string,string> _currentLocalizationDictionary = new Dictionary<string, string>();
        private const string CURRENT_LANGUAGE = "current_language";
        private int _currentLanguageID;

        [Serializable]
        private class Localization {
            public string weapon_0;
            public string weapon_1;
            public string weapon_2;
            public string weapon_3;
            public string menu_play;
            public string menu_respawn;
            public string menu_settings;
            public string menu_settings_language;
            public string killinfo_killed;
            public string killinfo_using;
        }

        private class LocalizationData {
            public Localization en_localization;
            public Localization ru_localization;
        }

        private void Awake() {
            _instance = this;
            _currentLanguageID = PlayerPrefs.GetInt(CURRENT_LANGUAGE, 1);
        }

        private void Start() {
            string json = File.ReadAllText(Application.dataPath + "/_project/Localization/LocalizationFile.json");
            _currentLocalizationData = JsonUtility.FromJson<LocalizationData>(json);
            ChooseLocalization(_currentLanguageID);
        }

        public void ChooseLocalization(int language_id) {
            _currentLanguageID = language_id;
            switch (_currentLanguageID) {
                case 1:
                    LoadCurrentLocalizationInDictionary(_currentLocalizationData.en_localization);
                    break;
                case 2:
                    LoadCurrentLocalizationInDictionary(_currentLocalizationData.ru_localization);
                    break;
                default:
                    LoadCurrentLocalizationInDictionary(_currentLocalizationData.en_localization);
                    Debug.LogWarning("Required Language doesn't exist in this game");
                    break;
            }
            PlayerPrefs.SetInt(CURRENT_LANGUAGE, _currentLanguageID);
        }

        public string GetLocalizationTextByKey(string key) {
            return _currentLocalizationDictionary[key];
        }

        private void LoadCurrentLocalizationInDictionary(Localization localization) {
            _currentLocalizationDictionary.Clear();
            _currentLocalizationDictionary.Add("weapon_0", localization.weapon_0);
            _currentLocalizationDictionary.Add("weapon_1", localization.weapon_1);
            _currentLocalizationDictionary.Add("weapon_2", localization.weapon_2);
            _currentLocalizationDictionary.Add("weapon_3", localization.weapon_3);
            _currentLocalizationDictionary.Add("menu_play", localization.menu_play);
            _currentLocalizationDictionary.Add("menu_respawn", localization.menu_respawn);
            _currentLocalizationDictionary.Add("menu_settings", localization.menu_settings);
            _currentLocalizationDictionary.Add("menu_settings_language", localization.menu_settings_language);
            _currentLocalizationDictionary.Add("killinfo_killed", localization.killinfo_killed);
            _currentLocalizationDictionary.Add("killinfo_using", localization.killinfo_using);
            localizationLoaded?.Invoke();
        }
    }
}
