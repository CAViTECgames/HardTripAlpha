using UnityEngine;

public class PlayerMovement : MonoBehaviour {


	Vector3 movement;                   // The vector to store the direction of the player's movement.     
	Animator anim;                      // Reference to the animator component.     
	Rigidbody playerRigidbody;          // Reference to the player's rigidbody.     
	int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.     
	float camRayLength = 100f;          // The length of the ray from the camera into the scene. 

	// Use this for initialization
	void Awake () 
	{
		// Create a layer mask for the floor layer.         
		floorMask = LayerMask.GetMask ("Floor");         

		// Set up references.         
		//anim = GetComponent <Animator> ();         
		playerRigidbody = GetComponent <Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		// Store the input axes.
		float h = Input.GetAxisRaw ("Horizontal");         
		float v = Input.GetAxisRaw ("Vertical");

		// Move the player around the scene
		Move (h, v);

		// tunr the player to face the mouse cursor
		Turning ();

		// Swap among the anomations that the player has
		Animating (h, v);

	}

	void Move(float h, float v)
	{
		movement.Set (h, 0f, v);
		movement = movement.normalized;
		playerRigidbody.MovePosition (transform.position + movement);
	}

	void Turning()
	{
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit floorHit;

		if(Physics.Raycast (camRay, out floorHit, camRayLength, floorMask))         
		{             // Create a vector from the player to the point on the floor the raycast from the mouse hit.             
			Vector3 playerToMouse = floorHit.point - transform.position;             // Ensure the vector is entirely along the floor plane.            
			playerToMouse.y = 0f;             // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.             
			Quaternion newRotation = Quaternion.LookRotation (playerToMouse);             // Set the player's rotation to this new rotation.             
			playerRigidbody.MoveRotation (newRotation);         
		}

	}

	void Animating (float h, float v)
	{
		bool walking = h != 0f || v != 0f;
		//anim.SetBool ("Walk", walking);
	}
}

