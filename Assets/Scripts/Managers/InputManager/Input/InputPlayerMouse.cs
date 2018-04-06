using UnityEngine;
using System.Collections;

public class InputMouse : InputBase
{
	private Vector3 m_vStartPosition;
	private Vector3 m_vEndPosition;
	private float 	m_fTimeElapsed = 0.0f;
	private float 	m_fInitialTime = 0.0f;

	public override void Init(PaintTable paintTable)
	{
		base.Init(paintTable);
	}

	public override void InputUpdate()
	{
		base.InputUpdate();

		if(Input.GetMouseButtonDown(0))
		{
			m_vStartPosition = Input.mousePosition;
			m_DrawerStart = Input.mousePosition;
			m_vStartPosition.x /= Screen.width;
			m_vStartPosition.y /= Screen.height;

			m_fInitialTime = Time.time;

			m_IsDrawing = true;
			m_drawerTool = m_paintTable.GetDrawerTool();
		}
		else if(Input.GetMouseButton(0))
		{
			if(m_IsDrawing){
				m_drawerTool.Draw(m_DrawerStart, Input.mousePosition);
			}
		}
		else if(Input.GetMouseButtonUp(0))
		{
			m_fTimeElapsed = Time.time - m_fInitialTime;

			m_vEndPosition = Input.mousePosition;
			m_vEndPosition.x /= Screen.width;
			m_vEndPosition.y /= Screen.height;

			CheckGesture();
			m_IsDrawing = false;
			m_drawerTool.EndDraw();
		}
	}

	private void CheckGesture()
	{
		Vector3 vDirection = m_vEndPosition - m_vStartPosition;
		float fDistance = vDirection.magnitude;

		if(fDistance >= StaticConf.Input.MIN_DISTANCE_VALIDATE)
		{
			float fSpeed = fDistance / m_fTimeElapsed;
			if(fSpeed > StaticConf.Input.MIN_DISTANCE_VALIDATE)
			{
				vDirection.Normalize();

				bool bValidAngle = VectorUtils.IsAngleWithinThreshold(StaticConf.Input.DIRECTION_REFERENCE, StaticConf.Input.UP_VECTOR, vDirection, StaticConf.Input.ANGLE_TRESHOLD);

				float fAngle = VectorUtils.Angle(StaticConf.Input.DIRECTION_REFERENCE, StaticConf.Input.UP_VECTOR, vDirection);
				//Debug.Log("fDistance = " + fDistance + "fSpeed = " + fSpeed + " bValidAngle = " + bValidAngle + " fAngle = " + fAngle);

				if(bValidAngle)
				{
					InternalSlideDetected(fDistance,fAngle);
				}
			}
		}
	}
}
