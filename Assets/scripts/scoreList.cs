using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class scoreList : MonoBehaviour {




	public GameObject scoreItemCstr;

	ScoreManager scoreM;


	void Start(){
		//scoreM = GameObject.FindObjectOfType<ScoreManager> ();
		//NetworkManager.instance.GetComponent<ScoreManager>()
	}



	public void changeHighScoreList(string array){
		items list = JsonUtility.FromJson<items>("{\"itemList\": " + array + "}");

		while(this.transform.childCount > 0){
			Transform C = this.transform.GetChild (0);
			C.SetParent (null);
			Destroy (C.gameObject);
		}

		foreach(item item in list.itemList){
			
			GameObject go = (GameObject)Instantiate(scoreItemCstr);
			go.transform.SetParent(this.transform);
			go.transform.localRotation = Quaternion.identity;
			go.transform.localPosition = Vector3.zero;
			go.transform.localScale = Vector3.one;
			go.transform.Find("pseudo").GetComponent<Text>().text = item.pseudo;
			go.transform.Find("score").GetComponent<Text>().text = item.score.ToString();

		}
	}

	public void changeScoreList(NetworkManager.ScoreJSON score){
		//items list = JsonUtility.FromJson<items>("{\"itemList\": " + array + "}");

		if(!scoreM){
			scoreM = GameObject.FindObjectOfType<ScoreManager> ();
			Debug.Log (scoreM);
			Debug.Log (NetworkManager.instance.GetComponent<ScoreManager>());
		}
		scoreM.setScore(score.pseudo,score.score);


		while(this.transform.childCount > 0){
			Transform C = this.transform.GetChild (0);
			C.SetParent (null);
			Destroy (C.gameObject);
		}


		string[] names = scoreM.getPlayerNames ();

		for(var i=0;i < 8 && i < names.Length ;i++){

			Debug.Log (names.Length);
			Debug.Log (i);
			GameObject go = (GameObject)Instantiate(scoreItemCstr);
			go.transform.SetParent(this.transform);
			go.transform.localRotation = Quaternion.identity;
			go.transform.localPosition = Vector3.zero;
			go.transform.localScale = Vector3.one;
			go.transform.Find("pseudo").GetComponent<Text>().text = names[i];
			go.transform.Find("score").GetComponent<Text>().text = scoreM.getScore(names[i]).ToString();

		}
	}


	public void deleteScoreList(string pseudo){
		//items list = JsonUtility.FromJson<items>("{\"itemList\": " + array + "}");

		if(!scoreM){
			scoreM = GameObject.FindObjectOfType<ScoreManager> ();
			Debug.Log (scoreM);
			Debug.Log (NetworkManager.instance.GetComponent<ScoreManager>());
		}
		scoreM.deleteScore(pseudo);


		while(this.transform.childCount > 0){
			Transform C = this.transform.GetChild (0);
			C.SetParent (null);
			Destroy (C.gameObject);
		}


		string[] names = scoreM.getPlayerNames ();

		for(var i=0;i < 8 && i < names.Length ;i++){

			Debug.Log (names.Length);
			Debug.Log (i);
			GameObject go = (GameObject)Instantiate(scoreItemCstr);
			go.transform.SetParent(this.transform);
			go.transform.localRotation = Quaternion.identity;
			go.transform.localPosition = Vector3.zero;
			go.transform.localScale = Vector3.one;
			go.transform.Find("pseudo").GetComponent<Text>().text = names[i];
			go.transform.Find("score").GetComponent<Text>().text = scoreM.getScore(names[i]).ToString();

		}
	}




	[System.Serializable]
	public struct item 
	{
		public int id;
		public string pseudo;
		public int score;
	}
	[System.Serializable]
	public class items 
	{
		public List<item> itemList;
	}
}
