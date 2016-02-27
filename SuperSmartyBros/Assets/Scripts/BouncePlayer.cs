using UnityEngine;
using System.Collections;

public class BouncePlayer : MonoBehaviour
{
	public float bounceForce;

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			other.gameObject.GetComponent<CharacterController2D>().Bounce(bounceForce);
		}
	}
}
