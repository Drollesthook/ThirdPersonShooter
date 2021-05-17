using UnityEngine;

namespace Project {
    
public class Player : MonoBehaviour {
    [SerializeField] float _speedMultiplyer = 1;
    
    MovementController _movementController;
    WeaponController _weaponController;
    InputManager _inputManager;

    void Awake() {
        _movementController = GetComponent<MovementController>();
        _inputManager = InputManager.instance;
    }

    void Update() {
        Move();
        if (Input.GetMouseButtonDown(0))
            Shoot();
    }

    void Move() {
        _movementController.MoveAndRotate(_inputManager.GetInputDirectionNormalized(), _speedMultiplyer);
    }

    void Shoot() {
        _weaponController.ShootWeapon();
    }
}
}
