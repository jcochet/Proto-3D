using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/**
 *  Manages the cameras connected to the character
 *  @required   PlayerMovement for camera shaking
 *  @author     Julien "pimkeomi" Cochet
 */
[RequireComponent(typeof(PlayerMovement))]
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
    // Maximum amplitude for the noise on the first person camera
    public float FPNoiseAmplitudeMax = 0.75f;
    // Maximum frequency for the noise on the first person camera
    public float FPNoiseFrequencyMax = 3.0f;
    // Maximum amplitude for the noise on the third person camera
    public float TPNoiseAmplitudeMax = 0.0f;
    // Maximum frequency for the noise on the third person camera
    public float TPNoiseFrequencyMax = 0.0f;

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
        this.Shake();
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

    // Shake the camera according to vSpeed
    private void Shake()
    {
        float vSpeed = this.gameObject.GetComponent<PlayerMovement>().GetVSpeed();
        if (this.thirdPersonView == false)
        {
            this.firstPersonCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = vSpeed * this.FPNoiseAmplitudeMax;
            this.firstPersonCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = vSpeed * this.FPNoiseFrequencyMax;
        } else
        {
            // Top rig
            this.thirdPersonCam.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = vSpeed * this.TPNoiseAmplitudeMax;
            this.thirdPersonCam.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = vSpeed * this.TPNoiseFrequencyMax;

            // Middle rig
            this.thirdPersonCam.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = vSpeed * this.TPNoiseAmplitudeMax;
            this.thirdPersonCam.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = vSpeed * this.TPNoiseFrequencyMax;

            // Bottom rig
            this.thirdPersonCam.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = vSpeed * this.TPNoiseAmplitudeMax;
            this.thirdPersonCam.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = vSpeed * this.TPNoiseFrequencyMax;
        }
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
        this.transform.localRotation = Quaternion.Euler(0.0f, this.thirdPersonCam.m_XAxis.Value, 0.0f);
        this.xRotation = (this.thirdPersonCam.m_YAxis.Value * 180.0f) - 90.0f;

        this.firstPersonCam.Priority = 10;
        this.thirdPersonCam.Priority = 9;

        this.thirdPersonView = false;
    }

    // Switch to third person view
    private void SwitchToTPV()
    {
        this.thirdPersonCam.m_XAxis.Value = Camera.main.transform.eulerAngles.y;
        this.thirdPersonCam.m_YAxis.Value = (this.xRotation + 90.0f) / 180.0f;

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
