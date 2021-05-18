using System;
using Cinemachine;
using UnityEngine;

namespace Project.Managers {
    public class InputManager : MonoBehaviour {
        static InputManager _instance;
        public event Action<Vector3> InputDirectionChanged;
        public event Action ScreenTouched;
        public event Action ScreenReleased;
        [SerializeField] CinemachineFreeLook _cinemachineFreeLook;
        [SerializeField] float _inputSensititvityThreshold;
        [SerializeField] bool _isInputGoingFromStick;
        [SerializeField] FixedJoystick _joystick;

        public static InputManager instance => _instance;

        bool _isLeftHanded;
        bool _isMovingCamera;
        float _halfWidthOfScreen;


        void Awake() {
            _halfWidthOfScreen = Screen.width / 2f;
            _instance = this;
            if (_isInputGoingFromStick)
                _joystick.gameObject.SetActive(true);
        }

        void Update() {
            CalculateInputDirectionNormalized();
            if (!_isInputGoingFromStick)
                return;

            CameraMove();
        }

        void CalculateInputDirectionNormalized() {
            var direction = new Vector3(GetHorizontalInput(), 0, GetVerticalInput());
            if (direction.magnitude > _inputSensititvityThreshold)
                InputDirectionChanged?.Invoke(direction.normalized);
        }

        float GetHorizontalInput() {
           return _isInputGoingFromStick ? _joystick.Horizontal : Input.GetAxisRaw("Horizontal");
        }

        float GetVerticalInput() {
            return _isInputGoingFromStick ? _joystick.Vertical : Input.GetAxisRaw("Vertical");
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