using System.Collections;
using UnityEngine;
using UnitySocketIO;
using UnitySocketIO.Events;
using UnityEngine.UI;
using System;

public class NetworkManager : MonoBehaviour {

	public static NetworkManager instance;
	public Canvas canvas;
	public Camera c1;
	public SocketIOController io;
	public InputField playerNameInputNewAccount;
	public InputField playerNameInputLogin;
	public Text errorNewAccount;
	public Text errorLogin;
	public GameObject player;



	void Start() {

		errorLogin.gameObject.SetActive(false);
		errorNewAccount.gameObject.SetActive(false);


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

		io.On("other player connected", OnOtherPlayerConnected);

	}

	public void Launch(object data)
	{
		StartCoroutine(JoinTheGame(data));
	}


	IEnumerator JoinTheGame(object data)
	{
		Debug.Log("JoinGame");
		Debug.Log(data);


		yield return new WaitForSeconds(0.5f);

		io.Emit("player connect");

		yield return new WaitForSeconds(1f);

		UserJSON P = JsonUtility.FromJson<UserJSON>(data.ToString());


		canvas.gameObject.SetActive(false);
		c1.gameObject.SetActive(false);
		Vector3 position = new Vector3(0,5,0);
		Quaternion rotation = Quaternion.Euler(0,0,0);
		GameObject playertest = Instantiate(player, position, rotation) as GameObject;



		Player pC = playertest.GetComponent<Player>();
		pC.isLocalPlayer = true;
		pC.pseudo = P.pseudo;
		pC.score = P.score;

		string playerAsJson = JsonUtility.ToJson(pC);
		io.Emit("play", playerAsJson);

		Transform t = player.transform.Find("Canvas");
		Debug.Log(t);
		Transform t1 = t.transform.Find("pseudo");
		Debug.Log(t1);
		Text playerName = t1.GetComponent<Text>();
		playerName.text = pC.pseudo;

	}

	public void NewAccount(){
		var from = "NewAccount";
		WWWForm form = new WWWForm();
		form.AddField("pseudo", playerNameInputNewAccount.text);

		byte[] rawFormData = form.data;
		
		WWW request = new WWW("http://localhost:3000/pseudo",rawFormData);
			StartCoroutine(OnResponse(request,from));
	}


	public void Login(){
		var from = "login";
		var pseudo = playerNameInputLogin.text;
		Debug.Log (pseudo);
		WWW request = new WWW("http://localhost:3000/pseudo/"+pseudo);
		StartCoroutine(OnResponse(request,from));
	}


	private IEnumerator OnResponse(WWW req,string from){
		yield return req;
		Debug.Log (req.text);
		if (from == "login") {

			if (req.text != "false") {
				Debug.Log ("login successfull");
				errorLogin.gameObject.SetActive(false);
				Launch(req.text);
			} else {
				Debug.Log ("mauvais pseudo ");
				//afficher erreur
				errorLogin.gameObject.SetActive(true);
			}

		} else {

			if (req.text != "false") {
				Debug.Log ("register successfull");
				errorNewAccount.gameObject.SetActive(false);
			} else {
				Debug.Log (" pseudo deja pris");
				//afficher erreur
				errorNewAccount.gameObject.SetActive(true);
			}
		}
	}



	void OnOtherPlayerConnected(SocketIOEvent socketIOEvent)
	{
		print("Someone else joined");
		Debug.Log (socketIOEvent.data);
		string data = socketIOEvent.data.ToString();
		Debug.Log (data);
		PlayerJSON playerJSON = PlayerJSON.CreateFromJSON(data);
		Vector3 position = new Vector3(playerJSON.position[0], playerJSON.position[1], playerJSON.position[2]);
		Quaternion rotation = Quaternion.Euler(playerJSON.rotation[0], playerJSON.rotation[1], playerJSON.rotation[2]);
		GameObject o = GameObject.Find(playerJSON.pseudo) as GameObject;
		if (o != null)
		{
			return;
		}
		GameObject p = Instantiate(player, position, rotation) as GameObject;
		// here we are setting up their other fields name and if they are local

		Player opC = p.GetComponent<Player>();

		opC.pseudo = playerJSON.pseudo;
		opC.score = playerJSON.score;

		Transform t = player.transform.Find("Canvas");
		Debug.Log(t);
		Transform t1 = t.transform.Find("pseudo");
		Debug.Log(t1);
		Text playerName = t1.GetComponent<Text>();
		playerName.text = playerJSON.pseudo;
		opC.isLocalPlayer = false;

	}








	[Serializable]
	public class UserJSON
	{
		public int id;
		public string pseudo;
		public int score;


		public UserJSON(int id, string pseudo,int score)
		{
			
			this.id = id;
			this.pseudo = pseudo;
			this.score = score;

		}
	}


	[Serializable]
	public class  PlayerJSON
	{
		public int id;
		public string pseudo;
		public int score;
		public float[] position;
		public float[] rotation;

		public static PlayerJSON CreateFromJSON(string data)
		{
			return JsonUtility.FromJson<PlayerJSON>(data);
		}
	}





}
