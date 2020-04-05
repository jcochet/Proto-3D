using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  Manages the cameras connected to the character
 *  @author     Julien "pimkeomi" Cochet
 */
public class PlayerCamera : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------
    /*
	** 	VARIABLES
	*/
    //----------------------------------------------------------------------------------------------------

    // Indicates if the movements are for third person view (first person view if not)
    public bool thirdPersonView;
    // Camera for first person view
    public Cinemachine.CinemachineVirtualCamera firstPersonCam;
    // Camera for third person view
    public Cinemachine.CinemachineFreeLook thirdPersonCam;

    // Mouse or joystick sensitivity
    [SerializeField] private float sensitivity = 200.0f;

    // InputActions
    private PlayerInputActions inputAction;
    // Player's input
    private Vector2 cameraInput = Vector2.zero;
    // First person camera's rotation on x axis
    private float xRotation = 0.0f;


    //----------------------------------------------------------------------------------------------------
    /*
	** 	METHODS
	*/
    //----------------------------------------------------------------------------------------------------

    // Awake is used to initialize any variables or game state before the game starts
    private void Awake()
    {
        this.inputAction = new PlayerInputActions();
        this.inputAction.CameraControls.FirstPerson.performed += ctx => this.cameraInput = ctx.ReadValue<Vector2>();
        this.inputAction.CameraControls.SwitchCamera.performed += _ => this.SwitchView();
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Hide the cursor
        // Cursor.lockState = CursorLockMode.Locked;

        if (this.thirdPersonView == true)
        {
            this.SwitchToTPV();
        } else
        {
            this.SwitchToFPV();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (this.thirdPersonView == false)
        {
            this.Rotate();
        }
    }

    // Rotates the character and the camera on first person view
    private void Rotate()
    {
        float inputX = this.cameraInput.x * this.sensitivity * Time.deltaTime;
        float inputY = this.cameraInput.y * this.sensitivity * Time.deltaTime;

        this.xRotation -= inputY;
        this.xRotation = Mathf.Clamp(this.xRotation, -90.0f, 90.0f);

        this.firstPersonCam.transform.localRotation = Quaternion.Euler(this.xRotation, 0.0f, 0.0f);
        this.transform.Rotate(Vector3.up * inputX);
    }

    // Change the view for first or third person
    private void SwitchView()
    {
        if (this.thirdPersonView == true)
        {
            this.SwitchToFPV();
        } else
        {
            this.SwitchToTPV();
        }
    }

    // Switch to first person view
    private void SwitchToFPV()
    {
        this.firstPersonCam.Priority = 10;
        this.thirdPersonCam.Priority = 9;
        this.thirdPersonView = false;
    }

    // Switch to third person view
    private void SwitchToTPV()
    {
        this.thirdPersonCam.Priority = 10;
        this.firstPersonCam.Priority = 9;
        this.thirdPersonView = true;
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

    public float GetSensitivity()
    {
        return this.sensitivity;
    }


    //----------------------------------------------------------------------------------------------------
    /*
	** 	SETS
	*/
    //----------------------------------------------------------------------------------------------------

    public void SetSensitivity(float sensitivity)
    {
        this.sensitivity = sensitivity >= 0.0f ? sensitivity : 0.0f;
    }
}
