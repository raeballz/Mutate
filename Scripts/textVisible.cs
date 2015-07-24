using UnityEngine;
using System.Collections;

public class textVisible : MonoBehaviour {
	public GameObject myText;   // Assign the text to this in the inspector

	// Use this for initialization
	void Start () {
		myText.SetActive( false );
	}
	
	// Update is called once per frame
	void Update () {
	}

	IEnumerator displayText(){
			yield return new WaitForSeconds(2);
			myText.SetActive( true ); // Enable the text so it shows
			yield return new WaitForSeconds(2);
			myText.SetActive( false ); // Disable the text so it is hidden
	}
}
