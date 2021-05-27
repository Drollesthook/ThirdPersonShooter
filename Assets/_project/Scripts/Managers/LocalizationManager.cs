using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

namespace Project.Managers {
    public class LocalizationManager : MonoBehaviour {
        public event Action localizationLoaded; 
        
        private static LocalizationManager _instance;
        
        [SerializeField] private LocalizationData _currentLocalizationData;

        public static LocalizationManager instance => _instance;

        private Localization _current_en_localization;
        private Localization _current_ru_localization;
        private Dictionary<string,string> _currentLocalizationDictionary = new Dictionary<string, string>();

        [Serializable]
        private class Localization {
            public string weapon_0;
            public string weapon_1;
            public string weapon_2;
            public string menu_play;
            public string menu_respawn;
        }

        private class LocalizationData {
            public Localization en_localization;
            public Localization ru_localization;
        }

        private void Awake() {
            _instance = this;
        }

        private void Start() {
            string json = File.ReadAllText(Application.dataPath + "/_project/Localization/LocalizationFile.json");
            _currentLocalizationData = JsonUtility.FromJson<LocalizationData>(json);
            LoadCurrentLocalizationInDictionary(_currentLocalizationData.ru_localization);
        }

        public string GetLocalizationTextByKey(string key) {
            return _currentLocalizationDictionary[key];
        }

        private void LoadCurrentLocalizationInDictionary(Localization localization) {
            _currentLocalizationDictionary.Add("weapon_0", localization.weapon_0);
            _currentLocalizationDictionary.Add("weapon_1", localization.weapon_1);
            _currentLocalizationDictionary.Add("weapon_2", localization.weapon_2);
            _currentLocalizationDictionary.Add("menu_play", localization.menu_play);
            _currentLocalizationDictionary.Add("menu_respawn", localization.menu_respawn);
            localizationLoaded?.Invoke();
        }
    }
}
