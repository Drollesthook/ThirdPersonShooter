using System;
using Cinemachine;
using UnityEngine;

namespace Project.Managers {
    public class InputManager : MonoBehaviour {
        static InputManager _instance;
        public event Action<Vector3> inputDirectionChanged;
        public event Action<int> weaponSelected;
        public event Action fireButtonPressed;
        public event Action fireButtonReleased;
        public event Action aimButtonPressed;
        public event Action aimButtonReleased;
        
        [SerializeField] CinemachineFreeLook _cinemachineFreeLook;
        [SerializeField] float _inputSensitivityThreshold;
        [SerializeField] bool _isInputGoingFromStick;
        [SerializeField] FixedJoystick _joystick;

        public static InputManager instance => _instance;

        bool _isLeftHanded;
        bool _isMovingCamera;
        float _halfWidthOfScreen;


        void Awake() {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _halfWidthOfScreen = Screen.width / 2f;
            _instance = this;
            if (_isInputGoingFromStick)
                _joystick.gameObject.SetActive(true);
        }

        void Update() {
            CalculateInputDirectionNormalized();
            GetFireButtonState();
            SelectingWeapon();
            if (!_isInputGoingFromStick)
                return;

            CameraMove();
        }

        void CalculateInputDirectionNormalized() {
            var direction = new Vector3(GetHorizontalInput(), 0, GetVerticalInput());
            if (direction.magnitude > _inputSensitivityThreshold)
                inputDirectionChanged?.Invoke(direction.normalized);
        }

        float GetHorizontalInput() {
           return _isInputGoingFromStick ? _joystick.Horizontal : Input.GetAxisRaw("Horizontal");
        }

        float GetVerticalInput() {
            return _isInputGoingFromStick ? _joystick.Vertical : Input.GetAxisRaw("Vertical");
        }

        void GetFireButtonState() {
            if(Input.GetMouseButtonDown(0))
                fireButtonPressed?.Invoke();
            if(Input.GetMouseButtonUp(0))
                fireButtonReleased?.Invoke();
        }

        void SelectingWeapon() {
            
        }

        void CameraMove() {
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