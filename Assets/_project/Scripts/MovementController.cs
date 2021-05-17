using UnityEngine;

namespace Project {
   public class MovementController : MonoBehaviour {
      [SerializeField] float _playerSpeed = 6f;
      [SerializeField] float _turnSmoothTime = 0.1f, _turnSmoothVelocity =  default;

      CharacterController _characterController;
      Transform _mainCameraTransform;

      void Awake() {
         _characterController = GetComponent<CharacterController>();
         _mainCameraTransform = Camera.main.transform;
      }

      public void MoveAndRotate(Vector3 direction, float speedMultiplier) {
         float targetAngle = + _mainCameraTransform.eulerAngles.y;
         float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                                             _turnSmoothTime);
         transform.rotation = Quaternion.Euler(0,angle,0);

         if (direction.magnitude < 0.1f) return;
         Vector3 moveDirection = Quaternion.Euler(0, Mathf.Atan2(direction.x, direction.z)  * Mathf.Rad2Deg + targetAngle, 0) * Vector3.forward;
         _characterController.Move(moveDirection * _playerSpeed * speedMultiplier * Time.deltaTime);
      }

   }
}
