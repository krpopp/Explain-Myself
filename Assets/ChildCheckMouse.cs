using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCheckMouse : MonoBehaviour {

	ArmPullScript parentScript;

	void Start(){
		parentScript = GetComponentInParent<ArmPullScript>();
	}

	void OnMouseDown(){
		if(parentScript != null){
			parentScript.ChildHit();
		}
	}
}
