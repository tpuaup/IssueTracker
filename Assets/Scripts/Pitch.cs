using UnityEngine;
using System.Collections;

public class Pitch : MonoBehaviour {
	
	public float speed = 100;
	public float rotatingSpeed = 100f;
	private Rigidbody rb;

	// Use this for initialization
	void Start () 
	{
		rb = GetComponent<Rigidbody> ();
		rb.maxAngularVelocity = 200 * 2 * Mathf.PI;
		rb.angularVelocity = this.transform.right * -2 * Mathf.PI * rotatingSpeed;

		rb.velocity = this.transform.forward * speed / 3.6f;
	}
	
	// Update is called once per frame
	void FixedUpdate() 
	{
		
	}
}
