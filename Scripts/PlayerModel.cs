using UnityEngine;
using System;

public class PlayerModel : MonoBehaviour {
	[System.Serializable]
	public class PlayerStats
	{
		public float Health = 100f;
		public float maxSpeed = 25f;
		public float acceleration = 60f;
		public float jumpSpeed = 9f;
		public float jumpDuration = 0.01f;
		public float DNALevel = 0;
		public float flyingKills = 0;
		public float acidKills = 0;
		public float speedKills = 0;
	}

	public PlayerStats playerStats = new PlayerStats();
	public int fallBoundary = -20;

	void Update(){
		if (transform.position.y < fallBoundary) { //if player falls below screen, then apply infinite dmamge.
			DamagePlayer (99999999);
		}
		playerStats.DNALevel = (playerStats.flyingKills + playerStats.acidKills + playerStats.speedKills);
		if (playerStats.DNALevel == 10) {
			playerStats.DNALevel = 0;

			if (playerStats.flyingKills > 3) {
				Debug.Log ("Player has mutated jump height");
				playerStats.jumpSpeed = playerStats.jumpSpeed + 2f;

				if (playerStats.flyingKills > 5) {
					Debug.Log ("Player has advanced jump height mutation");
					playerStats.jumpSpeed = playerStats.jumpSpeed + 3f;
				}

				playerStats.flyingKills = 0;
			
			}
		
			if (playerStats.speedKills > 3) {
				Debug.Log ("Player has mutated speed"); 
				playerStats.maxSpeed = playerStats.maxSpeed + 20f;
				playerStats.acceleration = playerStats.acceleration + 40f;
				if (playerStats.flyingKills > 5) 
				{
					Debug.Log ("Player has advanced speed mutation");
					playerStats.maxSpeed = playerStats.maxSpeed + 10f;
				}
				playerStats.speedKills = 0;

			}
			if (playerStats.acidKills > 3) {
				Debug.Log ("Player has mutated health"); 
				playerStats.Health = playerStats.Health + 20f;
				if (playerStats.acidKills > 5) 
				{
					Debug.Log ("Player has advanced health mutation");
					playerStats.Health = playerStats.Health + 50f;
				}

				playerStats.acidKills = 0;
			}
		}
	}

	public void DamagePlayer(float damage){
		playerStats.Health -= damage;
		if (playerStats.Health <= 0) {
			GameMaster.KillPlayer(this);	//"this" is the player in this instance. Could also be Game object as it's what the script is attached to.
		}
	}

	private int SelectRandomMutation()
	{	
		return (int)(UnityEngine.Random.Range(0,2));
		
	}
}
