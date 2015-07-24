using UnityEngine;
using System.Collections;

public class PlayerInput : PlayerModel
{
	//Used regions in this larger script because mono-develop self documents on the right hand side "document outline".
	#region public variables 

	private PlayerModel _playerModel;		//Max time to accelerate on jump
	private bool enableDoubleJump = true;			//To allow jumping off a wall if no floor contact.
	private bool wallHitDoubleJumpOverRide = true;	//To allow jumping off a wall if no floor contact.
	
	Renderer rend;
	Rigidbody2D playerRigidBody;
	Transform playerGraphics;
	#endregion

	#region private variables
	private bool canDoubleJump = true;
	private float jmpDuration;						//Current Internal value for how long jumpkey has been held for
	private bool jumpKeyDown = false;
	private bool _canJump = false;
	private bool facingRight = true;
	private Animator anim;
	#endregion

	void Start ()
	{
		DontDestroyOnLoad(transform.gameObject);
		playerGraphics = GameObject.FindGameObjectWithTag("Graphics").transform;
		playerRigidBody = GetComponent<Rigidbody2D>();
		_playerModel = GetComponent<PlayerModel>();
		rend = GetComponentInChildren<Renderer>(); //get player's sprite renderer so we can define size of sprite later on
		anim = GetComponentInChildren<Animator>();
	}
	
	
	// Update is called once per frame
	void FixedUpdate ()
	{

		anim.SetBool ("Ground", IsOnGround ());
		
		// Set the vertical animation
		anim.SetFloat ("xSpeed", playerRigidBody.velocity.x);
		anim.SetFloat ("ySpeed", playerRigidBody.velocity.y);

		float horizontalInput = Input.GetAxis ("Horizontal"); //Mapped so it will work with both joystick and keyboard through Unity
		
		if (horizontalInput < -0.01f) { //if input is negative, eg to the left
			if (facingRight) { 
				Flip (); 
			}
			if (playerRigidBody.velocity.x > -this._playerModel.playerStats.maxSpeed) { //If player speed is less than the set max speed
				playerRigidBody.AddForce (new Vector2 (-this._playerModel.playerStats.acceleration, 0.0f)); //Use acceleration value to add speed
			} else {
				playerRigidBody.velocity = new Vector2 (-this._playerModel.playerStats.maxSpeed, playerRigidBody.velocity.y); //Else player speed is max speed
			}
		} else if (horizontalInput > 0.01f) {
			if (!facingRight) {
				Flip ();
			}
			if (playerRigidBody.velocity.x < this._playerModel.playerStats.maxSpeed) {
				playerRigidBody.AddForce (new Vector2 (this._playerModel.playerStats.acceleration, 0.0f));
			} else {
				playerRigidBody.velocity = new Vector2 (this._playerModel.playerStats.maxSpeed, playerRigidBody.velocity.y);
			}
		}

		bool onTheGround = IsOnGround ();
		float verticalInput = Input.GetAxis ("Vertical");

		if (onTheGround) {
			canDoubleJump = true;
		}

		if (verticalInput > 0.01F) {
			if (!jumpKeyDown) {
				jumpKeyDown = true;
			}
			if (onTheGround || (canDoubleJump && enableDoubleJump) || wallHitDoubleJumpOverRide) { //Logic for checking if the player is able to jump off walls. Was going to use this as a mutation, but felt it was nice to have by default.
				bool wallHit = false;
				float wallHitDirection = 0;

				bool leftWallHit = IsOnWallLeft ();
				bool rightWallHit = IsOnWallRight ();

				if (horizontalInput != 0) {
					if (leftWallHit) {
						wallHit = true;
						wallHitDirection = 0.75f; //This value is used because jumping off walls felt wrong at full jump strength. I've lowered it to 75% of their max value when on walls.
					} else if (rightWallHit) {
						wallHit = true;
						wallHitDirection = -0.75f;//Negative value for ensuring jumping the correct way on a right hand wall (Away from the wall)
					}
				}

				if (!wallHit) {
					if (onTheGround || canDoubleJump && enableDoubleJump) {
						playerRigidBody.velocity = new Vector2 (playerRigidBody.velocity.x, this._playerModel.playerStats.jumpSpeed);

						jmpDuration = 0.0f;
						_canJump = true;
					}
				} else {
					playerRigidBody.velocity = new Vector2 (this._playerModel.playerStats.jumpDuration * wallHitDirection, this._playerModel.playerStats.jumpSpeed);
					jmpDuration = 0.0f;
					_canJump = true;
				}
				if (!onTheGround && !wallHit) {
					canDoubleJump = false;
				}
			} 
		} else if (_canJump) { //This is used to add a delay onto the next jump, it's an internal counter.
			jmpDuration += Time.deltaTime;
			bool hasBumpedRoof = HasBumpedRoof ();
			if ((jmpDuration < this._playerModel.playerStats.jumpDuration / 1000) && !hasBumpedRoof) { //If internal jump duration is less than duration we have set to be maximum, and player still hasn't collided with a roof. Devide by 1k b/c milliseconds from deltaTime. Was going to be used for a mutation with an inordinately large value to alow the player to fly. It works, but looks like it's a bug so I removed it.
				playerRigidBody.velocity = new Vector2 (playerRigidBody.velocity.x, this._playerModel.playerStats.jumpDuration); //Continue to add force.
				hasBumpedRoof = HasBumpedRoof ();
			}
			if (hasBumpedRoof) {
				playerRigidBody.velocity = new Vector2 (playerRigidBody.velocity.x, (-(playerRigidBody.velocity.y)*0.25f)); //If player hits roof, push down the way at 25% of the speed they crashed into it with
			}
		} else { //reset values
			jumpKeyDown = false;
			_canJump = false;
		}
	}

