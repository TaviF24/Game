using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    [SerializeField]
    private float speed = 5f;

    private bool isGrounded;
    [SerializeField]
    private float jumpHeight = 3f;
    [SerializeField]
    private float gravity = -9.8f;

    private bool isCrouching = false;
    private bool lerpCrouch = false;
    [SerializeField]
    private float crouchTimer;

    private bool isSprinting;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;

            if (isCrouching)
                controller.height = Mathf.Lerp(controller.height, 1, p);
            else
                controller.height = Mathf.Lerp(controller.height, 2, p);
      
            if (p > 1)
            {
                lerpCrouch = false ;
                crouchTimer = 0f;
            }
        }
    }

    //receive the inputs for our InputManager.cs and apply them to our character controller
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y; // if it was moveDirection.y, the player would move vertically
        controller.Move(transform.TransformDirection(moveDirection)*speed*Time.deltaTime); 
        playerVelocity.y += gravity * Time.deltaTime; // this line brings the player back to the earth:)
        if (isGrounded && playerVelocity.y < 0) {
            playerVelocity.y = -2f; //reset the value of playerVelocity.y because it will contantly grow from the previous line
        }
        controller.Move(playerVelocity*Time.deltaTime); 
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    public void Crouch()
    {
        isCrouching = !isCrouching;
        crouchTimer = 0;
        lerpCrouch = true;
    }

    public void Sprint()
    {
        isSprinting = !isSprinting;
        if(isSprinting)
            speed = 10;
        else
            speed = 5;
    }
}
