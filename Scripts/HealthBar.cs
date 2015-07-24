using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {


	public RectTransform rectTransform;
	private PlayerModel player; 
	private float health;
	private float scale = 0f;

	// Update is called once per frame

	void Start(){
		PlayerModel player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerModel>();
		float health = this.player.playerStats.Health;
	}

	void FixedUpdate () {
		if (player == null) {
			player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerModel>();
		}
		health = player.playerStats.Health;
		//Debug.Log (health);
		scale = (health / 100);
		//Debug.Log (scale);
		rectTransform.localScale = new Vector3 (rectTransform.localScale.x, scale, rectTransform.localScale.z);
	}


}
