using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 *  Manages the local multiplayer
 *  @required   PlayerInputManager Component
 *  @author     Julien "pimkeomi" Cochet
 */
[RequireComponent(typeof(PlayerInputManager))]
public class LocalMultiplayerManager : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------
    /*
	** 	VARIABLES
	*/
    //----------------------------------------------------------------------------------------------------

    // Position of the characters spawn
    public Vector3 spawn;

    // Initial position of the playerPrefab
    private Vector3 playerPrefabPosition;


    //----------------------------------------------------------------------------------------------------
    /*
	** 	METHODS
	*/
    //----------------------------------------------------------------------------------------------------

    // Awake is used to initialize any variables or game state before the game starts
    private void Awake()
    {
        this.playerPrefabPosition = this.gameObject.GetComponent<PlayerInputManager>().playerPrefab.transform.position;
        this.gameObject.GetComponent<PlayerInputManager>().playerPrefab.transform.position = this.spawn;
    }

    // Called when this object is disable
    private void OnDisable()
    {
        this.gameObject.GetComponent<PlayerInputManager>().playerPrefab.transform.position = this.playerPrefabPosition;
    }


}
