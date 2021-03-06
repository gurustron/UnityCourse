﻿using UnityEngine;
using System.Collections;

public abstract class BaseEnemy : MonoBehaviour
{
	[Range(0, 10f)]
	public float moveSpeed = 4f;  // enemy move speed when moving
	public int damageAmount = 10; // probably deal a lot of damage to kill player immediately
	public string playerLayer = Aliases.Layers.Player;  // name of the layer to put enemy on when stunned
	public GameObject[] myWaypoints; // to define the movement waypoints
	public float waitAtWaypointTime = 1f;   // how long to wait at a waypoint
	public bool loopWaypoints = true; // should it loop through the waypoints

	[Tooltip("Child GameObject to detect stun")]
	public GameObject headCheck; // what gameobject is the stunnedCheck
									// SFXs
	public AudioClip stunnedSFX;
	public AudioClip attackSFX;

	// private variables below

	// store references to components on the gameObject
	protected Transform _transform;
	protected Rigidbody2D _rigidbody;
	protected Animator _animator;
	protected AudioSource _audio;

	// movement tracking
	[SerializeField]
	protected int _myWaypointIndex = 0; // used as index for My_Waypoints
	protected float _moveTime;
	protected float _vx = 0f;
	protected bool _moving = true;

	// store the layer number the enemy is on (setup in Awake)
	protected int _enemyLayer;

	protected virtual void Awake()
	{
		// get a reference to the components we are going to be changing and store a reference for efficiency purposes
		_transform = GetComponent<Transform>();

		_rigidbody = GetComponent<Rigidbody2D>();
		if (_rigidbody == null) // if Rigidbody is missing
			Debug.LogError("Rigidbody2D component missing from this gameobject");

		_animator = GetComponent<Animator>();
		if (_animator == null) // if Animator is missing
			Debug.LogError("Animator component missing from this gameobject");

		_audio = GetComponent<AudioSource>();
		if (_audio == null)
		{ // if AudioSource is missing
			Debug.LogWarning("AudioSource component missing from this gameobject. Adding one.");
			// let's just add the AudioSource component dynamically
			_audio = gameObject.AddComponent<AudioSource>();
		}

		if (headCheck == null)
		{
			Debug.LogError("stunnedCheck child gameobject needs to be setup on the enemy");
		}

		// setup moving defaults
		_moveTime = 0f;
		_moving = true;

		// determine the enemies specified layer
		_enemyLayer = this.gameObject.layer;
	}


	// if not stunned then move the enemy when time is > _moveTime
	protected virtual void Update()
	{
		if (Time.time >= _moveTime)
		{
			EnemyMovement();
		}
		else
		{
			_animator.SetBool(Aliases.Animator.Moving, false);
		}
	}

	// Move the enemy through its rigidbody based on its waypoints
	protected void EnemyMovement()
	{
		// if there isn't anything in My_Waypoints
		if ((myWaypoints.Length != 0) && (_moving))
		{

			// make sure the enemy is facing the waypoint (based on previous movement)
			Flip(_vx);

			// determine distance between waypoint and enemy
			_vx = myWaypoints[_myWaypointIndex].transform.position.x - _transform.position.x;

			// if the enemy is close enough to waypoint, make it's new target the next waypoint
			if (Mathf.Abs(_vx) <= 0.05f)
			{
				// At waypoint so stop moving
				_rigidbody.velocity = new Vector2(0, 0);

				// increment to next index in array
				_myWaypointIndex++;

				// reset waypoint back to 0 for looping
				if (_myWaypointIndex >= myWaypoints.Length)
				{
					if (loopWaypoints)
						_myWaypointIndex = 0;
					else
						_moving = false;
				}

				// setup wait time at current waypoint
				_moveTime = Time.time + waitAtWaypointTime;
			}
			else {
				// enemy is moving
				_animator.SetBool(Aliases.Animator.Moving, true);

				// Set the enemy's velocity to moveSpeed in the x direction.
				_rigidbody.velocity = new Vector2(_transform.localScale.x * moveSpeed, _rigidbody.velocity.y);
			}

		}
	}

	// flip the enemy to face torward the direction he is moving in
	protected void Flip(float _vx)
	{

		// get the current scale
		Vector3 localScale = _transform.localScale;

		if ((_vx > 0f) && (localScale.x < 0f))
			localScale.x *= -1;
		else if ((_vx < 0f) && (localScale.x > 0f))
			localScale.x *= -1;

		// update the scale
		_transform.localScale = localScale;
	}

	// Attack player
	protected virtual void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == Aliases.Tags.Player)
		{
			CharacterController2D player = collision.gameObject.GetComponent<CharacterController2D>();
			if (player.playerCanMove)
			{
				OnCollisionWithPlayer(collision, player);
			}
		}
	}

	protected virtual void OnCollisionWithPlayer(Collider2D collision, CharacterController2D player)
	{
		// Make sure the enemy is facing the player on attack
		Flip(collision.transform.position.x - _transform.position.x);

		// attack sound
		playSound(attackSFX);

		// stop moving
		_rigidbody.velocity = new Vector2(0, 0);

		// apply damage to the player
		player.ApplyDamage(damageAmount);

		// stop to enjoy killing the player
		_moveTime = Time.time + 3f;
	}

	// if the Enemy collides with a MovingPlatform, then make it a child of that platform
	// so it will go for a ride on the MovingPlatform
	protected void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "MovingPlatform")
		{
			this.transform.parent = other.transform;
		}
	}

	// if the enemy exits a collision with a moving platform, then unchild it
	protected void OnCollisionExit2D(Collision2D other)
	{
		if (other.gameObject.tag == "MovingPlatform")
		{
			this.transform.parent = null;
		}
	}

	// play sound through the audiosource on the gameobject
	protected void playSound(AudioClip clip)
	{
		_audio.PlayOneShot(clip);
	}

	public abstract void OnHeadCollision();
}