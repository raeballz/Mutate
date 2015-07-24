using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public float fireRate = 0f; //If 0, singleshot weapon. Else automatic fire rate is determinted by this
	public float damage = 10f;
	public LayerMask whatToHit; //Used for raytrace selection

	public Transform BulletTrailPrefab;
	public Transform MuzzleFlashPrefab;
	float timeToSpawnEffect = 0f;
	public float effectSpawnRate = 10f;

	float timeToFire = 0f; //Used to calculate when next shot will be fired on automatic weapons
	Transform firePoint;
	EnemyHealth enemyHealth;

	void Awake(){
		firePoint = transform.FindChild ("firePoint"); //Get firing point from player prefab.
		if (firePoint == null) {
			Debug.LogError ("No firePoint");
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (fireRate == 0) {
			if (Input.GetButtonDown ("Fire1")) {
				Shoot();
			}
		}
		else {
			if (Input.GetButton ("Fire1") && Time.time > timeToFire) {
				timeToFire = Time.time + 1/fireRate;
				Shoot();
			}
		}
	}
	void Shoot(){
		Debug.Log ("Weapon Fired");
		Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y); 
		Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast (firePointPosition, mousePosition-firePointPosition, 100, whatToHit); //Draw raycast from fire point on model (tip of gun) to mouse position with 100 length
		//Debug.DrawLine (firePointPosition, (mousePosition-firePointPosition)*100, Color.cyan); 
		if (Time.time >= timeToSpawnEffect) {
			StartCoroutine("Effect");
			timeToSpawnEffect = Time.time + 1/effectSpawnRate;
		}
		if (hit.collider != null) {
			//Debug.DrawLine (firePointPosition, hit.point, Color.red); Red to denote a successful hit
			Debug.Log ("We hit " + hit.collider.name + " and did " + damage + " damage.");
			enemyHealth = hit.collider.GetComponent<EnemyHealth>();
			enemyHealth.SetHitPoints(damage);
		}
	}

	
	IEnumerator Effect () {
		Instantiate (BulletTrailPrefab, firePoint.position, firePoint.rotation);
		Transform clone = (Transform) Instantiate (MuzzleFlashPrefab, firePoint.position, firePoint.rotation); //Create a clone of the muzzleflash as a transform so we can move it.
		clone.parent = firePoint;
		float size = Random.Range (0.6f, 0.9f); //Randomise the size for pizzaz
		clone.localScale = new Vector3 (size, size, size);
		yield return 0;
		Destroy (clone.gameObject, 0.0f);
	}
}