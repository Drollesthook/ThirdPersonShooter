using System;
using Cinemachine;
using UnityEngine;

namespace Project.Managers {
    public class InputManager : MonoBehaviour {
        private static InputManager _instance;
        public event Action<Vector3> inputDirectionChanged;
        public event Action<int> weaponSelected;
        public event Action fireButtonPressed;
        public event Action fireButtonReleased;
        public event Action aimButtonPressed;
        public event Action aimButtonReleased;
        
        [SerializeField] private CinemachineFreeLook _cinemachineFreeLook;
        [SerializeField] private float _inputSensitivityThreshold;
        [SerializeField] private bool _isInputGoingFromStick;
        [SerializeField] private FixedJoystick _joystick;

        public static InputManager instance => _instance;

        private bool _isLeftHanded;
        private bool _isMovingCamera;
        private float _halfWidthOfScreen;


        private void Awake() {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _halfWidthOfScreen = Screen.width / 2f;
            _instance = this;
            if (_isInputGoingFromStick)
                _joystick.gameObject.SetActive(true);
        }

        private void Update() {
            CalculateInputDirectionNormalized();
            GetFireButtonState();
            SelectingWeapon();
            if (!_isInputGoingFromStick)
                return;

            CameraMove();
        }

        private void CalculateInputDirectionNormalized() {
            var direction = new Vector3(GetHorizontalInput(), 0, GetVerticalInput());
            if (direction.magnitude > _inputSensitivityThreshold)
                inputDirectionChanged?.Invoke(direction.normalized);
        }

        private float GetHorizontalInput() {
           return _isInputGoingFromStick ? _joystick.Horizontal : Input.GetAxisRaw("Horizontal");
        }

        private float GetVerticalInput() {
            return _isInputGoingFromStick ? _joystick.Vertical : Input.GetAxisRaw("Vertical");
        }

        private void GetFireButtonState() {
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

        private void CameraMove() {
            Vector3 firstTouchPos = Vector3.zero;
            if (Input.GetMouseButtonDown(0))
                if (_isLeftHanded
                        ? Input.mousePosition.x < _halfWidthOfScreen
                        : Input.mousePosition.x > _halfWidthOfScreen) {
                    firstTouchPos = Input.mousePosition;
                    _isMovingCamera = true;
                }

            if (Input.GetMouseButton(0) && _isMovingCamera) {
                Vector3 swipeDirection = Input.mousePosition - firstTouchPos;
                _cinemachineFreeLook.m_YAxis.m_InputAxisValue = swipeDirection.y;
                _cinemachineFreeLook.m_XAxis.m_InputAxisValue = swipeDirection.x;
            }

            if (Input.GetMouseButtonUp(0) && _isMovingCamera)
                _isMovingCamera = false;
        }
    }
}