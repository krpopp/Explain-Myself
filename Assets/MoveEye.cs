using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEye : MonoBehaviour {

	public float moveSpeed;

	Vector3 prevMousePos;
	Vector3 newMousePos;

	Vector3 relativePos;

	Vector3 targetPos;

	float mouseDist;

	Rigidbody2D myBod;


	// Use this for initialization
	void Start () {
		myBod = gameObject.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		newMousePos = Input.mousePosition;
		mouseDist = Vector3.Distance (newMousePos, prevMousePos);
		if (mouseDist > 5f) {
			MoveTheEye ();
			prevMousePos = newMousePos;
			Debug.Log ("sip");
		}
		if (mouseDist < 5f) {
			myBod.velocity = Vector3.zero;
		}
	}

	void MoveTheEye(){
		myBod.velocity = Vector3.zero;
		targetPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		targetPos.z = transform.position.z;
		//transform.position = Vector3.MoveTowards (transform.position, targetPos, moveSpeed * Time.deltaTime);
		relativePos = targetPos - transform.position;
		myBod.AddForce(relativePos * moveSpeed);
	}
}
