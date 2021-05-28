using System.Collections;

using Project.Managers;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Project.Controllers {
    public class KillInfoController : MonoBehaviour {
        [SerializeField] private string _killedKey = default;
        [SerializeField] private string _usingKey = default;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Color _BGHostileKillColor;
        [SerializeField] private Color _BGAllyKillColor;
        
        private string _killText;
        private string _usingText;

        private void Awake() {
            OnLocalizationLoaded();
        }

        private void Start() {
            StartCoroutine(DestroyWithDelay());
        }

        public void SetText(string killer, string weapon, string deadguy, bool _isKillerHostile) {
            _text.text = killer + " " + _killText + " " + deadguy + " " + _usingText + " " + weapon;
            _backgroundImage.color = _isKillerHostile ? _BGHostileKillColor : _BGAllyKillColor;
        }

        private void OnLocalizationLoaded() {
            _killText = LocalizationManager.instance.GetLocalizationTextByKey(_killedKey);
            _usingText = LocalizationManager.instance.GetLocalizationTextByKey(_usingKey);
        }

        IEnumerator DestroyWithDelay() {
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
        }
    }
}