	#region HelperFunctions
	//Raycasts a small, invisible line which checks if player is on the ground.
	private bool IsOnGround () 
	{
		float raycastLength = 0.1f;
		float colliderThreshold = 0.001f;

		//Start raycast line @ transform of the players current x, and the player's current y position minus the bottom of the sprite, minus our offset. EG the calibrated distance from the center of the sprite to the bottom.
		Vector2 linestart = new Vector2 (this.rend.transform.position.x, this.rend.transform.position.y - this.rend.bounds.extents.y - colliderThreshold);

		Vector2 vectorToSearch = new Vector2 (this.transform.position.x, linestart.y - raycastLength);

		RaycastHit2D hit = Physics2D.Linecast (linestart, vectorToSearch);
		//Debug.DrawLine (linestart, vectorToSearch, Color.cyan); Makes the line visible via "Gizmos" in unity, only for debugging.
		if (hit) {
			//Debug.Log ("onGround");
		}
		return hit;
	}

	//Raycasts a small line above your head to check if we should stop a jump to avoid clipping issues
	private bool HasBumpedRoof ()
	{
		float raycastLength = 0.1f;
		float colliderThreshold = 0.0001f;

		Vector2 linestart = new Vector2 (this.transform.position.x, this.transform.position.y + this.rend.bounds.extents.y + colliderThreshold); //Start point of raycast is slightly lower than the lowest y co-ord of the renderer's image
		Vector2 vectorToSearch = new Vector2 (this.transform.position.x, linestart.y + raycastLength);
		RaycastHit2D hit = Physics2D.Linecast (linestart, vectorToSearch);
		//Debug.DrawLine (linestart, vectorToSearch, Color.cyan);
		return hit;
	}

	//Raycasts a line to see if player is colliding with a wall on the left side
	public bool IsOnWallLeft ()
	{
		bool returnValue = false;
		float lengthToSearch = 0.1f;
		float colliderThreshold = 0.001f;

		Vector2 linestart = new Vector2 (this.transform.position.x - this.rend.bounds.extents.x - colliderThreshold, this.transform.position.y); //Starting point of the raycast is slightly further than the width of the renderer that this script is attached to.
		Vector2 vectorToSearch = new Vector2 (linestart.x - lengthToSearch, this.transform.position.y); //Second point which the line raycasts to, set by the length of the search.

		RaycastHit2D hitLeft = Physics2D.Linecast (linestart, vectorToSearch); 
		Debug.DrawLine (linestart, vectorToSearch, Color.cyan);
		returnValue = hitLeft;

		if (returnValue == true) {
			playerRigidBody.velocity = new Vector2 (0, 0);
			if (hitLeft.collider.GetComponent<NoSlideJump> ()) {
				returnValue = false;
			}
		}

		return returnValue;
	}

	private bool IsOnWallRight ()
	{
		bool returnValue = false;
		float lengthToSearch = 0.1f;
		float colliderThreshold = 0.001f;
		
		Vector2 linestart = new Vector2 (this.transform.position.x + this.rend.bounds.extents.x + colliderThreshold, this.transform.position.y);
		Vector2 vectorToSearch = new Vector2 (linestart.x + lengthToSearch, this.transform.position.y);

		RaycastHit2D hitRight = Physics2D.Linecast (linestart, vectorToSearch);
		Debug.DrawLine (linestart, vectorToSearch, Color.cyan);
		returnValue = hitRight;
		
		if (returnValue == true) {
			playerRigidBody.velocity = new Vector2 (0, 0);
			if (hitRight.collider.GetComponent<NoSlideJump> ()) {
				returnValue = false;
			}
		}
		return returnValue;
	}

	private void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1 to flip char.
		Vector3 theScale = playerGraphics.localScale;
		theScale.x *= -1;
		playerGraphics.localScale = theScale;
	}
	#endregion


	#region SettersAndGetters
	float MaxSpeed {
		get {
			return this._playerModel.playerStats.maxSpeed;
		}
		set {
			_playerModel.playerStats.maxSpeed = value;
		}
	}
	
	float Acceleration {
		get {
			return this._playerModel.playerStats.Health;
		}
		set {
			this._playerModel.playerStats.Health = value;
		}
	}
	
	float JumpDuration {
		get {
			return this._playerModel.playerStats.jumpDuration;
		}
		set {
			this._playerModel.playerStats.jumpDuration = value;
		}
	}
	
	float JumpSpeed {
		get {
			return this._playerModel.playerStats.jumpDuration;
		}
		set {
			this._playerModel.playerStats.jumpDuration = value;
		}
	}
	#endregion
}
