using UnityEngine;
using System.Collections;

public class NextLevelScript : MonoBehaviour {
	public string levelToLoad;

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
			other.transform.position = new Vector3(0,0,0);
			Application.LoadLevel(levelToLoad);
	}
}
