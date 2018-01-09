using System.Collections;
using UnityEngine;
using UnitySocketIO;
using UnitySocketIO.Events;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;

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
	public string localName;


	void Awake()
	{
		if (instance == null) {
			instance = this;
		}else if (instance != this){
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}



	void Start() {

		errorLogin.gameObject.SetActive(false);
		errorNewAccount.gameObject.SetActive(false);


		io.On("connect", (SocketIOEvent e) => {
			Debug.Log("SocketIO connected");
		

			TestObject t = new TestObject();
			t.test = 123;
			t.test2 = "test1";



		});

		io.Connect();


		io.On("other player connected", OnOtherPlayerConnected);
		io.On("other player disconnected", OnOtherPlayerDisconnect);

		io.On("player move", OnPlayerMove);
		io.On("player turn", OnPlayerTurn);
		io.On("player shoot", OnPlayerShoot);
		io.On("touch", OnTouch);



	}

	public void Launch(object data)
	{
		StartCoroutine(JoinTheGame(data));
	}


	IEnumerator JoinTheGame(object data)
	{
		Debug.Log("JoinGame");
		Debug.Log(data);


		yield return new WaitForSeconds(0.3f);

		io.Emit("player connect");

		yield return new WaitForSeconds(0.5f);

		UserJSON user = JsonUtility.FromJson<UserJSON>(data.ToString());


		canvas.gameObject.SetActive(false);
		c1.gameObject.SetActive(false);
		Vector3 position = new Vector3(0,5,0);
		Quaternion rotation = Quaternion.Euler(0,0,0);
		GameObject playerCstr = Instantiate(player, position, rotation) as GameObject;



		Player pC = playerCstr.GetComponent<Player>();
		pC.isLocalPlayer = true;
		pC.pseudo = user.pseudo;
		pC.score = user.score;
		pC.currentPosition = position;
		pC.currentRotation = rotation;

		string playerAsJson = PlayerJSON.CreateToJSON(pC);


		io.Emit("play", playerAsJson);

		Transform t = playerCstr.transform.Find("Canvas");
		Transform t1 = t.transform.Find("pseudo");
		Text playerName = t1.GetComponent<Text>();

		playerName.text = pC.pseudo;
		playerCstr.name = pC.pseudo;
		localName = pC.pseudo;
		Debug.Log(playerName.text);

	}


	//fierce-stream-59902.herokuapp.com

	public void UpdateScore(Player data){
		
	
		Debug.Log (" before send highscore");
		string myData = data.ToString();
		byte[] myfData = System.Text.Encoding.UTF8.GetBytes(myData);
		UnityWebRequest www = UnityWebRequest.Put("http://www.my-server.com/upload", myfData);

	 	StartCoroutine(OnScoreResponse(www));
	
	}

	private IEnumerator OnScoreResponse(UnityWebRequest req){
		yield return req.Send();
		Debug.Log (" after send highscore");
		if(req.isNetworkError || req.isHttpError) {
			Debug.Log(req.error);
		}
		else {
			Debug.Log("Upload complete!");
			Debug.Log(req);
		}
	}


	public void NewAccount(){
		var from = "NewAccount";


		if (playerNameInputNewAccount.text != "") {
			WWWForm form = new WWWForm();

			form.AddField("pseudo", playerNameInputNewAccount.text);

			byte[] rawFormData = form.data;

			WWW request = new WWW("https://fierce-stream-59902.herokuapp.com/user",rawFormData);
			StartCoroutine(OnResponse(request,from));
		}
	}


	public void Login(){
		var from = "login";
		var pseudo = playerNameInputLogin.text;
		Debug.Log (pseudo);

		if (pseudo != ""){
			WWW request = new WWW("https://fierce-stream-59902.herokuapp.com/user/"+pseudo);
			StartCoroutine(OnResponse(request,from));
		}

	}


	private IEnumerator OnResponse(WWW req,string from){
		yield return req;
		Debug.Log (req.text);
		ErrorJSON res = JsonUtility.FromJson<ErrorJSON>(req.text.ToString());
	
			
		if (from == "login") { 

			if (res.erreur == null) {
				Debug.Log ("login successfull");
				errorLogin.gameObject.SetActive(false);
				Launch(req.text);
			} else {
				Debug.Log ("mauvais pseudo ");
				//afficher erreur
				errorLogin.gameObject.SetActive(true);
			}

		} else {

			if (res.erreur == null) {
				Debug.Log ("register successfull");
				errorNewAccount.gameObject.SetActive(false);
				Launch(req.text);
			} else {
				Debug.Log (" pseudo deja pris");
				//afficher erreur
				errorNewAccount.gameObject.SetActive(true);
			}
		}
	}



	public void OnOtherPlayerConnected(SocketIOEvent socketIOEvent){
		
		string data = socketIOEvent.data.ToString();
		Debug.Log (data);

		PlayerJSON otherplayerJSON = PlayerJSON.CreateFromJSON(data);
		Debug.Log ("other player "+otherplayerJSON.pseudo+" is connected");
		Vector3 position = new Vector3(otherplayerJSON.position[0], otherplayerJSON.position[1], otherplayerJSON.position[2]);
		Quaternion rotation = Quaternion.Euler(otherplayerJSON.rotation[0], otherplayerJSON.rotation[1], otherplayerJSON.rotation[2]);
		GameObject o = GameObject.Find(otherplayerJSON.pseudo) as GameObject;

		if (o != null)
		{
			Debug.Log (o);
			Debug.Log ("this player exist already");
			return;
		}
		Debug.Log ("this player is new ");
		GameObject op = Instantiate(player, position, rotation) as GameObject;
		// here we are setting up their other fields name and if they are local

		Player opC = op.GetComponent<Player>();

		opC.pseudo = otherplayerJSON.pseudo;
		opC.score = otherplayerJSON.score;

		//var testr = op.GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController> ();

		//Debug.Log(testr);

		//testr.enabled = false;
		Transform ot = op.transform.Find("Canvas");
		Transform ot1 = ot.transform.Find("pseudo");
		Text oplayerName = ot1.GetComponent<Text>();
		oplayerName.text = otherplayerJSON.pseudo;
		opC.isLocalPlayer = false;
		op.name = otherplayerJSON.pseudo;
		Debug.Log(opC.pseudo);
		Debug.Log(oplayerName.text);

	}

	public void OnOtherPlayerDisconnect(SocketIOEvent socketIOEvent){
		
		string data = socketIOEvent.data.ToString();
		PlayerJSON otherplayerJSON = PlayerJSON.CreateFromJSON(data);
		Debug.Log (otherplayerJSON.pseudo);
		Debug.Log (" want to disconnect");
		GameObject test = GameObject.Find(otherplayerJSON.pseudo) as GameObject;

		if (test != null) {
			Debug.Log ("this player exist and want to disconnect");
		} else {
			Debug.Log ("this player doesnt exist and want to disconnect");
		}

		Destroy(GameObject.Find(otherplayerJSON.pseudo));
	}


	public void OnPlayerMove(SocketIOEvent socketIOEvent){
		string data = socketIOEvent.data.ToString();
		PlayerJSON otherplayerJSON = PlayerJSON.CreateFromJSON(data);
		Debug.Log ("other player "+otherplayerJSON.pseudo+" want to move");
		Vector3 position = new Vector3(otherplayerJSON.position[0], otherplayerJSON.position[1], otherplayerJSON.position[2]);
		// if it is the current player exit
		if (otherplayerJSON.pseudo == playerNameInputLogin.text || otherplayerJSON.pseudo == playerNameInputNewAccount.text )
		{
			Debug.Log ("pas normal");
			return;
		}
		GameObject op = GameObject.Find(otherplayerJSON.pseudo) as GameObject;
		if (op != null) {
			Debug.Log (op);
			Debug.Log ("this other player  moved");
			op.transform.position = position;
		} else {
			Debug.Log ("this otherplayer  want to move but doesnt exist");
			Debug.Log (op);
		}
	}


	void OnPlayerTurn(SocketIOEvent socketIOEvent){
		string data = socketIOEvent.data.ToString(); 
		PlayerJSON otherplayerJSON = PlayerJSON.CreateFromJSON(data);
		Quaternion rotation = Quaternion.Euler(otherplayerJSON.rotation[0], otherplayerJSON.rotation[1], otherplayerJSON.rotation[2]);
		// if it is the current player exit
		if (otherplayerJSON.pseudo == playerNameInputLogin.text || otherplayerJSON.pseudo == playerNameInputNewAccount.text)
		{
			return;
		}
		GameObject p = GameObject.Find(otherplayerJSON.pseudo) as GameObject;
		if (p != null)
		{
			p.transform.rotation = rotation;
			Transform cp = p.transform.Find("FirstPersonCharacter");
			if (cp != null) {
				cp.transform.rotation = rotation;
				Debug.Log (" cp exist");
				Debug.Log (rotation);
				Debug.Log (rotation.x);
				Debug.Log (cp.transform.rotation);
			} else {
				Debug.Log (" cp fail");
			}
				
		}
	}

	void OnPlayerShoot(SocketIOEvent socketIOEvent){
		string data = socketIOEvent.data.ToString();
		ShootJSON shootJSON = ShootJSON.CreateFromJSON(data);
		Vector3 velocityshoot = new Vector3(shootJSON.position[0], shootJSON.position[1], shootJSON.position[2]);
		//find the gameobject
		GameObject p = GameObject.Find(shootJSON.pseudo);
		Debug.Log (shootJSON.pseudo+" has shot");
		// instantiate the bullet etc from the player script
		Player pc = p.GetComponent<Player>();
		pc.CmdFire(true,velocityshoot);
		Debug.Log (shootJSON.pseudo+" has shot");
	}



	void OnTouch(SocketIOEvent socketIOEvent){
		Debug.Log (" i got shot");
		string data = socketIOEvent.data.ToString();
		HitJSON touch = HitJSON.CreateFromJSON(data);
		Vector3 position = new Vector3(touch.position[0], touch.position[1], touch.position[2]);

		if(localName == touch.pseudo){
			//find the gameobject
			GameObject p = GameObject.Find(localName);
			if(p != null){
				ImpactReceiver script = p.GetComponent<ImpactReceiver> ();
				if (script) {
					script.AddImpact (position * 2);
					Debug.Log (" i have been shot");
				} 
			}
		}

			
	}







	public void CommandMove(Vector3 vec3){
		string data = JsonUtility.ToJson(new PositionJSON(vec3));
		io.Emit("player move", data);
	}


	public void CommandTurn(Quaternion quat){
		string data = JsonUtility.ToJson(new RotationJSON(quat));
		io.Emit("player turn", data);
		Debug.Log ("i tell my rotation");
	}

	public void CommandShoot(Vector3 vel){
		string data = JsonUtility.ToJson(new PositionJSON(vel));
		io.Emit("player shoot",data);
		Debug.Log ("i tell my shot");
	}

	public void CommandHit(string pseudo,Vector3 force){

		string data = JsonUtility.ToJson(new HitJSON(pseudo,force));
		io.Emit("player hit",data);
		Debug.Log ("i tell my shot");
	}





	[Serializable]
	public class UserJSON{
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
	public class  PlayerJSON{
		public int id;
		public string pseudo;
		public int score;
		public float[] position;
		public float[] rotation;

		public static PlayerJSON CreateFromJSON(string data)
		{
			return JsonUtility.FromJson<PlayerJSON>(data);
		}

		public static string CreateToJSON(Player data)
		{
			PlayerJSON playerjson = new PlayerJSON();
			playerjson.id = data.id;
			playerjson.pseudo = data.pseudo;
			playerjson.score = data.score;
			playerjson.position = new float[] { data.currentPosition.x, data.currentPosition.y, data.currentPosition.z };
			playerjson.rotation = new float[] { data.currentRotation.eulerAngles.x,data.currentRotation.eulerAngles.y,data.currentRotation.eulerAngles.z };
			return JsonUtility.ToJson(playerjson);
		}
	}


	[Serializable]
	public class PositionJSON{
		public float[] position;

		public PositionJSON(Vector3 _position)
		{
			position = new float[] { _position.x, _position.y, _position.z };
		}


			
	}



	[Serializable]
	public class RotationJSON{
		public float[] rotation;

		public RotationJSON(Quaternion _rotation)
		{
			rotation = new float[] { _rotation.eulerAngles.x,_rotation.eulerAngles.y, _rotation.eulerAngles.z };
		}
	}


	[Serializable]
	public class ShootJSON{
		public string pseudo;
		public float[] position;

		public static ShootJSON CreateFromJSON(string data)
		{
			return JsonUtility.FromJson<ShootJSON>(data);
		}
	}


	[Serializable]
	public class HitJSON{
		public string pseudo;
		public float[] position;

		public  HitJSON(string pseudo,Vector3 position)
		{
			this.pseudo = pseudo;
			this.position = new float[] { position.x, position.y, position.z };
			
		}

		public static HitJSON CreateFromJSON(string data)
		{
			return JsonUtility.FromJson<HitJSON>(data);
		}


	}


	[Serializable]
	public class ErrorJSON{
		public string erreur;


	}




}
