using System.Collections;
using UnityEngine;
using UnitySocketIO;
using UnitySocketIO.Events;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {

	public static NetworkManager instance;
	public Canvas canvas;
	public Camera c1;
	public SocketIOController io;
	public InputField playerNameInput;
	public GameObject player;



	void Start() {
		io.On("connect", (SocketIOEvent e) => {
			Debug.Log("SocketIO connected");
		

			TestObject t = new TestObject();
			t.test = 123;
			t.test2 = "test1";

			io.Emit("test-event2", JsonUtility.ToJson(t));

			TestObject t2 = new TestObject();
			t2.test = 1234;
			t2.test2 = "test2";

			io.Emit("test-event3", JsonUtility.ToJson(t2), (string data) => {
				Debug.Log(data);
			});

		});

		io.Connect();

		io.On("test-event", (SocketIOEvent e) => {
			Debug.Log(e.data);
		});
	}



	public void JoinTheGame()
	{
		Debug.Log("JoinGame");
		canvas.gameObject.SetActive(false);
		c1.gameObject.SetActive(false);
		Vector3 position = new Vector3(0,5,0);
		Quaternion rotation = Quaternion.Euler(0,0,0);
		GameObject p = Instantiate(player, position, rotation) as GameObject;

		Player pC = p.GetComponent<Player>();
		pC.isLocalPlayer = true;

		Transform t = p.transform.Find("Canvas");
		Debug.Log(t);
		Transform t1 = t.transform.Find("pseudo");
		Debug.Log(t1);
		Text playerName = t1.GetComponent<Text>();
		playerName.text = playerNameInput.text;

	}

	public void NewCount(){

		WWWForm form = new WWWForm();
		form.AddField("var1", playerNameInput.text);

		byte[] rawFormData = form.data;
		
		WWW request = new WWW("http://localhost:3000/user/pseudo",rawFormData);
			StartCoroutine(OnResponse(request));
	}


	private IEnumerator OnResponse(WWW req){
		yield return req;
		Debug.Log (req.text);
	}



}
