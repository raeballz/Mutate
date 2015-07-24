using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;

	void Start(){
	if (gm == null) {
			gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>(); //if gm object is empty, set it to current game object as tagged "GM"
		}
	}

	public Transform Player;
	public Transform playerPrefab;
	public Transform finishParticles;

	public Transform spawnPoint;
	public float spawnDelay = 3.5f;
	public Transform spawnPrefab;

	public Transform levelFinish;

	public IEnumerator RespawnPlayer(){
		GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(spawnDelay);
		Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation);
		GameObject clone = Instantiate (spawnPrefab, spawnPoint.position, spawnPoint.rotation) as GameObject; //create a clone of particle system
		Destroy (clone, 3f);//Delete clone so game doesn't overload with empty particle systems
	}

	public static void KillPlayer(PlayerModel player){ 		//Method takes player object to be killed
		Destroy (player.gameObject); 					//.gameObject is to prevent errors. Unity expects it, just do it.
		gm.StartCoroutine(gm.RespawnPlayer());
	}
	
	public void selectPlayerPrefab(){

	}
}
