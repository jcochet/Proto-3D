using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 *  Moves the attached GameObject according to the player's inputs
 *  @required   PlayerInput Component
 *  @required   CharacterController Component
 *  @required   CharacterCamera Component
 *  @required   Child with Animator Controller Component (3 parameters : "vSpeed", "jump", "grounded")
 *  @required   Camera for this character
 *  @author     Julien "pimkeomi" Cochet
 */
[RequireComponent(typeof(PlayerInput), typeof(CharacterController), typeof(CharacterCamera))]
public class CharacterMovement : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------
    /*
	** 	VARIABLES
	*/
    //----------------------------------------------------------------------------------------------------

    // Child's Animator Controller
    public Animator anim;
    // Gravity
    public float gravity = 9.81f;

    // Character's movement speed
    [SerializeField] private float speed = 6.0f;
    // Character's speed of rotation
    [SerializeField] private float rotationSpeed = 0.1f;
    // Character's height of jump
    [SerializeField] private float jumpHeight = 1.0f;
    
    // CharacterController Component of the attached GameObject
    private CharacterController controller;
    // Player's input
    private Vector2 movementInput = Vector2.zero;
    // Character's movement
    private Vector3 move = Vector3.zero;
    // Character's horizontal speed 
    private float hSpeed = 0.0f;
    // Character's velocity
    private Vector3 velocity = Vector3.zero;


    //----------------------------------------------------------------------------------------------------
    /*
	** 	METHODS
	*/
    //----------------------------------------------------------------------------------------------------

    // Awake is used to initialize any variables or game state before the game starts
    private void Awake()
    {
        this.controller = this.gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        this.Move();
    }

    // Mades the attached GameObject move
    private void Move()
    {
        // Animation
        this.Animate(this.movementInput.x, this.movementInput.y);

        // Gravity 
        if (this.controller.isGrounded && this.velocity.y < 0)
        {
            this.velocity.y = -2;
        }

        if (this.gameObject.GetComponent<CharacterCamera>().thirdPersonView)
        {
            // Forward according to the camera's view
            Vector3 forward = this.gameObject.GetComponent<CharacterCamera>().camTrans.forward;
            // Right according to the camera's view
            Vector3 right = this.gameObject.GetComponent<CharacterCamera>().camTrans.right;

            forward.y = 0.0f;
            forward.Normalize();
            right.y = 0.0f;
            right.Normalize();

            // Third person view's move
            this.move = ((forward * this.movementInput.y) + (right * this.movementInput.x));

            // Rotation
            if (this.move != Vector3.zero)
            {
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(this.move), this.rotationSpeed);
            }
        } else
        {
            // First person view's move
            this.move = ((this.transform.forward * this.movementInput.y) + (this.transform.right * this.movementInput.x));
        }

        // Movement
        this.controller.Move(this.move * this.speed * Time.deltaTime);

        // Gravity
        this.velocity.y -= this.gravity * Time.deltaTime;
        this.controller.Move(this.velocity * Time.deltaTime);
    }

    // Read the player's input for horizontal movements
    private void OnHorizontalMove(InputValue value)
    {
        this.movementInput = value.Get<Vector2>();
    }

    // Mades the attached GameObject move
    private void OnJump()
    {
        if (this.controller.isGrounded)
        {
            this.velocity.y = Mathf.Sqrt(this.jumpHeight * 2.0f * this.gravity);
            this.anim.SetTrigger("jump");
        }
    }

    // Animates the character
    private void Animate(float xInput, float zInput)
    {
        this.hSpeed = new Vector2(xInput, zInput).sqrMagnitude;
        this.anim.SetFloat("hSpeed", this.hSpeed);

        this.anim.SetBool("grounded", this.controller.isGrounded);
    }


    //----------------------------------------------------------------------------------------------------
    /*
	** 	GETS
	*/
    //----------------------------------------------------------------------------------------------------

    public float GetSpeed()
    {
        return this.speed;
    }

    public float GetRotationSpeed()
    {
        return this.rotationSpeed;
    }

    public float GetHSpeed()
    {
        return this.hSpeed;
    }


    //----------------------------------------------------------------------------------------------------
    /*
	** 	SETS
	*/
    //----------------------------------------------------------------------------------------------------

    public void SetSpeed(float speed)
    {
        this.speed = speed >= 0.0f ? speed : 0.0f;
    }

    public void SetRotationSpeed(float rotationSpeed)
    {
        this.rotationSpeed = rotationSpeed >= 0.0f ? rotationSpeed : 0.0f;
    }


}
