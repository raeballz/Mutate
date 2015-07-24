using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	public void QuitGame() {
		Application.Quit(); //Simple quit
	}

	public void StartGame(string level){
		Application.LoadLevel(level);//Takes a string so I can assign it what ever level I'd like.
	}

}
