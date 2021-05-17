using UnityEngine;

namespace Project {

    public class InputManager : MonoBehaviour {
        [SerializeField] bool _isInputGoingFromStick;
        [SerializeField] FixedJoystick _joystick;
        public static InputManager instance => _instance;
        
        static InputManager _instance;

        void Awake() {
            _instance = this;
            if(_isInputGoingFromStick)
                _joystick.gameObject.SetActive(true);
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
    }
}
