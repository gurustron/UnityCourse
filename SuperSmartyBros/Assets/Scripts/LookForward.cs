using UnityEngine;
using System.Collections;

public class LookForward : MonoBehaviour
{
	public bool facingLeft;

	private Transform _transform;
	private float _previousFrameX;
	private int _defaultDirection;


	protected virtual void Awake()
	{
		// get a reference to the components we are going to be changing and store a reference for efficiency purposes
		_transform = GetComponent<Transform>();
		_previousFrameX = _transform.position.x;
		_defaultDirection = facingLeft ? -1 : 1;
	}

	public void Update()
	{
		var deltaX = transform.position.x - _previousFrameX;
		Flip(deltaX);
	}

	// flip the enemy to face torward the direction he is moving in
	protected void Flip(float deltaX)
	{
		// get the current scale
		Vector3 localScale = _transform.localScale;

		if ((deltaX > 0f) && (localScale.x * _defaultDirection < 0f))
		{
			localScale.x *= -1;
		}

		if ((deltaX < 0f) && (localScale.x * _defaultDirection > 0f))
		{
			localScale.x *= -1;
		}

		// update the scale
		_transform.localScale = localScale;
	}
}
