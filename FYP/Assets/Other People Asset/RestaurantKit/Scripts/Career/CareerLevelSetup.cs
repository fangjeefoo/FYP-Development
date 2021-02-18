using UnityEngine;
using System.Collections;

public class CareerLevelSetup : MonoBehaviour {
	
	///*************************************************************************///
	/// Use this class to set different missions for each level.
	/// when you click/tap on any level button, these values automatically get saved 
	/// inside playerPrefs and then get read when the game starts.
	///*************************************************************************///

	public GameObject label;				//reference to child gameObject
	public int levelID;						//unique level identifier. Starts from 1.

	public int levelPrize = 150;			//prize (money) given to player if level is finished successfully
	public int careerGoalBallance = 1500;	//mission goal
	public int careerAvailableTime = 300;	//mission time
	public bool canUseCandy = true;			//are we allowed to use candy
	public int[] availableProducts;			//array of indexes of available products. starts from 1.


	void Start (){
		
		if(CareerMapManager.userLevelAdvance >= levelID - 1) {
			//this level is open
			GetComponent<BoxCollider>().enabled = true;
			label.GetComponent<TextMesh>().text = levelID.ToString();
			GetComponent<Renderer>().material.color = new Color(1,1,1,1);

			//set heartbeat animation to active, if this is the newest opened level.
			if(CareerMapManager.userLevelAdvance == levelID - 1)
				GetComponent<HeartBeatAnimationEffect>().enabled = true;

		} else {
			//level is locked
			GetComponent<BoxCollider>().enabled = false;
			label.GetComponent<TextMesh>().text = "Locked";
			GetComponent<Renderer>().material.color = new Color(1,1,1,0.5f);

			//set heartbeat animation to inactive
			GetComponent<HeartBeatAnimationEffect>().enabled = false;
		}
	}
}