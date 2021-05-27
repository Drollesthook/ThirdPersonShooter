using System.Collections;
using System.Collections.Generic;

using Project.Managers;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Project.Controllers {
    public class LocalizationController : MonoBehaviour {
        [SerializeField] private string _localizationKey;
        private string _localizationText;
        private TMP_Text _text;

        private void Awake() {
            _text = GetComponent<TMP_Text>();
        }

        private void Start() {
            LocalizationManager.instance.localizationLoaded += OnLocalizationLoaded;
            GetLocalizationText();
            SetLocalizationText();
        }

        private void OnDestroy() {
            LocalizationManager.instance.localizationLoaded -= OnLocalizationLoaded;
        }

        private void GetLocalizationText() {
            _localizationText = LocalizationManager.instance.GetLocalizationTextByKey(_localizationKey);
        }

        private void SetLocalizationText() {
            _text.text = _localizationText;
        }

        private void OnLocalizationLoaded() {
            GetLocalizationText();
            SetLocalizationText();
        }
    }
}
