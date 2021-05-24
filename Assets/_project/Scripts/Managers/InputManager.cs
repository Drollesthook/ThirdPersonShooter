using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

using Cursor = UnityEngine.Cursor;

namespace Project.Managers {
    public class InputManager : MonoBehaviour {
        private static InputManager _instance;
        public event Action<Vector3> inputMoveDirectionChanged;
        public event Action<Vector3> inputCameraDirectionChanged;
        public event Action<int> weaponSelected;
        public event Action fireButtonPressed;
        public event Action fireButtonReleased;
        
        public event Action<float> grenadeForceChanged;
        public event Action grenadeButtonPressed;
        public event Action<float> grenadeButtonReleased;
        
        [SerializeField] private CinemachineFreeLook _cinemachineFreeLook;
        [SerializeField] private float _inputSensitivityThreshold;
        [SerializeField] private bool _isInputGoingFromStick;
        [SerializeField] private bool _isCameraInverted;
        [SerializeField] private FixedJoystick _moveJoystick;
        [SerializeField] private FixedJoystick _cameraJoystick;
        [SerializeField] private FixedJoystick _grenadeJoystick;
        [SerializeField] private Image _shootButton;

        public static InputManager instance => _instance;

        private bool _isLeftHanded;
        private bool _isMovingCamera;
        private bool _isGrenadeEquipped;
        private float _halfWidthOfScreen;
        private Vector3 _beginCameraInputMousePos;
        private Vector3 _currentCameraInputMousePos;
        private float _lastGrenadeJoystickVerticalValue;


        private void Awake() {
            _halfWidthOfScreen = Screen.width / 2f;
            _instance = this;
            if (!_isInputGoingFromStick) {
                _currentCameraInputMousePos = Input.mousePosition;
                //Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                return;
            }

            _moveJoystick.gameObject.SetActive(true);
            _cameraJoystick.gameObject.SetActive(true);
            _grenadeJoystick.gameObject.SetActive(true);
            _shootButton.enabled = true;
        }

        private void Update() {
            CalculateMovementInputDirectionNormalized();
            CalculateCameraInputDirection();
            GetFireButtonState();
            GetGrenadeButtonState();
            SelectingWeapon();
        }

        public void ShootButtonPressed() {
            fireButtonPressed?.Invoke();
        }

        public void ShootButtonReleased() {
            fireButtonReleased?.Invoke();
        }
        
        public void GrenadeEquipped() {
            if(_isGrenadeEquipped)
                return;
            grenadeButtonPressed?.Invoke();
            _isGrenadeEquipped = true;
        }

        public void GrenadeOut() {
            if(!_isGrenadeEquipped)
                return;
            grenadeButtonReleased?.Invoke(_lastGrenadeJoystickVerticalValue < 0 ? -_lastGrenadeJoystickVerticalValue : _lastGrenadeJoystickVerticalValue);
            _isGrenadeEquipped = false;
        }

        private void CalculateMovementInputDirectionNormalized() {
            var direction = new Vector3(GetHorizontalMovementInput(), 0, GetVerticalMovementInput());
            if (direction.magnitude > _inputSensitivityThreshold)
                inputMoveDirectionChanged?.Invoke(direction.normalized);
        }

        private float GetHorizontalMovementInput() {
           return _isInputGoingFromStick ? _moveJoystick.Horizontal : Input.GetAxisRaw("Horizontal");
        }

        private float GetVerticalMovementInput() {
            return _isInputGoingFromStick ? _moveJoystick.Vertical : Input.GetAxisRaw("Vertical");
        }

        private void CalculateCameraInputDirection() {
            if (_isInputGoingFromStick) {
                Vector3 direction = new Vector3(_isCameraInverted? GetVerticalCameraInput(): -GetVerticalCameraInput(), GetHorizontalCameraInput(), 0);
                if(direction.magnitude > _inputSensitivityThreshold)
                inputCameraDirectionChanged?.Invoke(direction);
            } else {
                _beginCameraInputMousePos = _currentCameraInputMousePos;
                _currentCameraInputMousePos = Input.mousePosition;
                Vector3 direction = _currentCameraInputMousePos - _beginCameraInputMousePos;
                direction.z = direction.y;
                direction.y = direction.x;
                direction.x = _isCameraInverted ? direction.z : -direction.z;
                direction.z = 0;
                inputCameraDirectionChanged?.Invoke(direction);
            }
        }

        private float GetHorizontalCameraInput() {
            if (_isGrenadeEquipped) {
                return Mathf.Abs(_grenadeJoystick.Horizontal) > _inputSensitivityThreshold
                           ? _grenadeJoystick.Horizontal
                           : 0;
            }
           return _cameraJoystick.Horizontal;
        }
        
        private float GetVerticalCameraInput() {
            return _cameraJoystick.Vertical;
        }

        private void GetFireButtonState() {
            if(_isInputGoingFromStick)
                return;
            if(Input.GetMouseButtonDown(0))
                fireButtonPressed?.Invoke();
            if(Input.GetMouseButtonUp(0))
                fireButtonReleased?.Invoke();
        }

        private void SelectingWeapon() {
            if(Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
                weaponSelected?.Invoke(0);
            if(Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
                weaponSelected?.Invoke(1);
            if(Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
                weaponSelected?.Invoke(2);
            if(Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
                weaponSelected?.Invoke(3);
            if(Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5))
                weaponSelected?.Invoke(4);
        }

        private void GetGrenadeButtonState() {
            if(!_isInputGoingFromStick)
                GetGrenadeButtonStateByKeyboard();
            if(_isGrenadeEquipped)
                GetGrenadeJoystickDirection();
        }

        private void GetGrenadeButtonStateByJoystick() {
            if (_grenadeJoystick.Direction.magnitude > 0 && !_isGrenadeEquipped) {
                GrenadeEquipped();
            }
            if (_isGrenadeEquipped && Input.GetMouseButtonUp(0)) {
                GrenadeOut();
            }
        }
        
        private void GetGrenadeButtonStateByKeyboard() {
            if (!_isGrenadeEquipped && Input.GetKeyDown(KeyCode.G)) {
                GrenadeEquipped();
            }
            if (_isGrenadeEquipped && Input.GetMouseButtonDown(0)) {
                GrenadeOut();
            }
        }

        

        private void GetGrenadeJoystickDirection() {
            if(!_isGrenadeEquipped)
                return;

            if (_isInputGoingFromStick) {
                _lastGrenadeJoystickVerticalValue = _grenadeJoystick.Vertical;
            } else {
                // надо продумать как брать инпут с мышки, а пока среднее значение
                _lastGrenadeJoystickVerticalValue = 0.5f;
            }
            grenadeForceChanged?.Invoke(_lastGrenadeJoystickVerticalValue < 0 ? -_lastGrenadeJoystickVerticalValue : _lastGrenadeJoystickVerticalValue);
        }
    }
}