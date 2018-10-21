using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnravelBelly : MonoBehaviour {

	public float timeToChange;
	float changeTime;

	public Texture2D myTexture;

	static int xPos;
	static int yPos;

	static int moveStep = -1;
	static int maxStep = 1;

	public int currentPosX;
	public int currentPosY;

	public Color color;

	// Use this for initialization
	void Start () {
		//myTexture = GetComponent<MeshRenderer> ().material.mainTexture as Texture2D;
		changeTime = timeToChange;

		//xPos = Mathf.RoundToInt (gameObject.transform.position.x / 2);
		//yPos = Mathf.RoundToInt (gameObject.transform.position.y / 2);
	}
	
	// Update is called once per frame
	void Update () {
		if(GameManager.holdingItem){
			changeTime--;
			if (changeTime <= 0) {
				ChangeColor ();
				changeTime = timeToChange;
			}
		}
	
	}

	void ChangeColor(){
//		if (yPos < myTexture.height) {
//			yPos++;
//		} else {
//			if (xPos < myTexture.width) {
//				xPos++;
//			} else {
//				xPos = 0;
//			}
//			yPos = 0;
//		}
		Debug.Log("x is" + xPos);
		Debug.Log ("y is" + yPos);
		switch(moveStep){
		case -1: 
				yPos++;
			if (Mathf.Abs(yPos) >= maxStep) {
					moveStep = 1;
					maxStep++;
				}
				break;
			case 0:
				yPos--;
			if (Mathf.Abs(yPos) >= maxStep) {
					moveStep = 2;
				}
				break;
			case 1:
				xPos++;
			if (Mathf.Abs(xPos) >= maxStep) {
					moveStep = 0;	
				}
				break;
			case 2:
				xPos--;
			if (Mathf.Abs(xPos) >= maxStep) {
					moveStep = -1;

				}
				break;
		}
//		xPos = xPos + 100;
//		yPos = yPos + 100;
//

		//Color color = ((xPos & yPos) != 0 ? Color.white : Color.gray);
		MoveRope.pointX = xPos;
		MoveRope.pointY = yPos;

		myTexture.SetPixel((currentPosX+xPos), (currentPosY+yPos), color);
			

		myTexture.Apply();
	}

}
