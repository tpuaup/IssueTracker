using UnityEngine;
using System.Collections;

public class VanishTheBall : MonoBehaviour {

	private float timer;
	// Use this for initialization
	void Start () {
	
		timer = 0f;
	}
	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Space))
			timer = 0f;
		timer += Time.deltaTime;
	}

	void OnTriggerEnter(Collider other)
	{
		Destroy (other.gameObject);
		Debug.Log (timer.ToString());

	}
}
