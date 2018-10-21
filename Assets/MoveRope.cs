using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRope : MonoBehaviour {


	public static float xPosRope;
	public static float yPosRope;

	public static float pointY;
	public static float pointX;


	Vector3 prevLoc;
	Vector3 newLoc;


	public Vector3 force;
	public Rigidbody2D myBody;


	// Use this for initialization
	void Start () {
		//myBody = gameObject.GetComponent<Rigidbody2D> ();

	}
	
	// Update is called once per frame
	void Update () {
		xPosRope = transform.position.x + (pointX/1000);
		yPosRope = transform.position.y + (pointY/1000);

		prevLoc = gameObject.transform.position;
		newLoc = new Vector2 (xPosRope, yPosRope);

		force = newLoc - prevLoc;
		myBody.velocity = force;

		gameObject.transform.position = new Vector2 (xPosRope, yPosRope);
	}
}
