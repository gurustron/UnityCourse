using System;
using UnityEngine;

public class Mage : BaseEnemy
{
	public GameObject projectile;
	public GameObject deathExplosion;
	[Tooltip("Shots per second")]
	public float rateOfFire;
	public float power;
	[Tooltip("Distance to react on player")]
	public float detectionRadius;
	[Tooltip("Field of vision")]
	public float detectionAngle;
	
	private GameObject _player;
	private bool _enemyIsSeen = false;
	private float _timeNotShooting;

	private static readonly LayerMask NotTransparentLayers;
	private bool _firstShotFired;

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
		
		if (_player && _player.GetComponent<CharacterController2D>().playerCanMove)
		{
			var playerPosition = _player.transform.position;
			var magePosition = transform.position;

			var rayDirection = playerPosition - magePosition;
			var hit = Physics2D.Raycast(magePosition, rayDirection, detectionRadius, NotTransparentLayers);

			//var angle2 = Mathf.Abs(Vector3.Angle(magePosition, playerPosition));
			//var angle = Mathf.Abs(Mathf.Rad2Deg * Mathf.Atan((magePosition.y - playerPosition.y)/(magePosition.x - playerPosition.x)));// 180/Mathf.PI

			var angleInRadians = Mathf.Min(Mathf.Abs(Mathf.Atan2(magePosition.y - playerPosition.y, magePosition.x - playerPosition.x)),
				Mathf.Abs(Mathf.Atan2(playerPosition.y - magePosition.y, playerPosition.x - magePosition.x)));

			var angle = Mathf.Rad2Deg * angleInRadians;// 180/Mathf.PI

			if (hit && hit.transform.position == playerPosition && angle <= detectionAngle)
			{
				if (!_enemyIsSeen)
				{
					_enemyIsSeen = true;
					_animator.SetTrigger(Aliases.Animator.LookAtEnemy);
				}

				_timeNotShooting += Time.deltaTime;
				if (projectile && rateOfFire > 0 && ( (!_firstShotFired && _timeNotShooting >= 0.2 / rateOfFire) || _timeNotShooting >= 1/rateOfFire))
				{
					var newProjectile = (GameObject)Instantiate(projectile, magePosition, transform.rotation);

					var tmpProjectile = newProjectile.GetComponent<Projectile>();

					if (!tmpProjectile)
					{
						tmpProjectile = newProjectile.AddComponent<Projectile>();
					}

					tmpProjectile.isEnemyProjectile = true;

					var rigidBody = newProjectile.GetComponent<Rigidbody2D>();
					// if the projectile does not have a rigidbody component, add one
					if (!rigidBody)
					{
						rigidBody = newProjectile.AddComponent<Rigidbody2D>();
						rigidBody.gravityScale = 0;
					}

					// Apply force to the newProjectile's Rigidbody component if it has one

					rigidBody.AddForce(new Vector3(rayDirection.x, 0).normalized*power);
					_firstShotFired = true;
					_timeNotShooting = 0;
				}

				Flip(rayDirection.x);
			}
			else
			{
				if (_enemyIsSeen)
				{
					_enemyIsSeen = false;
					_animator.SetTrigger(Aliases.Animator.Idle);
					_timeNotShooting = 0;
					_firstShotFired = false;
				}
			}
		}
	}

	protected override void OnCollisionWithPlayer(Collider2D collision, CharacterController2D player)
	{
		base.OnCollisionWithPlayer(collision, player);

		MageDeath();
	}

	public override void OnHeadCollision()
	{
		MageDeath();
	}

	private void MageDeath()
	{
		if (deathExplosion)
		{
			Instantiate(deathExplosion, transform.position, transform.rotation);
		}

		Destroy(gameObject);
	}
}
