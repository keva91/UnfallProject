using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactReceiver : MonoBehaviour {

	public float mass = 3.0f; // defines the character mass
	public Vector3 impact = Vector3.zero;

	//public float hitForce = 20f;
	private CharacterController character;

	void Start(){
		character = GetComponent<CharacterController>();
	}



	void Update(){
		// apply the impact force:
		if (impact.magnitude > 0.2){
			character.Move(impact * Time.deltaTime);
			Debug.Log ("after move");
		}
		// consumes the impact energy each cycle:
		impact = Vector3.Lerp(impact, Vector3.zero, 5*Time.deltaTime);
	}


	public void AddImpact(Vector3 direction){
		var dir = direction.normalized;
		dir.y = 0.5f; // add some velocity upwards - it's cooler this way
		impact += dir.normalized * direction.magnitude / mass;
		if(impact.magnitude < 28.0f){
			impact *= 3.0f;
			Debug.Log ("low magnitude :"+impact.magnitude);
		}
		if(impact.magnitude > 100.0f){
			impact = impact/1.5f;
		}
		Debug.Log (impact.magnitude);

	}

}
