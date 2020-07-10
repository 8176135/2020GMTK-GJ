using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlStuff : MonoBehaviour
{
	private Rigidbody2D rb;

	// Start is called before the first frame update
	void Start()
	{
		rb = gameObject.GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void FixedUpdate()
	{
		rb.AddForce(Vector2.up * 2);
	}
}