using Cinemachine;

using UnityEngine;

namespace Project {

    public class InputManager : MonoBehaviour {
        [SerializeField] bool _isInputGoingFromStick;
        [SerializeField] FixedJoystick _joystick;
        [SerializeField] CinemachineFreeLook _cinemachineFreeLook;
        public static InputManager instance => _instance;
        
        static InputManager _instance;

        bool _isLeftHanded;
        bool _isMovingCamera;
        float _halfWidthOfScreen;
        
        void Awake() {
            _halfWidthOfScreen = Screen.width / 2f;
            _instance = this;
            if(_isInputGoingFromStick)
                _joystick.gameObject.SetActive(true);
        }

        void Update() {
            if (!_isInputGoingFromStick) 
                return;
            CameraMove();
        }

        public Vector3 GetInputDirectionNormalized() {
            Vector3 direction = new Vector3(GetHorizontalInput(), 0, GetVerticalInput());
            return direction.normalized;
        }

        float GetHorizontalInput() {
            return _isInputGoingFromStick ? _joystick.Horizontal : Input.GetAxisRaw("Horizontal");
        }

        float GetVerticalInput() {
            return _isInputGoingFromStick ? _joystick.Vertical : Input.GetAxisRaw("Vertical");
        }

        void CameraMove() {
            Vector3 firstTouchPos = Vector3.zero;
            if (Input.GetMouseButtonDown(0)) {
                if (_isLeftHanded ? Input.mousePosition.x < _halfWidthOfScreen : Input.mousePosition.x > _halfWidthOfScreen) {
                    firstTouchPos = Input.mousePosition;
                    _isMovingCamera = true;
                }
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
