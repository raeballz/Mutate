using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SpriteRenderer))]		//This is to make sure there is always a sprite render attatched to prevent errors


public class Tiling : MonoBehaviour {

	public int offsetX = 2; 					//Offset to ensure overlap of foreground sprites
	public Transform parents;
	public bool hasRightBuddy = false; 			//Checks if sprite has a buddy sprite on it's right.
	public bool hasLeftBuddy = false;  			//'											' left.		

	public bool reverseScale = false;			//Used to tile untileable background sprites.

	private float spriteWidth = 0f;
	private Camera cam;
	private Transform myTransform;


	void Awake (){
		cam = Camera.main;
		myTransform = transform;
		SpriteRenderer sRenderer = GetComponent<SpriteRenderer> (); //grabs sprite renderer from engine
		spriteWidth = sRenderer.sprite.bounds.size.x;				//get width of screen
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (hasLeftBuddy ==false || hasRightBuddy ==false) { 	//Check 

			//get half the width of what the world can se in world coordinates.
			float camHorizontalExtend = cam.orthographicSize * Screen.width/Screen.height;

			//Calculates the position that the camera can see the edge of the sprites.
			float edgeVisablePositionRight = (myTransform.position.x + (spriteWidth/2)) - camHorizontalExtend;
			float edgeVisablePositionLeft = (myTransform.position.x - (spriteWidth/2)) + camHorizontalExtend;

			if (cam.transform.position.x >= edgeVisablePositionRight - offsetX && !hasRightBuddy)
			{
				MakeNewBuddy (1);
				hasRightBuddy = true;
			}

			else if (cam.transform.position.x <= edgeVisablePositionLeft + offsetX && !hasLeftBuddy)
			{
				MakeNewBuddy (-1);
				hasLeftBuddy = true;
			}
		}
	
	}

	//Function creates buddy on side required.
	void MakeNewBuddy (int direction){
		//Vector holds position of our new buddy
		Vector3 newPosition = new Vector3 (myTransform.position.x + myTransform.localScale.x*spriteWidth * direction, myTransform.position.y, myTransform.position.z);
		Transform newBuddy = (Transform)Instantiate (myTransform, newPosition, myTransform.rotation); //Creates a new sprite in the location with it's rotation in a vector.
	
		//if declared untilable, then reverse new sprite to stop graphics errors.
		if (reverseScale) {
			newBuddy.localScale = new Vector3(newBuddy.localScale.x*-1, newBuddy.localScale.y, newBuddy.localScale.z);
		}

		newBuddy.transform.parent = parents.transform;
		if(direction>0){
			newBuddy.GetComponent<Tiling>().hasLeftBuddy = true;
		}
		else {
			newBuddy.GetComponent<Tiling>().hasRightBuddy = true;
		}
	}
}
