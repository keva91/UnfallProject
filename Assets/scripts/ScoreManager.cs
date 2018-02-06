using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScoreManager : MonoBehaviour {




	Dictionary<string,int> playersScores;


	// Use this for initialization
	void Start () {
		
		
	}


	void init(){
		if (playersScores != null) {
			return;
		} else {
			playersScores = new Dictionary<string, int>();
		}
	}


	public int getScore(string pseudo){
		init ();
		if(playersScores.ContainsKey(pseudo) == false){
			return 0;
		}

		return playersScores [pseudo];
	}

	public void setScore(string pseudo,int score){
		init ();

		if(playersScores.ContainsKey(pseudo) == false){
			playersScores [pseudo] = 0;
		}
		playersScores [pseudo] = score;
	}


	public void deleteScore(string pseudo){
		init ();

		if(playersScores.ContainsKey(pseudo) == false){
			return;
		}
		playersScores.Remove(pseudo);
	}

	public string[] getPlayerNames(){
		init ();
		return playersScores.Keys.OrderByDescending (n => getScore (n)).ToArray ();
	}

	public void changeScore(string pseudo,int score){
		
	}

}
