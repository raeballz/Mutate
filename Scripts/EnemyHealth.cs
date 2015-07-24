using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {
	public float hitPoints = 20; 
	public bool flyingEnemy;
	public bool acidEnemy;
	public bool speedEnemy;

	private PlayerModel _playerModel; 

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (this.hitPoints <= 0) {
			Destroy (this.gameObject);
			_playerModel = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerModel>();
			if(flyingEnemy){
			float newFlyingKills = _playerModel.playerStats.flyingKills + 1;
			_playerModel.playerStats.flyingKills = newFlyingKills;
			}
			if(acidEnemy){
				float newAcidKills= _playerModel.playerStats.acidKills + 1;
				_playerModel.playerStats.acidKills = newAcidKills;
			}
			if(speedEnemy){
				float newSpeedKills= _playerModel.playerStats.speedKills + 1;
				_playerModel.playerStats.flyingKills = newSpeedKills;
			}
		}
	}

	public void SetHitPoints(float damage)
	{
		hitPoints = (hitPoints - damage);
	}
}
