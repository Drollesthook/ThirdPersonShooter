using System;
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
        
        [SerializeField] private float _inputSensitivityThreshold;
        [SerializeField] private float _joystickLookSensitivity = 2f;
        [SerializeField] private float _mouseLookSensitivity = 200f;
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
        private bool _isGamePlayStarted;
        private float _halfScreenWidth;
        private float _halfScreenHeight;
        private Vector3 _beginCameraInputMousePos;
        private Vector3 _currentCameraInputMousePos;
        private float _grenadeEquippedPositionY;
        private float _lastGrenadeJoystickVerticalValue;

        private void Awake() {
            _instance = this;
        }

        private void Start() {
            GameManager.instance.gamePlayStarted += OnGamePlayStarted;
            GameManager.instance.playerDied += OnPlayerDied;
            GameManager.instance.playerRespawned += OnPlayerRespawned;
            if (_isInputGoingFromStick) {
                _moveJoystick.gameObject.SetActive(true);
                _cameraJoystick.gameObject.SetActive(true);
                _grenadeJoystick.gameObject.SetActive(true);
                _shootButton.enabled = true;
                return;
            }
            _halfScreenWidth = Screen.width * 0.5f;
            _halfScreenHeight = Screen.height * 0.5f;
        }

        private void OnDestroy() {
            GameManager.instance.gamePlayStarted -= OnGamePlayStarted;
            GameManager.instance.playerDied -= OnPlayerDied;
            GameManager.instance.playerRespawned -= OnPlayerRespawned;
        }

        private void Update() {
            if(!_isGamePlayStarted)
                return;
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

        public void SelectWeaponById(int weaponID) {
            weaponSelected?.Invoke(weaponID);
        }

        private void OnGamePlayStarted() {
            _isGamePlayStarted = true;
            _currentCameraInputMousePos = Input.mousePosition;
            if(!_isInputGoingFromStick) Cursor.visible = false;
        }

        private void OnPlayerDied() {
            _isGamePlayStarted = false;
            Cursor.visible = true;
        }

        private void OnPlayerRespawned() {
            _isGamePlayStarted = true;
            _currentCameraInputMousePos = Input.mousePosition;
            if(!_isInputGoingFromStick) Cursor.visible = false;
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
            Vector3 direction;
            if (_isInputGoingFromStick) {
                direction = new Vector3(_isCameraInverted? GetVerticalCameraInput(): -GetVerticalCameraInput(), GetHorizontalCameraInput(), 0);
            } else {
                _beginCameraInputMousePos = _currentCameraInputMousePos;
                _currentCameraInputMousePos = Input.mousePosition;
                direction = _currentCameraInputMousePos - _beginCameraInputMousePos;
                direction.z = direction.y / _halfScreenHeight;
                direction.y = direction.x / _halfScreenWidth;
                direction.x = _isCameraInverted ? direction.z : -direction.z;
                direction.z = 0;
            }
            inputCameraDirectionChanged?.Invoke(direction * (_isInputGoingFromStick ? _joystickLookSensitivity : _mouseLookSensitivity));
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
            if(_isInputGoingFromStick)
                return;
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
        
        private void GetGrenadeButtonStateByKeyboard() {
            if (!_isGrenadeEquipped && Input.GetKeyDown(KeyCode.G)) {
                _grenadeEquippedPositionY = Input.mousePosition.y;
                GrenadeEquipped();
            }
            if (_isGrenadeEquipped && Input.GetKeyUp(KeyCode.G)) {
                GrenadeOut();
            }
        }

        

        private void GetGrenadeJoystickDirection() {
            if(!_isGrenadeEquipped)
                return;

            _lastGrenadeJoystickVerticalValue = _isInputGoingFromStick ? _grenadeJoystick.Vertical :
                                                    Mathf.Clamp((Input.mousePosition.y - _grenadeEquippedPositionY + _halfScreenHeight * 0.5f)/_halfScreenHeight, 0, 1);
            grenadeForceChanged?.Invoke(_lastGrenadeJoystickVerticalValue < 0 ? -_lastGrenadeJoystickVerticalValue : _lastGrenadeJoystickVerticalValue);
        }
    }
}