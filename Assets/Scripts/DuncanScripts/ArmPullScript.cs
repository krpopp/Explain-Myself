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
	Vector3 momentum;

	Vector3 startPos;
	Vector3 startFrontOffset;
	Vector3 startBackOffset;

	bool firstDetached = true;

	Vector3 storedMomentum;

	Collider2D col;

	Renderer rend;

	float fullyVisibleDist;
	float fullyHiddenDist;

	bool forwardGrabbed = false;


	// Use this for initialization
	void Start () {
		startPos = transform.position;
		col = GetComponent<Collider2D>();
		rend = GetComponent<SpriteRenderer>();
		if(forwardNeighbor != null){
			startFrontOffset = transform.position - forwardNeighbor.transform.position;
			// col.enabled = false;
		}
		if(backNeighbor != null){
			startBackOffset = transform.position - backNeighbor.transform.position;
			fullyVisibleDist = startBackOffset.magnitude;
			fullyHiddenDist = ArmPullManager.Instance.detachDistance;
		}
		
		SetState(currentState);
	}
	
	// Update is called once per frame
	void Update () {
		if(stateUpdate != null){
			stateUpdate();
		}
		if(backNeighbor != null){
			float dist = Vector3.Distance(transform.position, backNeighbor.transform.position);
			float percentage = (dist - fullyVisibleDist) / (fullyHiddenDist - fullyVisibleDist);
			HandleShader(percentage);
		}
	}

	void FixedUpdate(){
		momentum *= 1 - ArmPullManager.Instance.drag;
	}

	#region ATTACHED
	void InitAttached(){

	}

	void AttachedUpdate(){
		if(forwardNeighbor != null){
			if((transform.position - forwardNeighbor.transform.position - startFrontOffset).magnitude > ArmPullManager.Instance.detachDistance)
				SetState(SegmentState.DETACHED);
		} else if(forwardGrabbed){
			if(Input.GetMouseButtonUp(0)){
				forwardGrabbed = false;
			}
			Vector3 worldSpaceMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			worldSpaceMouse.z = 0;

			Vector3 offset = transform.position - worldSpaceMouse;
			if(offset.magnitude > ArmPullManager.Instance.detachDistance){
				SetState(SegmentState.GRABBED);
			}
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
		AddForces();
		momentum += new Vector3(0, ArmPullManager.Instance.gravity  * Time.deltaTime, 0);	
		transform.position += Vector3.ClampMagnitude(momentum, ArmPullManager.Instance.maxPullMagnitude);
	}
	#endregion

	void AddForces(){
		if(forwardNeighbor != null && backNeighbor != null){
			Vector3 backOffset = transform.position - backNeighbor.transform.position - startBackOffset;
			momentum -= backOffset.normalized * Mathf.Pow(backOffset.sqrMagnitude, ArmPullManager.Instance.magnitudePower) * ArmPullManager.Instance.pullForce * Time.deltaTime;

			Vector3 frontOffset = transform.position - forwardNeighbor.transform.position - startFrontOffset;
			momentum -= frontOffset.normalized * Mathf.Pow(frontOffset.sqrMagnitude, ArmPullManager.Instance.magnitudePower) * ArmPullManager.Instance.forwardPullForce * Time.deltaTime;

		} else if(forwardNeighbor != null){
			Vector3 frontOffset = transform.position - forwardNeighbor.transform.position - startFrontOffset;
			momentum -= frontOffset.normalized * Mathf.Pow(frontOffset.sqrMagnitude, ArmPullManager.Instance.magnitudePower) * ArmPullManager.Instance.forwardPullForce * Time.deltaTime;
		} else{
			Vector3 backOffset = transform.position - backNeighbor.transform.position - startBackOffset;
			momentum -= backOffset.normalized * Mathf.Pow(backOffset.sqrMagnitude, ArmPullManager.Instance.magnitudePower) * ArmPullManager.Instance.pullForce * Time.deltaTime;
		}
	}

	#region GRABBED
	void InitGrabbed(){
		lastMousePosition = transform.position;
		momentum = Vector3.zero;
		ArmPullManager.Instance.SomethingGrabbed(transform);
	}

	void GrabbedUpdate(){
		if(Input.GetMouseButtonUp(0)){
			ArmPullManager.Instance.LetGo();
			SetState(SegmentState.DETACHED);
		}

		Vector3 worldSpaceMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		worldSpaceMouse.z = 0;

		Vector3 offset = transform.position - worldSpaceMouse;

		momentum -= Vector3.ClampMagnitude(offset * ArmPullManager.Instance.grabPullForce * Time.deltaTime, ArmPullManager.Instance.maxGrabMagnitude);

		Vector3 backOffset = transform.position - backNeighbor.transform.position - startBackOffset;
		momentum -= backOffset.normalized * Mathf.Pow(backOffset.sqrMagnitude, ArmPullManager.Instance.magnitudePower) * ArmPullManager.Instance.pullForce * Time.deltaTime;

		transform.position += Vector3.ClampMagnitude(momentum, ArmPullManager.Instance.maxPullMagnitude);
	}
	#endregion

	void OnMouseDown(){
		if(currentState == SegmentState.DETACHED){
			SetState(SegmentState.GRABBED);
		} else if(currentState == SegmentState.ATTACHED){
			ArmPullManager.Instance.SomethingGrabbed(transform);
			if(forwardNeighbor == null)
				forwardGrabbed = true;
		}
	}


	void HandleShader(float percentageGone){
		rend.material.SetVector("_Point1", Vector2.Lerp(ArmPullManager.Instance.visibleOne, ArmPullManager.Instance.hiddenOne, percentageGone));
		rend.material.SetVector("_Point2", Vector2.Lerp(ArmPullManager.Instance.visibleTwo, ArmPullManager.Instance.hiddenTwo, percentageGone));
	}

	//TODO: add a light pull in OnMouseOver
}
