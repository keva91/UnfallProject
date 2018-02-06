using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvasCtrl : MonoBehaviour {


	Vector3 cameraPosition;
	void Start(){
		cameraPosition = new Vector3 (0,0,0);
		cameraPosition = Camera.main.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		if(cameraPosition != Camera.main.transform.position){
			Debug.Log("look at");
			cameraPosition = Camera.main.transform.position;
			this.transform.LookAt (Camera.main.transform);
		}

	
	}
}
