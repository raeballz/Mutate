using UnityEngine;
using System.Collections;

public class DamageZone : MonoBehaviour {
	public float damage;
	private PlayerModel player;
	
	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerModel>();
			player.DamagePlayer(damage);
		}
	}
}
