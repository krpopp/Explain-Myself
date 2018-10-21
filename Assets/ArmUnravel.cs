using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmUnravel : MonoBehaviour {

	public Vector3 gameObjectSreenPoint;
	public Vector3 mousePreviousLocation;
	public Vector3 mouseCurLocation;


	public Vector3 force;
	public Vector3 objectCurrentPosition;
	public Vector3 objectTargetPosition;
	public float topSpeed = 10;



	private bool isClicked = false;

	Rigidbody2D myBody;



	// Use this for initialization
	void Start () {
		myBody = gameObject.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void FixedUpdate()
	{
		myBody.velocity = force;
	}

	void OnMouseDown(){
		isClicked = true;
		GameManager.holdingItem = true;

		gameObjectSreenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);
		mousePreviousLocation = new Vector3(Input.mousePosition.x, Input.mousePosition.y, gameObjectSreenPoint.z);
	}

	void OnMouseDrag(){
		mouseCurLocation = new Vector3(Input.mousePosition.x, Input.mousePosition.y, gameObjectSreenPoint.z);
		force = mouseCurLocation - mousePreviousLocation;//Changes the force to be applied
		mousePreviousLocation = mouseCurLocation;

	}

	void OnMouseUp (){
		isClicked = false;
		GameManager.holdingItem = false;

		if (myBody.velocity.magnitude > topSpeed){
			force = myBody.velocity.normalized * topSpeed;
		}
	}



}
