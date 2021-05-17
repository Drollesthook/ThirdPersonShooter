using UnityEngine;

namespace Project {
    
public class Player : MonoBehaviour {
    [SerializeField] float _speedMultiplyer = 1;
    [SerializeField] Transform _aimSight;
    
    MovementController _movementController;
    WeaponController _weaponController;
    InputManager _inputManager;

    void Awake() {
        _weaponController = GetComponent<WeaponController>();
        _movementController = GetComponent<MovementController>();
        _inputManager = InputManager.instance;
    }

    void Update() {
        Move();
        if (Input.GetMouseButton(0))
            Shoot();
    }

    void Move() {
        _movementController.MoveAndRotate(InputManager.instance.GetInputDirectionNormalized(), _speedMultiplyer);
    }

    void Shoot() {
        Vector3 aimSightPosition = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        //Vector3 aimSightPosition = _aimSight.position;
        
        _weaponController.ShootWeapon(aimSightPosition);
    }
}
}
