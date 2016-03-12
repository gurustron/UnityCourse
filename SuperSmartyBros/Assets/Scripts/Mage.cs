using UnityEngine;
using System.Collections;

public class Mage : BaseEnemy
{
	public GameObject projectile;
	public GameObject deathExplosion;
	[Tooltip("Distance to react on player")]
	public float visibilityRadius;

	private GameObject _player;
	private bool _enemyIsSeen = false;

	private static readonly LayerMask NotTransparentLayers;
	static Mage()
	{
		NotTransparentLayers = LayerMask.GetMask(Aliases.Layers.Ground, Aliases.Layers.Platform, Aliases.Layers.Player);
	}


	protected override void Awake()
	{
		base.Awake();

		_player = GameObject.FindGameObjectWithTag(Aliases.Tags.Player);

		if (_player == null)
		{
			Debug.LogError("Missing player");
		}
	}

	protected override void Update()
	{
		base.Update();
		if (_player)
		{
			var rayDirection = _player.transform.position - transform.position;
			var hit = Physics2D.Raycast(transform.position, rayDirection, visibilityRadius, NotTransparentLayers);
			if (hit && hit.transform.position == _player.transform.position)
			{
				if (!_enemyIsSeen)
				{
					_enemyIsSeen = true;
					_animator.SetTrigger(Aliases.Animator.LookAtEnemy);
				}

				Flip(rayDirection.x);
			}
			else
			{
				if (_enemyIsSeen)
				{
					_enemyIsSeen = false;
					_animator.SetTrigger(Aliases.Animator.Idle);
				}
			}
		}
	}

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
