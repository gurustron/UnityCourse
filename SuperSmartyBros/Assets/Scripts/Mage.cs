using UnityEngine;

public class Mage : BaseEnemy
{
	public GameObject projectile;
	public GameObject deathExplosion;
	[Tooltip("Distance to react on player")]
	public float detectionRadius;
	[Tooltip("Field of vision")]
	public float detectionAngle;
	
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
			var playerPosition = _player.transform.position;
			var magePosition = transform.position;

			var rayDirection = playerPosition - magePosition;
			var hit = Physics2D.Raycast(magePosition, rayDirection, detectionRadius, NotTransparentLayers);

			//var angle2 = Mathf.Abs(Vector3.Angle(magePosition, playerPosition));
			//var angle = Mathf.Abs(Mathf.Rad2Deg * Mathf.Atan((magePosition.y - playerPosition.y)/(magePosition.x - playerPosition.x)));// 180/Mathf.PI

			var angle = Mathf.Abs(Mathf.Rad2Deg * Mathf.Atan2(magePosition.y - playerPosition.y,magePosition.x - playerPosition.x));// 180/Mathf.PI

			if (hit && hit.transform.position == playerPosition && angle <= detectionAngle)
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
