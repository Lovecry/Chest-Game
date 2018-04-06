using UnityEngine;
using System.Collections;
using System;

public class CameraMovement : MonoBehaviour {

	public event Action m_onEndAnimation;

	Transform m_OriginalParent;

	// Use this for initialization
	void Start () {
		m_OriginalParent = transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartShootMovement()
	{
		StartCoroutine (ShootMovement());
	}

	public void StartEndGameMovement()
	{

		StartCoroutine (EndGameMovement());
	}

	public void Reset()
	{
		transform.localPosition = StaticConf.Camera.DEFAULT_POSIITON;
		StopAllCoroutines ();
		transform.localEulerAngles = Vector3.zero;
	}

	IEnumerator ShootMovement()
	{
		float tempSpeed = StaticConf.CameraAnim.SHOOT_SCRIPTED_ANIM_SPEED_Z_MOV;
		Vector3 zOffsetMovement = new Vector3 (0.0f, 0.0f, 20.0f);
		Vector3 yOffsetMovement = new Vector3 (0.0f, 3.0f, 0.0f);
		Vector3 xOffsetRotation = new Vector3 (6.0f, 0.0f, 0.0f);

		//Anim position y
		while (transform.localPosition.y < StaticConf.CameraAnim.SHOOT_SCRIPTED_ANIM_POS_Y)
		{
			transform.localPosition += yOffsetMovement * Time.smoothDeltaTime * StaticConf.CameraAnim.SHOOT_SCRIPTED_ANIM_SPEED_Y_MOV;
			yield return null;
		}
		transform.localPosition = new Vector3(transform.localPosition.x, StaticConf.CameraAnim.SHOOT_SCRIPTED_ANIM_POS_Y, transform.localPosition.z);

		//Anim position z and rotation x
		while (transform.localPosition.z < StaticConf.CameraAnim.SHOOT_SCRIPTED_ANIM_POS_Z || transform.localEulerAngles.x < StaticConf.CameraAnim.SHOOT_SCRIPTED_ANIM_ROT_X )
		{
			if (transform.localPosition.z < StaticConf.CameraAnim.SHOOT_SCRIPTED_ANIM_POS_Z)
				transform.localPosition += zOffsetMovement * Time.smoothDeltaTime * tempSpeed;
			else
			{
				transform.localPosition += zOffsetMovement * Time.smoothDeltaTime * tempSpeed;
				tempSpeed -= tempSpeed >= StaticConf.CameraAnim.SHOOT_SCRIPTED_ANIM_POS_SPEED_Z_SLOWING_OFFSET? StaticConf.CameraAnim.SHOOT_SCRIPTED_ANIM_POS_SPEED_Z_SLOWING_OFFSET : 0.0f;
			}
			if (transform.localEulerAngles.x < StaticConf.CameraAnim.SHOOT_SCRIPTED_ANIM_ROT_X)
				transform.localEulerAngles += xOffsetRotation * Time.smoothDeltaTime * StaticConf.CameraAnim.SHOOT_SCRIPTED_ANIM_SPEED_X_ROT;
			yield return null;
		}

		//Anim with slowed movement camera, until a max distance from chest
		while (tempSpeed < StaticConf.CameraAnim.SHOOT_SCRIPTED_ANIM_POS_SPEED_Z_MIN && ((transform.position - GameManager.instance.ChestPosition().position).magnitude > StaticConf.CameraAnim.SHOOT_SCRIPTED_ANIM_CHEST_DISTANCE_MAX)) 
		{
			transform.localPosition += zOffsetMovement * Time.smoothDeltaTime * tempSpeed;
			tempSpeed -= tempSpeed >= StaticConf.CameraAnim.SHOOT_SCRIPTED_ANIM_POS_SPEED_Z_SLOWING_OFFSET? StaticConf.CameraAnim.SHOOT_SCRIPTED_ANIM_POS_SPEED_Z_SLOWING_OFFSET : 0.0f;

			yield return null;
		}
		if (m_onEndAnimation != null)
			m_onEndAnimation ();
	}

	IEnumerator EndGameMovement()
	{
		transform.SetParent (null,true);

		float tempPosSpeed = 5.0f;
		float tempRotSpeed = 1.0f;

		Vector3 distanceVector =  StaticConf.CameraAnim.ENDGAME_SCRIPTED_ANIM_CAMERA_POS - transform.position;
		float distance = distanceVector.magnitude;
		Vector3 distanceVectorNormalized = distanceVector.normalized;

		Quaternion destinationRotation = Quaternion.Euler (StaticConf.CameraAnim.ENDGAME_SCRIPTED_ANIM_CAMERA_ROT);

		while (distance > 1.0f ) // Just to avoid bad behaviour of reaching float
		{

			distance = (StaticConf.CameraAnim.ENDGAME_SCRIPTED_ANIM_CAMERA_POS - transform.position).magnitude;
			transform.position +=  distanceVectorNormalized * tempPosSpeed * Time.smoothDeltaTime ;

			transform.rotation = Quaternion.Slerp(Quaternion.Euler(transform.eulerAngles), destinationRotation, tempRotSpeed * Time.smoothDeltaTime); 
			yield return null; 
		}
		transform.position =  StaticConf.CameraAnim.ENDGAME_SCRIPTED_ANIM_CAMERA_POS;
		transform.eulerAngles = StaticConf.CameraAnim.ENDGAME_SCRIPTED_ANIM_CAMERA_ROT;
		transform.SetParent (m_OriginalParent);
		yield return null;

		if (m_onEndAnimation != null)
			m_onEndAnimation ();
	}
}
