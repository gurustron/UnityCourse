using System;
using UnityEngine;

// this comes from the Unity Standard Assets
namespace UnityStandardAssets._2D
{
    public class Camera2DFollow : MonoBehaviour
    {
        public Transform target;
        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;

		// private variables
        float _offsetZ;
        Vector3 _lastTargetPosition;
        Vector3 _currentVelocity;
        Vector3 _lookAheadPos;

        // Use this for initialization
        private void Start()
        {
            _lastTargetPosition = target.position;
            _offsetZ = (transform.position - target.position).z;
            transform.parent = null;

			// if target not set, then set it to the player
			if (target==null)
			{
				target = GameObject.FindGameObjectWithTag(Aliases.Tags.Player).transform;
			}

			if (target==null)
				Debug.LogError("Target not set on Camera2DFollow.");

        }

        // Update is called once per frame
		private void Update()
        {
			if (target == null)
				return;

            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target.position - _lastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                _lookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
            }
            else
            {
				_lookAheadPos = Vector3.MoveTowards(_lookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
            }

			Vector3 aheadTargetPos = target.position + _lookAheadPos + Vector3.forward*_offsetZ;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref _currentVelocity, damping);

            transform.position = newPos;

            _lastTargetPosition = target.position;
        }
    }
}
