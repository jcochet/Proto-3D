using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/**
 *  Manages the entry and exit of a player in the game
 *  @required   NetworkIdentity Component
 *  @author     Julien "pimkeomi" Cochet
 */
[RequireComponent(typeof(NetworkIdentity))]
public class PlayerOnlineSetup : NetworkBehaviour
{
    //----------------------------------------------------------------------------------------------------
    /*
	** 	VARIABLES
	*/
    //----------------------------------------------------------------------------------------------------
    
    // Components to disable is this object is not the local player
    public Behaviour[] componentsToDisable;

    // Main camera before the player join the game
    private Camera sceneCamera;


    //----------------------------------------------------------------------------------------------------
    /*
	** 	METHODS
	*/
    //----------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    private void Start()
    {
        if(!isLocalPlayer)
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        } else
        {
            this.sceneCamera = Camera.main;
            if (this.sceneCamera != null)
            {
                this.sceneCamera.gameObject.SetActive(false);
            }
        }
    }

    // Called when this object is disable
    private void OnDisable()
    {
        if (this.sceneCamera != null)
        {
            this.sceneCamera.gameObject.SetActive(true);
        }
    }


}
