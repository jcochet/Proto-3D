using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  Moves the attached GameObject according to the player's inputs
 *  @required   CharacterController Component
 *  @required   Child with Animator Controller Component (3 parameters : "vSpeed", "jump", "grounded")
 *  @author     Julien "pimkeomi" Cochet
 */
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement: MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------
    /*
	** 	VARIABLES
	*/
    //----------------------------------------------------------------------------------------------------

    // Child's Animator Controller
    public Animator anim;

    // Character's movement speed
    [SerializeField] private float speed = 5.0f;
    // Character's speed of rotation
    [SerializeField] private float rotationSpeed = 0.1f;
    // Character's height of jump
    [SerializeField] private float jumpHeight = 1.0f;
    // Gravity
    [SerializeField] private float gravity = 9.81f;


    // InputActions
    private PlayerInputActions inputAction;
    // CharacterController Component of the attached GameObject
    private CharacterController controller;
    // Transform component of themain camera
    private Transform camTrans;
    // Character's movement
    private Vector2 movementInput;
    // Character's velocity
    private Vector3 velocity;


    //----------------------------------------------------------------------------------------------------
    /*
	** 	METHODS
	*/
    //----------------------------------------------------------------------------------------------------

    // Awake is used to initialize any variables or game state before the game starts
    private void Awake()
    {
        this.inputAction = new PlayerInputActions();
        this.inputAction.PlayerControls.Move.performed += ctx => this.movementInput = ctx.ReadValue<Vector2>();
        this.inputAction.PlayerControls.Jump.performed += _ => this.Jump();

        this.controller = this.gameObject.GetComponent<CharacterController>();

        this.camTrans = Camera.main.transform;
    }

    // Update is called once per frame
    private void Update()
    {
        this.Move();
    }
    
    // Mades the attached GameObject move
    private void Move()
    {
        this.Animate(this.movementInput.x, this.movementInput.y);

        if (this.controller.isGrounded && this.velocity.y < 0)
        {
            this.velocity.y = -2;
        }

        // Forward according to the camera's view
        Vector3 forward = this.camTrans.forward;
        // Right according to the camera's view
        Vector3 right = this.camTrans.right;

        forward.y = 0.0f;
        forward.Normalize();
        right.y = 0.0f;
        right.Normalize();

        Vector3 move = ((forward * this.movementInput.y) + (right * this.movementInput.x));

        // Rotation
        if (move != Vector3.zero)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(move), this.rotationSpeed);
        }

        // Movement
        this.controller.Move(move * this.speed * Time.deltaTime);
        
        // Gravity
        this.velocity.y -= this.gravity * Time.deltaTime;
        this.controller.Move(this.velocity * Time.deltaTime);
    }
    
    // Mades the attached GameObject move
    private void Jump()
    {
        if(this.controller.isGrounded)
        {
            this.velocity.y = Mathf.Sqrt(this.jumpHeight * 2.0f * this.gravity);
            this.anim.SetTrigger("jump");
        }
    }

    // Animates the character
    private void Animate(float xInput, float zInput)
    {
        float vSpeed = new Vector2(xInput, zInput).sqrMagnitude;
        this.anim.SetFloat("vSpeed", vSpeed);

        this.anim.SetBool("grounded", this.controller.isGrounded);
    }

    // OnEnable is called when the attached GameObject is enable and active
    private void OnEnable()
    {
        this.inputAction.Enable();
    }

    // OnEnable is called when the attached GameObject is disable or inactive
    private void OnDisable()
    {
        this.inputAction.Disable();
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

    public float GetGravity()
    {
        return this.gravity;
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

    public void SetGravity(float gravity)
    {
        this.gravity = gravity;
    }
}