using UnityEngine;
using System.Collections;

public class MoveTrail : MonoBehaviour {

	public int moveSpeed = 230; //Can be changed in the editor.
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.right * Time.deltaTime * moveSpeed); //Simple movement 
		Destroy (this.gameObject, 1);
	}
}
