using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmPullScript : MonoBehaviour {


	[SerializeField] SegmentState currentState = SegmentState.ATTACHED;

	delegate void StateUpdate();
	StateUpdate stateUpdate;

	void SetState(SegmentState newState){
		currentState = newState;

		switch(currentState){
			case SegmentState.ATTACHED:
				InitAttached();
				stateUpdate = AttachedUpdate;
				break;
			case SegmentState.DETACHED:
				InitDetached();
				stateUpdate = DetachedUpdate;
				break;
			case SegmentState.GRABBED:
				InitGrabbed();
				stateUpdate = GrabbedUpdate;
				break;
			case SegmentState.FIXED:
				stateUpdate = null;
				break;
		}
	}

	[SerializeField] ArmPullScript forwardNeighbor;
	[SerializeField] ArmPullScript backNeighbor;

	Vector3 lastMousePosition;
	Vector3 offset;
	Vector3 momentum;

	Vector3 startPos;
	Vector3 startFrontOffset;
	Vector3 startBackOffset;

	bool firstDetached = true;

	Vector3 storedMomentum;

	Collider2D col;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
		col = GetComponent<Collider2D>();
		if(forwardNeighbor != null){
			startFrontOffset = transform.position - forwardNeighbor.transform.position;
			col.enabled = false;
		}
		if(backNeighbor != null){
			startBackOffset = transform.position - backNeighbor.transform.position;
		}
		
		SetState(currentState);
	}
	
	// Update is called once per frame
	void Update () {
		if(stateUpdate != null){
			stateUpdate();
		}
	}

	void FixedUpdate(){
		momentum *= 1 - ArmPullManager.Instance.drag;
	}

	#region ATTACHED
	void InitAttached(){
		if(forwardNeighbor != null)
			startFrontOffset = transform.position - forwardNeighbor.transform.position;
	}

	void AttachedUpdate(){
		if(forwardNeighbor != null && (transform.position - forwardNeighbor.transform.position - startFrontOffset).magnitude > ArmPullManager.Instance.detachDistance){
			SetState(SegmentState.DETACHED);
		}
	}
	#endregion

	#region DETACHED
	void InitDetached(){
		if(firstDetached){
			firstDetached = false;
			col.enabled = true;
		}
	}

	void DetachedUpdate(){
		if(forwardNeighbor != null && backNeighbor != null){
			Vector3 backOffset = transform.position - backNeighbor.transform.position - startBackOffset;
			momentum -= backOffset.normalized * Mathf.Pow(backOffset.sqrMagnitude, ArmPullManager.Instance.magnitudePower) * ArmPullManager.Instance.pullForce;

			Vector3 frontOffset = transform.position - forwardNeighbor.transform.position - startFrontOffset;
			momentum -= frontOffset.normalized * Mathf.Pow(frontOffset.sqrMagnitude, ArmPullManager.Instance.magnitudePower) * ArmPullManager.Instance.pullForce;

		} else if(forwardNeighbor != null){
			Vector3 frontOffset = transform.position - forwardNeighbor.transform.position - startFrontOffset;
			momentum -= frontOffset.normalized * Mathf.Pow(frontOffset.sqrMagnitude, ArmPullManager.Instance.magnitudePower) * ArmPullManager.Instance.pullForce;
		} else{
			Vector3 backOffset = transform.position - backNeighbor.transform.position - startBackOffset;
			momentum -= backOffset.normalized * Mathf.Pow(backOffset.sqrMagnitude, ArmPullManager.Instance.magnitudePower) * ArmPullManager.Instance.pullForce;
		}
		momentum += new Vector3(0, ArmPullManager.Instance.gravity, 0);		
		transform.position += Vector3.ClampMagnitude(momentum, ArmPullManager.Instance.maxPullMagnitude);
	}
	#endregion

	#region GRABBED
	void InitGrabbed(){
		lastMousePosition = transform.position;
		momentum = Vector3.zero;
	}

	void GrabbedUpdate(){
		if(Input.GetMouseButtonUp(0)){
			SetState(SegmentState.DETACHED);
		}

		Vector3 worldSpaceMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		worldSpaceMouse.z = 0;

		offset = transform.position - worldSpaceMouse;

		momentum -= offset * ArmPullManager.Instance.grabPullForce * Time.deltaTime;
		
		transform.position += Vector3.ClampMagnitude(momentum, ArmPullManager.Instance.maxPullMagnitude);
	}
	#endregion

	void OnMouseDown(){
		if(currentState == SegmentState.DETACHED){
			SetState(SegmentState.GRABBED);
		} else if(currentState == SegmentState.ATTACHED && forwardNeighbor == null){
			SetState(SegmentState.GRABBED);
		}
	}

	//TODO: add a light pull in OnMouseOver
}
