using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
using System;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;



public class Player : MonoBehaviour {

	public bool isLocalPlayer = false; //TODO networking

	[System.NonSerialized]
	public int currentScore = 0;
	public Text currentScorehud;
	public int Hscore;
	public  Text Hscorehud;
	public string pseudo;
	public int id;


	public Rigidbody bulletCasing;
	public CharacterController controller;

	public int ejectSpeed = 100;
	public double fireRate = 0.5;
	private double nextFire = 0.0;
	public Vector3 shotVelocity;
	public GameObject myArm; 

	Vector3 oldPosition;
	public Vector3 currentPosition;
	Quaternion oldRotation;
	public Quaternion currentRotation;
	Camera cam;
	FirstPersonController ctrl;

    // Use this for initialization
    void Start () {

		cam = GetComponentInChildren<Camera>();
		ctrl = GetComponent<FirstPersonController>();
		controller = GetComponent<CharacterController>();
		oldPosition = transform.position;
		currentPosition = oldPosition;
		oldRotation = transform.rotation;
		currentRotation = oldRotation;


		if (isLocalPlayer) {
			this.currentScorehud.text = "score :";
			//myArm = this.transform.Find("arm").gameObject;
		}

    }


    void Update()
    {
     
		if (isLocalPlayer)
		{
			if (!cam.enabled) {
				cam.enabled = true;
				ctrl.enabled = true;
			}
		}else{
			
			return;
		}

		currentPosition = transform.position;
		currentRotation = transform.rotation;

		if(currentPosition != oldPosition){
			//Debug.Log("changement pos");
			oldPosition = currentPosition;
			NetworkManager.instance.GetComponent<NetworkManager>().CommandMove(transform.position);
		}

		if(currentRotation != oldRotation){
			//Debug.Log("changement rotation");
			oldRotation = currentRotation;
			 
			NetworkManager.instance.GetComponent<NetworkManager>().CommandTurn(transform.rotation);
		}

		if (currentPosition.y < -8) {
			respawn();
		}

		if (Input.GetButton("Fire1") && Time.time > nextFire) {
			Vector3 fveloc = new Vector3();
			CmdFire(false,fveloc);
			NetworkManager.instance.GetComponent<NetworkManager>().CommandShoot(shotVelocity);
			myArm.transform.localPosition  = new Vector3(myArm.transform.localPosition .x, myArm.transform.localPosition .y,0.3198293f);
		}

		if (myArm && myArm.transform.localPosition.z < 0.61f){
			// consumes recoil
			myArm.transform.localPosition = Vector3.Lerp(myArm.transform.localPosition, new Vector3(myArm.transform.localPosition.x, myArm.transform.localPosition.y, 0.61982f), 5*Time.deltaTime);
		}



    }

	public void respawn() {
		transform.position =  new Vector3(0,5,0);
		Debug.Log (this.currentScore);
		Debug.Log (this.Hscore);
		if(this.currentScore > this.Hscore){
			this.Hscore = this.currentScore;
			this.Hscorehud.text = "HighScore : "+ this.currentScore;
			NetworkManager.instance.GetComponent<NetworkManager>().UpdateHighScore(this);
			Debug.Log (" call send highscore");
		}
		this.currentScore = 0;
		this.currentScorehud.text = " score : 0";
		NetworkManager.instance.GetComponent<NetworkManager>().UpdateScore(this);
	}

	public void increaseScore() {
		this.currentScore +=  10;
		this.currentScorehud.text = "score : " + this.currentScore;
		NetworkManager.instance.GetComponent<NetworkManager>().UpdateScore(this);
		Debug.Log (" increase score");
	}

	public void CmdFire(bool fakeshot,Vector3 veloc) {

		nextFire = Time.time + fireRate;

		Transform case2 = this.transform.Find("FirstPersonCharacter");
		Transform case3 = case2.transform.Find("arm");
		Transform case4 = case3.transform.Find("bulletCase"); 

		Rigidbody bullet = Instantiate(bulletCasing, case4.position, case4.rotation);
		if (fakeshot) {
			bullet.velocity = veloc;
			shot b = bullet.GetComponent<shot>();
			b.playerFrom = this;
			if(fakeshot){
				b.fakeshot = true;
			}
		} else {
			bullet.velocity = case4.TransformDirection(Vector3.left)* ejectSpeed;
			shotVelocity = bullet.velocity;
			shot b = bullet.GetComponent<shot>();
			b.playerFrom = this;
			if(fakeshot){
				b.fakeshot = true;
			}
		}
			
		Destroy(bullet, 2.0f);

	}




    
}
