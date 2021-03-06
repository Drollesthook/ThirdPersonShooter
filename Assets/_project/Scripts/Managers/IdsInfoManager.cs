using System.Collections.Generic;

using UnityEngine;

namespace Project.Managers {
    public class IdsInfoManager : MonoBehaviour {
        private static IdsInfoManager _instance;

        [SerializeField] private List<string> _listOfUnits = new List<string>();
        [SerializeField] private List<string> _listOfWeaponsKeys = new List<string>();
        [SerializeField] private List<string> _listOfFractions = new List<string>();
        
        public static IdsInfoManager instance => _instance;
        private List<string> _listOfWeapons = new List<string>();

        private void Awake() {
            _instance = this;
        }

        private void Start() {
            OnLocalizationLoaded();
            LocalizationManager.instance.localizationLoaded += OnLocalizationLoaded;
        }

        private void OnDestroy() {
            LocalizationManager.instance.localizationLoaded -= OnLocalizationLoaded;
        }

        public string GetUnitsNameById(int id) {
            return id >= _listOfUnits.Count ? "NoName" : _listOfUnits[id];
        }
        
        public string GetWeaponNameById(int id) {
            return id >= _listOfWeapons.Count ? "NoName" : _listOfWeapons[id];
        }
        
        public string GetFractionNameById(int id) {
            return id >= _listOfFractions.Count ? "NoName" : _listOfFractions[id];
        }

        private void OnLocalizationLoaded() {
            SetWeaponsNames();
        }
        private void SetWeaponsNames() {
            _listOfWeapons.Clear();
            foreach (string t in _listOfWeaponsKeys) {
                _listOfWeapons.Add(LocalizationManager.instance.GetLocalizationTextByKey(t));
            }
        }
    }
}
