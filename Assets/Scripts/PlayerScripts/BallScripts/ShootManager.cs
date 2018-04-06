using UnityEngine;
using System.Collections;

public static class ShootManager {

	public static void Shoot(float inputDistance, float inputAngle, float perfectShootPowerNormalizedValue, GameObject ball)
	{
		Vector3 startingPoint = ball.transform.position;
		Vector3 endingPoint = GameManager.instance.ChestPosition ().position;
		Vector3 force = CalculateForce (startingPoint, endingPoint);

		Vector3 angleErrorOffset = CalculateAngleErrorOffset (force, inputAngle); //Does not work as i want
		Vector3 powerErrorOffset = CalculatePowerErrorOffset (force, inputDistance, perfectShootPowerNormalizedValue );
		powerErrorOffset.y = 0.0f; //I Want error not on y

		ball.GetComponent<Rigidbody>().AddForce (force + powerErrorOffset +angleErrorOffset , ForceMode.Impulse);
	}

	private static Vector3 CalculateForce(Vector3 startPosition, Vector3 endPosition)
	{
		Vector3 velocity = new Vector3();
		Vector3 direction = new Vector3(endPosition.x, 0f, endPosition.z) - new Vector3(startPosition.x, 0f, startPosition.z);
		float distance = direction.magnitude;

		distance += StaticConf.Ball.SHOOT_PREDICT_RANGE_OFFSET; //This to ensure precision, try hit back of trigger
		Vector3 directionNormalized = direction.normalized;
		float maxYPos = endPosition.y + StaticConf.Ball.SHOOT_PREDICT_MAX_HEIGHT; //Max height ball
		
		// check if the range is far enough away where the shot may have flattened out enough to hit the front of the rim
		// if it has, switch the height to match a 45 degree launch angle
		if (distance / 2f > maxYPos)
				maxYPos = distance / 2f;
		
		// find the initial velocity in y direction
		velocity.y = Mathf.Sqrt(-2.0f * Physics.gravity.y * (maxYPos - startPosition.y));
		
		// find the total time by adding up the parts of the trajectory
		// time to reach the max
		float timeToMax = Mathf.Sqrt(-2.0f * (maxYPos - startPosition.y) / Physics.gravity.y);
		
		// time to return to y-target
		float timeToTargetY = Mathf.Sqrt(-2.0f * (maxYPos - endPosition.y) / Physics.gravity.y);
		
		// add them up to find the total flight time
		float totalFlightTime = timeToMax + timeToTargetY;
		
		// find the magnitude of the initial velocity in the xz direction
		float horizontalVelocityMagnitude = distance / totalFlightTime;
		
		// use the unit direction to find the x and z components of velocity
		velocity.x = horizontalVelocityMagnitude * directionNormalized.x;
		velocity.z = horizontalVelocityMagnitude * directionNormalized.z;
		
		return velocity;
	}

	private static Vector3 CalculatePowerErrorOffset(Vector3 optimalForce, float inputDistanceNormalized, float optimalDistanceNormalized)
	{
		Vector3 powerErrorOffset = default(Vector3);
		float powerErrorOffsetNormalized = inputDistanceNormalized - optimalDistanceNormalized;
		if (Mathf.Abs (powerErrorOffsetNormalized) < StaticConf.Gameplay.PERFECT_SHOOT_POWER_TOLLERANCE_NORMALIZED) {
			//Debug.Log("Perfect Power");
		}
		else {
			powerErrorOffset = ((optimalForce / optimalDistanceNormalized) * (optimalDistanceNormalized + powerErrorOffsetNormalized) - optimalForce);
		}
		return powerErrorOffset;
	}

	private static Vector3 CalculateAngleErrorOffset(Vector3 optimalForce, float inputAngle)
	{
		Vector3 angleErrorOffset = default(Vector3);
		float angleErrorOffsetNormalized = inputAngle - StaticConf.Gameplay.PERFECT_SHOOT_ANGLE_TOLLERANCE_NORMALIZED;
		if (Mathf.Abs (angleErrorOffsetNormalized) < StaticConf.Gameplay.PERFECT_SHOOT_ANGLE_TOLLERANCE_NORMALIZED) {
			//Debug.Log("Perfect Angle");
		}
		else {
			Vector3 newVector = Quaternion.AngleAxis(-inputAngle * StaticConf.Gameplay.SHOOT_ERROR_ANGLE_RIDUCTION, Vector3.up) * optimalForce ;
			angleErrorOffset = newVector - optimalForce;
		}
		return angleErrorOffset;
	}
}
