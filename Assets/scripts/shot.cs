
using UnityEngine;
using System.Collections;

public class shot : MonoBehaviour{


   
	[HideInInspector]
	public Player playerFrom;
	public bool fakeshot = false;

	public float force1 = 20;
	void OnCollisionEnter(Collision collision)
	{
		if (!fakeshot) {
			
		
			var hit = collision.gameObject;
			Player playerhit = hit.GetComponent<Player> ();

			if (playerhit) {


				var dir = (collision.transform.position - playerFrom.transform.position);

				//hit.GetComponent<CharacterMotor>().SetVelocity(dir*force1);
				//.Knockback(dir);
				NetworkManager.instance.GetComponent<NetworkManager>().CommandHit(playerhit.pseudo,dir*2);

				/*ImpactReceiver script = hit.gameObject.GetComponent<ImpactReceiver> ();
				if (script) {
					script.AddImpact (dir * 2);
				} */

				Debug.Log (playerFrom.pseudo + " hit " + playerhit.pseudo);
				Destroy (gameObject, 0.1f);

			}
		}

		Destroy(gameObject,0.2f);
	}


	void Start () {
		Destroy(gameObject,2f);
	}
}