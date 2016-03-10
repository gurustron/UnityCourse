using UnityEngine;
using System.Collections;

public class Mage : BaseEnemy
{
	public GameObject projectile;
	public GameObject deathExplosion;

	protected override void OnCollisionWithPlayer(Collider2D collision, CharacterController2D player)
	{
		base.OnCollisionWithPlayer(collision, player);

		if(deathExplosion)
		{
			Instantiate(deathExplosion, transform.position, transform.rotation);
		}

		Destroy(gameObject);
	}
}
