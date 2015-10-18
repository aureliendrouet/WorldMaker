﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class AirCraft : MonoBehaviour {

	private Rigidbody cRigidbody;
	private Rigidbody CRigidbody {
		get {
			if (cRigidbody == null) {
				this.cRigidbody = this.GetComponent<Rigidbody> ();
			}

			return cRigidbody;
		}
	}

	public bool engineBoost = false;
	public float engineBoostPow = 10f;
	public float engineInc = 5f;
	public float enginePow = 0f;
	public float enginePowMax = 20f;
	public float enginePowMin = -5f;

	public float lift;

	public float cForward;
	public float cRight;
	public float cUp;
	
	public float wingC;
	public float leftWingRotation;
	public Transform leftWing;
	public float rightWingRotation;
	public Transform rightWing;

	public float cYaw;
	public float cPitch;
	public float cRoll;

	void Start () {
		this.enginePow = 0f;
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Z)) {
			this.enginePow += this.engineInc;
			if (this.enginePow > this.enginePowMax) {
				this.enginePow = this.enginePowMax;
			}
		}

		if (Input.GetKeyDown (KeyCode.S)) {
			this.enginePow -= this.engineInc;
			if (this.enginePow < this.enginePowMin) {
				this.enginePow = this.enginePowMin;
			}
		}
	}

	private float forwardVelocity;
	private float rightVelocity;
	private float upVelocity;

	private float sqrt2halved = Mathf.Sqrt (2f) / 2f;

	private float mouseX;
	private float mouseY;

	void FixedUpdate () {
		mouseX = (Input.mousePosition.x - (Screen.width / 2f)) / (Screen.width / 2f);
		mouseY = (Input.mousePosition.y - (Screen.height / 2f)) / (Screen.height / 2f);

		mouseX = mouseX * Mathf.Abs (mouseX);
		mouseY = mouseY * Mathf.Abs (mouseY);

		this.leftWingRotation = (mouseY * sqrt2halved - mouseX * sqrt2halved) * 60f;
		this.rightWingRotation = (mouseX * sqrt2halved + mouseY * sqrt2halved) * 60f;

		forwardVelocity = Vector3.Dot (this.CRigidbody.velocity, this.transform.forward);
		rightVelocity = Vector3.Dot (this.CRigidbody.velocity, this.transform.right);
		upVelocity = Vector3.Dot (this.CRigidbody.velocity, this.transform.up);

		float sqrForwardVelocity = forwardVelocity * Mathf.Abs (forwardVelocity);
		float sqrRightVelocity = rightVelocity * Mathf.Abs (rightVelocity);
		float sqrUpVelocity = upVelocity * Mathf.Abs (upVelocity);

		if (Input.GetKey (KeyCode.Space)) {
			this.engineBoost = true;
			this.CRigidbody.AddForce ((enginePow + engineBoostPow) * this.transform.forward);
		} 
		else {
			this.engineBoost = false;
			this.CRigidbody.AddForce (enginePow * this.transform.forward);
		}
		this.cRigidbody.AddForce (sqrForwardVelocity * this.lift * this.transform.up);

		this.cRigidbody.AddForce (- sqrForwardVelocity * this.cForward * this.transform.forward);
		this.cRigidbody.AddForce (- sqrRightVelocity * this.cRight * this.transform.right);
		this.cRigidbody.AddForce (- sqrUpVelocity * this.cUp * this.transform.up);

		this.CRigidbody.AddTorque (cYaw * mouseX * this.transform.up);
		this.CRigidbody.AddTorque (- cPitch * mouseY * this.transform.right);

		float roll = 0;
		if (Input.GetKey (KeyCode.Q)) {
			roll ++;
		}
		if (Input.GetKey (KeyCode.D)) {
			roll --;
		}
		this.CRigidbody.AddTorque (cRoll * roll * this.transform.forward);

	}

	void OnGUI () {
		if (this.engineBoost) {
			GUILayout.TextArea ("EnginePow = " + (this.enginePow + this.engineBoostPow));
		} 
		else {
			GUILayout.TextArea ("EnginePow = " + this.enginePow);
		}
		GUILayout.TextArea ("ForwardVelocity = " + this.forwardVelocity);
		GUILayout.TextArea ("RightdVelocity = " + this.rightVelocity);
		GUILayout.TextArea ("UpVelocity = " + this.upVelocity);
		GUILayout.TextArea ("Velocity = " + this.CRigidbody.velocity);
		GUILayout.TextArea ("MouseX = " + this.mouseX);
		GUILayout.TextArea ("MouseY = " + this.mouseY);
	}
}
