using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

/**
 *  Manages the cameras connected to the character
 *  @required   PlayerInput Component
 *  @required   PlayerMovement for camera shaking
 *  @author     Julien "pimkeomi" Cochet
 */
[RequireComponent(typeof(PlayerInput))]
public class CharacterCamera : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------
    /*
	** 	VARIABLES
	*/
    //----------------------------------------------------------------------------------------------------

    // Indicates if the movements are for third person view (first person view if not)
    public bool thirdPersonView;
    // Transform component of the character's camera
    public Transform camTrans;
    // Camera for first person view
    public CinemachineVirtualCamera firstPersonCamera;
    // Camera for third person view
    public CinemachineFreeLook thirdPersonCamera;
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
    
    // Player's input
    private Vector2 cameraInput = Vector2.zero;
    // First person camera's rotation on x axis
    private float xRotation = 0.0f;


    //----------------------------------------------------------------------------------------------------
    /*
	** 	METHODS
	*/
    //----------------------------------------------------------------------------------------------------
    
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

        this.firstPersonCamera.transform.localRotation = Quaternion.Euler(this.xRotation, 0.0f, 0.0f);
        this.transform.Rotate(Vector3.up * inputX);
    }

    // Shake the camera according to vSpeed
    private void Shake()
    {
        float hSpeed = this.gameObject.GetComponent<CharacterMovement>().GetHSpeed();
        if (this.thirdPersonView == false)
        {
            this.firstPersonCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = hSpeed * this.FPNoiseAmplitudeMax;
            this.firstPersonCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = hSpeed * this.FPNoiseFrequencyMax;
        } else
        {
            // Top rig
            this.thirdPersonCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = hSpeed * this.TPNoiseAmplitudeMax;
            this.thirdPersonCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = hSpeed * this.TPNoiseFrequencyMax;

            // Middle rig
            this.thirdPersonCamera.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = hSpeed * this.TPNoiseAmplitudeMax;
            this.thirdPersonCamera.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = hSpeed * this.TPNoiseFrequencyMax;

            // Bottom rig
            this.thirdPersonCamera.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = hSpeed * this.TPNoiseAmplitudeMax;
            this.thirdPersonCamera.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = hSpeed * this.TPNoiseFrequencyMax;
        }
    }

    // Change the view for first or third person
    private void OnSwitchCamera()
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
        this.transform.localRotation = Quaternion.Euler(0.0f, this.thirdPersonCamera.m_XAxis.Value, 0.0f);
        this.xRotation = (this.thirdPersonCamera.m_YAxis.Value * 180.0f) - 90.0f;

        this.firstPersonCamera.Priority = 10;
        this.thirdPersonCamera.Priority = 9;

        this.thirdPersonView = false;
    }

    // Switch to third person view
    private void SwitchToTPV()
    {
        this.thirdPersonCamera.m_XAxis.Value = this.camTrans.eulerAngles.y;
        this.thirdPersonCamera.m_YAxis.Value = (this.xRotation + 90.0f) / 180.0f;

        this.thirdPersonCamera.Priority = 10;
        this.firstPersonCamera.Priority = 9;

        this.thirdPersonView = true;
    }


    private void OnFirstPersonCamera(InputValue value)
    {
        this.cameraInput = value.Get<Vector2>();
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
