using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

public class Player : MonoBehaviour {

	public bool isLocalPlayer = true; //TODO networking

	Vector3 oldPosition;
	Vector3 currentPosition;
	Quaternion oldRotation;
	Quaternion currentRotation;

    // Use this for initialization
    void Start () {
		oldPosition = transform.position;
		currentPosition = oldPosition;
		oldRotation = transform.rotation;
		currentRotation = oldRotation;
    }


    void Update()
    {
     
		if(!isLocalPlayer){
			return;
		}

		currentPosition = transform.position;
		currentRotation = transform.rotation;

		if(currentPosition != oldPosition){
			Debug.Log("changement pos");
			oldPosition = currentPosition;
		}

		if(currentRotation != oldRotation){
			Debug.Log("changement rotation");
			oldRotation = currentRotation;
		}

    }

    
}
