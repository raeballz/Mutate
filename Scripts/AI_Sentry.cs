using UnityEngine;
using System.Collections;

public class AI_Sentry : MonoBehaviour {
	public Transform scanStart;
	public Transform scanEnd;
	public Transform acidPrefab;
	public float timeToFire;

	public LayerMask whatToHit; //Used for raytrace selection
	private Vector3 pointAPos;
	private Vector3 pointBPos;
	private Transform firePoint;

	private float timeToSpawnEffect;
	public float effectSpawnRate = 10f;

	// Use this for initialization
	void Start (){
		pointAPos = scanStart.transform.position;
		pointBPos = scanEnd.transform.position;
		firePoint = transform.FindChild ("FirePoint"); //Get firing point from player prefab.
		timeToSpawnEffect = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit2D hit = Physics2D.Raycast (pointAPos, pointBPos, scanEnd.transform.position.x - scanStart.transform.position.x, whatToHit); //Draw line between two "nodes"
		//Debug.DrawLine (pointAPos, pointBPos, Color.green); To see if pointA/B referenced correctly
		if (hit && Time.time > timeToSpawnEffect) { //if player trips the wire and the scorpion is ok to fire again 
			Debug.Log("Enemy hit player");
			Instantiate (acidPrefab, firePoint.position, firePoint.rotation); //Create a new projectile @ fire point
			timeToSpawnEffect = Time.time + 1/effectSpawnRate;
		}
	}
}
