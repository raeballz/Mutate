using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour {

	public Transform [] backgrounds; 
	private float[] parallaxScales; //proportion of movement to background distances
	public float smoothing = 1f;	

	private Transform cam;			//current main cam pos
	private Vector3 previousCamPos; //position of cam in previous frame
	 
	//Called pre start. Used for reference.
	void Awake() {
		cam = Camera.main.transform;
	}
	// Use this for initialization
	void Start () {
		previousCamPos = cam.position;

		parallaxScales = new float[backgrounds.Length];
		for(int i = 0; i < backgrounds.Length;i++)
		{
			parallaxScales[i] = backgrounds [i].position.z*-1;
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < backgrounds.Length; i++) {
			//the parallax is the opposite of camera movement. Previous frame cam position multiplied by distance between layers.
			float parallax = (previousCamPos.x - cam.position.x)*parallaxScales[i];
		
			float backgroundTargetPosX = backgrounds[i].position.x+parallax;

			// create a target position which is the background's current position with it's target x position
			Vector3 backgroundTargetPos = new Vector3 (backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);
			
			backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);


		}
		
		previousCamPos = cam.position;
	}
}
