using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;
    public PlayerInput.ShootingActions shooting;

    private PlayerMotor motor;
    private PlayerLook look;
    private PlayerShoot shoot;
    private bool heldShooting = false;
    public float timePressed = 0f;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        shooting = playerInput.Shooting;
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        shoot = GetComponent<PlayerShoot>();

        onFoot.Jump.performed += ctx => motor.Jump(); // anytime when onFoot.Jump is performed, we use a callback context(ctx) to call out the motor.Jump function
        //all actions have 3 states: performed, started, canceled
        
        onFoot.Sprint.performed += ctx => motor.Sprint();
        onFoot.Crouch.performed += ctx => motor.Crouch();
        }



    void FixedUpdate()
    {
        //tell the playermotor to move using the value from our movement action
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
        shooting.Shoot.performed += ctx => {
            heldShooting = true;
        };
        shooting.Shoot.canceled += ctx => {
            heldShooting = false;
        };

        shooting.Reload.performed += ctx => shoot.Reload();

        if (heldShooting)
        {
            shoot.Shoot();
            timePressed += Time.deltaTime;
        }
        else
        {
            timePressed = 0f;
        }
    }

    void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
        shooting.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
        shooting.Disable();
    }
}
