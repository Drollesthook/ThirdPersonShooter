using System.Collections;
using DG.Tweening;

using Project.Managers;
using Project.Units;

using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

namespace Project.Controllers {
public class HPBarController : MonoBehaviour {
    [SerializeField] private CanvasGroup _canvas;
    [SerializeField] private Image _hpBar = default;
    [SerializeField] private Image _secondHpBar = default;
    [SerializeField] private float _secondBarChangeDelay = 0.4f;
    [SerializeField] private Color _allyColor = default;
    [SerializeField] private Color _hostileColor = default;

    private Coroutine _hideBarWithDelay;
    private Transform _mainCamera;
    private Unit _myUnit;

    private void Awake() {
        _myUnit = GetComponent<Unit>();
        _mainCamera = Camera.main.transform;
    }

    private void Start() {
        _myUnit.hPAmountChanged += OnHpAmountChanged;
        SetHPBarColor();
    }

    private void LateUpdate()
    {
        CalculateAngle();
    }

    private void OnDestroy() {
        DOTween.Kill(_secondHpBar);
        _myUnit.hPAmountChanged -= OnHpAmountChanged;
    }

    private void SetHPBarColor() {
        _hpBar.color = _myUnit.fractionIdentifier == UnitsHolderManager.instance.playerFractionId ? _allyColor : _hostileColor;
    }

    private void OnHpAmountChanged(float hpPercent) {
        ChangeHPAmount(hpPercent);
    }

    private void ChangeHPAmount(float fillAmount) {
        _hpBar.fillAmount = fillAmount;
        StartCoroutine(ChangeSecondBarWithDelay(fillAmount));
    }
    
    private void CalculateAngle() {
        Vector3 cameraPosition = _mainCamera.position;
        Vector3 lookPos = new Vector3(cameraPosition.x, _canvas.transform.position.y, cameraPosition.z);
        _canvas.transform.LookAt(lookPos);
    }
    
    private IEnumerator ChangeSecondBarWithDelay(float fillAmount) {
        yield return new WaitForSeconds(_secondBarChangeDelay);
        _secondHpBar.DOFillAmount(fillAmount, _secondBarChangeDelay);
    }
}
}
