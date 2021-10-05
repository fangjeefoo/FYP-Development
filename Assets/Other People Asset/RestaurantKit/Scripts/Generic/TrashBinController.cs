using UnityEngine;
using System.Collections;

public class TrashBinController : MonoBehaviour {

	//***************************************************************************//
	// This class manages all thing related to TrashBin.
	// 
	//***************************************************************************//

	//AudioClip
	public AudioClip deleteSfx;

	//Flags
	internal bool canDelete = true;
	private GameObject deliveryPlate;

	//Textures for open/closed states
	public Texture2D[] state;

	//Flag used to let managers know that player is intended to send the order to trashbin.
	public bool isCloseEnoughToTrashbin;//Do not modify this.


	//***************************************************************************//
	// Simple Init
	//***************************************************************************//
	void Awake (){
		deliveryPlate = GameObject.FindGameObjectWithTag("serverPlate");
		isCloseEnoughToTrashbin = false;
		GetComponent<Renderer>().material.mainTexture = state[0];
	}


	//***************************************************************************//
	// FSM
	//***************************************************************************//
	void Update (){	
		//check if player wants to move the order to trash bin
		if(PlateController.canDeliverOrder) {
			checkDistanceToDelivery();
		}
	}


	//***************************************************************************//
	// If player is dragging the deliveryPlate, check if maybe he wants to trash it.
	// we do this by calculation the distance of deliveryPlate and trashBin.
	//***************************************************************************//
	private float myDistance;
	void checkDistanceToDelivery (){
		myDistance = Vector3.Distance(transform.position, deliveryPlate.transform.position);
		//print("distance to trashBin is: " + myDistance + ".");
		
		//2.0f is a hardcoded value. specify yours with caution.
		if(myDistance < 2.0f) {
			isCloseEnoughToTrashbin = true;
			//change texture
			GetComponent<Renderer>().material.mainTexture = state[1];
		} else {
			isCloseEnoughToTrashbin = false;
			//change texture
			GetComponent<Renderer>().material.mainTexture = state[0];
		}
	}


	/// <summary>
	/// Allow other controllers to update the animation state of this trashbin object
	/// by controlling it's door state.
	/// </summary>
	public void updateDoorState(int _state) {
		if(_state == 1)
			GetComponent<Renderer>().material.mainTexture = state[1];
		else
			GetComponent<Renderer>().material.mainTexture = state[0];
	}


	//***************************************************************************//
	// Activate using trashbin again, after a few seconds.
	//***************************************************************************//
	IEnumerator reactivate (){
		yield return new WaitForSeconds(0.25f);
		canDelete = true;
	}


	//***************************************************************************//
	// Play audioclips.
	//***************************************************************************//
	public void playSfx ( AudioClip _sfx  ){
		GetComponent<AudioSource>().clip = _sfx;
		if(!GetComponent<AudioSource>().isPlaying)
			GetComponent<AudioSource>().Play();
	}

}