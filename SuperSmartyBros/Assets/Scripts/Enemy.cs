using UnityEngine;
using System.Collections;



public class Enemy : BaseEnemy
{
	[Tooltip("Child GameObject to detect stun")]
	public GameObject stunnedCheck; // what gameobject is the stunnedCheck

	public float stunnedTime = 3f;   // how long to be stunned

	public string stunnedLayer = "StunnedEnemy";  // name of the layer to put maenemy on when stunned
												  // store the layer number the enemy should be moved to when stunned
	int _stunnedLayer;

	[HideInInspector]
	public bool isStunned = false;  // flag for isStunned

	protected override void Awake()
	{
		base.Awake();

		if (stunnedCheck == null)
		{
			Debug.LogError("stunnedCheck child gameobject needs to be setup on the enemy");
		}
		// determine the stunned enemy layer number
		_stunnedLayer = LayerMask.NameToLayer(stunnedLayer);

		// make sure collision are off between the playerLayer and the stunnedLayer
		// which is where the enemy is placed while stunned
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(playerLayer), _stunnedLayer, true);
	}

	protected override void Update()
	{
		if(!isStunned)
		{
			base.Update();
		}		
	}

	protected override void OnTriggerEnter2D(Collider2D collision)
	{
		if (!isStunned)
		{
			base.OnTriggerEnter2D(collision);
		}
	}

	// setup the enemy to be stunned
	public void Stunned()
	{
		if (!isStunned)
		{
			isStunned = true;

			// provide the player with feedback that enemy is stunned
			playSound(stunnedSFX);
			_animator.SetTrigger("Stunned");

			// stop moving
			_rigidbody.velocity = new Vector2(0, 0);

			// switch layer to stunned layer so no collisions with the player while stunned
			this.gameObject.layer = _stunnedLayer;
			stunnedCheck.layer = _stunnedLayer;

			// start coroutine to stand up eventually
			StartCoroutine(Stand());
		}
	}

	// coroutine to unstun the enemy and stand back up
	IEnumerator Stand()
	{
		yield return new WaitForSeconds(stunnedTime);

		// no longer stunned
		isStunned = false;

		// switch layer back to regular layer for regular collisions with the player
		this.gameObject.layer = _enemyLayer;
		stunnedCheck.layer = _enemyLayer;

		// provide the player with feedback
		_animator.SetTrigger("Stand");
	}
}
