using UnityEngine;
using System.Collections;

public class Mage : MonoBehaviour
{
	public int damageAmount = 10; // probably deal a lot of damage to kill player immediately

	public GameObject projectile;

	[Range(0, 10f)]
	public float moveSpeed = 4f;  // enemy move speed when moving
	public GameObject[] myWaypoints; // to define the movement waypoints
	public float waitAtWaypointTime = 1f;   // how long to wait at a waypoint
	public bool loopWaypoints = true; // should it loop through the waypoints


	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
