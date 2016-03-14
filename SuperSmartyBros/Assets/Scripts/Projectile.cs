using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
	[HideInInspector]
	public bool isEnemyProjectile;

	public void OnTriggerEnter2D(Collider2D collision)
	{
		switch (collision.gameObject.tag)
		{
			case Aliases.Tags.Player:
				var animator = GetComponent<Animator>();
				if (animator)
				{
					animator.SetTrigger(Aliases.Animator.Explode);
				}
				else
				{
					Debug.Log("Missing projectile animator");
				}

				//Destroy(gameObject);
				break;
			case Aliases.Tags.Projectile:
				var projectile = collision.gameObject.GetComponent<Projectile>();
				if (projectile.isEnemyProjectile != isEnemyProjectile)
				{
					Destroy(gameObject);
				}
				break;
			case Aliases.Tags.Ground:
			case Aliases.Tags.Platform:
			default:
				Destroy(gameObject);
				break;
		}
	}
}
