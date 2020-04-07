using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

/**
 *  Manages the entry and exit of a player in the game
 *  @required   PlayerInput Component
 *  @author     Julien "pimkeomi" Cochet
 */
[RequireComponent(typeof(PlayerInput))]
public class CharacterLocalMultiplayer : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------
    /*
	** 	VARIABLES
	*/
    //----------------------------------------------------------------------------------------------------

    // Virtual camera for first person view
    public CinemachineVirtualCamera firstPersonCamera;
    // Virtual camera for third person view
    public CinemachineFreeLook thirdPersonCamera;
    // Camera of this player
    public Camera playerCamera;
    
    // Number of this character's player
    private int playerIndex;
    // Current Control Scheme
    private string scheme;


    //----------------------------------------------------------------------------------------------------
    /*
	** 	METHODS
	*/
    //----------------------------------------------------------------------------------------------------

    // Awake is used to initialize any variables or game state before the game starts
    private void Awake()
    {
        if (GameObject.FindObjectOfType<PlayerInputManager>() == null)
        {
            this.enabled = false;
        } else
        {
            this.playerIndex = this.gameObject.GetComponent<PlayerInput>().playerIndex;
            this.scheme = this.gameObject.GetComponent<PlayerInput>().currentControlScheme;
            switch (this.playerIndex)
            {
                case 0:
                    this.firstPersonCamera.gameObject.layer = LayerMask.NameToLayer("PlayerOne");
                    this.thirdPersonCamera.gameObject.layer = LayerMask.NameToLayer("PlayerOne");

                    this.playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("PlayerTwo"));
                    this.playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("PlayerThree"));
                    this.playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("PlayerFour"));

                    if (this.scheme == "Keyboard")
                    {
                        this.thirdPersonCamera.m_XAxis.m_InputAxisName = "ThirdPersonCameraXM";
                        this.thirdPersonCamera.m_YAxis.m_InputAxisName = "ThirdPersonCameraYM";
                    } else
                    {
                        this.thirdPersonCamera.m_XAxis.m_InputAxisName = "ThirdPersonCameraX1";
                        this.thirdPersonCamera.m_YAxis.m_InputAxisName = "ThirdPersonCameraY1";
                    }
                    break;
                case 1:
                    this.firstPersonCamera.gameObject.layer = LayerMask.NameToLayer("PlayerTwo");
                    this.thirdPersonCamera.gameObject.layer = LayerMask.NameToLayer("PlayerTwo");

                    this.playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("PlayerOne"));
                    this.playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("PlayerThree"));
                    this.playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("PlayerFour"));

                    if (this.scheme == "Keyboard")
                    {
                        this.thirdPersonCamera.m_XAxis.m_InputAxisName = "ThirdPersonCameraXM";
                        this.thirdPersonCamera.m_YAxis.m_InputAxisName = "ThirdPersonCameraYM";
                    } else
                    {
                        this.thirdPersonCamera.m_XAxis.m_InputAxisName = "ThirdPersonCameraX2";
                        this.thirdPersonCamera.m_YAxis.m_InputAxisName = "ThirdPersonCameraY2";
                    }
                    break;
                case 2:
                    this.firstPersonCamera.gameObject.layer = LayerMask.NameToLayer("PlayerThree");
                    this.thirdPersonCamera.gameObject.layer = LayerMask.NameToLayer("PlayerThree");

                    this.playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("PlayerOne"));
                    this.playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("PlayerTwo"));
                    this.playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("PlayerFour"));

                    if (this.scheme == "Keyboard")
                    {
                        this.thirdPersonCamera.m_XAxis.m_InputAxisName = "ThirdPersonCameraXM";
                        this.thirdPersonCamera.m_YAxis.m_InputAxisName = "ThirdPersonCameraYM";
                    } else
                    {
                        this.thirdPersonCamera.m_XAxis.m_InputAxisName = "ThirdPersonCameraX3";
                        this.thirdPersonCamera.m_YAxis.m_InputAxisName = "ThirdPersonCameraY3";
                    }
                    break;
                case 3:
                    this.firstPersonCamera.gameObject.layer = LayerMask.NameToLayer("PlayerFour");
                    this.thirdPersonCamera.gameObject.layer = LayerMask.NameToLayer("PlayerFour");

                    this.playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("PlayerOne"));
                    this.playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("PlayerTwo"));
                    this.playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("PlayerThree"));

                    if (this.scheme == "Keyboard")
                    {
                        this.thirdPersonCamera.m_XAxis.m_InputAxisName = "ThirdPersonCameraXM";
                        this.thirdPersonCamera.m_YAxis.m_InputAxisName = "ThirdPersonCameraYM";
                    } else
                    {
                        this.thirdPersonCamera.m_XAxis.m_InputAxisName = "ThirdPersonCameraX4";
                        this.thirdPersonCamera.m_YAxis.m_InputAxisName = "ThirdPersonCameraY4";
                    }
                    break;
            }
        }
    }
    
    // Exit the game
    private void OnExit()
    {
        if(this.playerIndex != 0)
        {
            Destroy(this.transform.parent.gameObject);
        }
    }
    

    //----------------------------------------------------------------------------------------------------
    /*
	** 	GETS
	*/
    //----------------------------------------------------------------------------------------------------

    public float GetPlayerIndex()
    {
        return this.playerIndex;
    }


}
