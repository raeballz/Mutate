using UnityEngine;
using System.Collections;

public class DNABar : MonoBehaviour {
	public RectTransform rectTransform;
	private PlayerModel player; 
	private float DNA;
	private float scale = 0f;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerModel>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (player == null) {
			player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerModel>();
		}
		DNA = player.playerStats.DNALevel;
		//Debug.Log (health);
		scale = (DNA / 10);
		//Debug.Log (scale);
		rectTransform.localScale = new Vector3 (rectTransform.localScale.x, scale, rectTransform.localScale.z);
	}

	public void AdjustScale(float adj)
	{
		if (adj < 0)
			adj = 0;
		else if (adj > 1)
			adj = 1;
		scale = adj;
	}
}
