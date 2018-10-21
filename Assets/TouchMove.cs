using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMove : MonoBehaviour {

	public Vector3 mousePosition;

	public float topSpeed;

	bool isClicked = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (isClicked) {
			mousePosition = Input.mousePosition;
			mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
			transform.position = Vector2.Lerp(transform.position, mousePosition, topSpeed);
		}
	}

	void OnMouseDown(){
		isClicked = true;
		GameManager.holdingItem = true;
	}

	void OnMouseUp(){
		isClicked = false;
		GameManager.holdingItem = false;
	}
}
