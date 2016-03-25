using UnityEngine;
using System.Collections;

public class EnemyHeadCollision : MonoBehaviour {

	// if Player hits the stun point of the enemy, then call Stunned on the enemy
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == Aliases.Tags.Player)
		{
			// tell the enemy to be stunned
			this.GetComponentInParent<BaseEnemy>().OnHeadCollision();
			other.gameObject.GetComponent<CharacterController2D>().EnemyBounce();
		}
	}
}
