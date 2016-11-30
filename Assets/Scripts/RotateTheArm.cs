using UnityEngine;
using System.Collections;

public class RotateTheArm : MonoBehaviour {

	public Transform pivot;
	public GameObject baseball;
	public Transform firePoint;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		this.transform.RotateAround (pivot.position, this.transform.right, -20 * Time.deltaTime);
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Instantiate (baseball, firePoint.position, firePoint.rotation);

		}
	
	}
}
