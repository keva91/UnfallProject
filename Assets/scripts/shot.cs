
using UnityEngine;
using System.Collections;

public class shot : MonoBehaviour{


   
	[HideInInspector]
	public Player playerFrom;
	public bool fakeshot = false;
	public bool alreadyshot = false;

	public GameObject explo;

	public float force1 = 20;
	void OnCollisionEnter(Collision collision)
	{
		if (!fakeshot) {
			
		
			var hit = collision.gameObject;
			Player playerhit = hit.GetComponent<Player> ();

			if (playerhit && !alreadyshot && playerhit.pseudo != playerFrom.pseudo) {


				var dir = (collision.transform.position - playerFrom.transform.position);
				GameObject myExplo = Instantiate (explo, transform.position, transform.rotation);

			
				NetworkManager.instance.GetComponent<NetworkManager>().CommandHit(playerhit.pseudo,dir*10);
				playerFrom.increaseScore ();
				this.alreadyshot = true;

				Debug.Log (playerFrom.pseudo + " hit " + playerhit.pseudo);
				Destroy (gameObject, 0.03f);
				Destroy (myExplo, 0.8f);

			}
		}

		Destroy(gameObject,0.2f);
	}


	void Start () {
		Destroy(gameObject,2f);
	}
}