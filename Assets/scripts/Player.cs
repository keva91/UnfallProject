using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
using System;
using UnityStandardAssets.Characters.FirstPerson;



public class Player : MonoBehaviour {

	public bool isLocalPlayer = false; //TODO networking
	public int highScore;
	public int score = 0;
	public string pseudo;
	public int id;


	public Rigidbody bulletCasing;
	public CharacterController controller;

	public int ejectSpeed = 100;
	public double fireRate = 0.5;
	private double nextFire = 0.0;
	public Vector3 shotVelocity;

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
			if (cam.enabled){
				cam.enabled = false;
				ctrl.enabled = false;
			}
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

		if (currentPosition.y < -3) {
			respawn();
		}

		if (Input.GetButton("Fire1") && Time.time > nextFire) {
			Vector3 fveloc = new Vector3();
			CmdFire(false,fveloc);
			NetworkManager.instance.GetComponent<NetworkManager>().CommandShoot(shotVelocity);
		}




    }

	public void respawn() {
		transform.position =  new Vector3(0,5,0);

	}

	public void CmdFire(bool fakeshot,Vector3 veloc) {
		/*var bullet1 = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation) as GameObject;

		Bullet b = bullet1.GetComponent<Bullet>();

		bullet1.GetComponent<Rigidbody>().velocity = bullet1.transform.up * 6;*/


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
