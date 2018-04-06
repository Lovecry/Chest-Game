using UnityEngine;
using System.Collections;

public class InputPlayerTouch : InputBase
{
	private struct TouchInfo
	{
		public Vector3 	m_vStartPosition;
		public Vector3 	m_vEndPosition;
		public float 	m_fTimeElapsed;
		public bool 	m_bStarted;
		public bool 	m_bStillTouched;
	}
	
	private TouchInfo[] 		m_aoTouchInfos;

	public override void Init(PaintTable paintTable)
	{
		base.Init(paintTable);

		m_aoTouchInfos = new TouchInfo[StaticConf.Input.MAX_TOUCH_NUMBERS];
	}

	public override void InputUpdate()
	{
		base.InputUpdate();

		for(int i = 0; i < StaticConf.Input.MAX_TOUCH_NUMBERS; ++i)
		{
			m_aoTouchInfos[i].m_bStillTouched = false;
		}

		for(int i = 0; i < Input.touchCount; ++i)
		{
			if(!m_aoTouchInfos[Input.touches[i].fingerId].m_bStarted)
			{
				StartTouch(Input.touches[i]);
				m_IsDrawing = true;
				m_drawerTool = m_paintTable.GetDrawerTool();
				m_DrawerStart = Input.mousePosition;
			}
			else
			{
				//Update the touch position
				m_aoTouchInfos[Input.touches[i].fingerId].m_vEndPosition = Input.touches[i].position;
				m_aoTouchInfos[Input.touches[i].fingerId].m_vEndPosition.x /= Screen.width;
				m_aoTouchInfos[Input.touches[i].fingerId].m_vEndPosition.y /= Screen.height;
				m_aoTouchInfos[Input.touches[i].fingerId].m_bStillTouched = true;
				if(m_IsDrawing){
					m_drawerTool.Draw(m_DrawerStart, Input.mousePosition);
				}
			}
		}

		for(int i = 0; i < StaticConf.Input.MAX_TOUCH_NUMBERS; ++i)
		{
			if(m_aoTouchInfos[i].m_bStarted && !m_aoTouchInfos[i].m_bStillTouched)
			{
				//Touch finished..
				TouchFinished(i);
				m_IsDrawing = false;
				m_drawerTool.EndDraw();
			}
		}
	}

	private void StartTouch(Touch iTouch)
	{
		if(iTouch.fingerId >= StaticConf.Input.MAX_TOUCH_NUMBERS)
		{
			Debug.LogError("Finger ID excedes max touch numbers");
			return;
		}

		int iID = iTouch.fingerId;
		m_aoTouchInfos[iID].m_bStarted = true;
		m_aoTouchInfos[iID].m_bStillTouched = true;
		m_aoTouchInfos[iID].m_fTimeElapsed = Time.time;
		m_aoTouchInfos[iID].m_vStartPosition = iTouch.position;
		m_aoTouchInfos[iID].m_vStartPosition.x /= Screen.width;
		m_aoTouchInfos[iID].m_vStartPosition.y /= Screen.height;
		m_aoTouchInfos[iID].m_vEndPosition = m_aoTouchInfos[iID].m_vStartPosition;
	}

	private void TouchFinished(int iID)
	{
		float fDistance = Vector3.Distance(m_aoTouchInfos[iID].m_vStartPosition, m_aoTouchInfos[iID].m_vEndPosition);
		if(fDistance >= StaticConf.Input.MIN_DISTANCE_VALIDATE)
		{
			m_aoTouchInfos[iID].m_fTimeElapsed = Time.time - m_aoTouchInfos[iID].m_fTimeElapsed;

			float fSpeed = fDistance / m_aoTouchInfos[iID].m_fTimeElapsed;
			if(fSpeed > StaticConf.Input.MIN_SPEED_VALIDATE)
			{
				Vector3 vDirection = m_aoTouchInfos[iID].m_vEndPosition - m_aoTouchInfos[iID].m_vStartPosition;
				vDirection.Normalize();
				
				float fAngle = VectorUtils.Angle(StaticConf.Input.DIRECTION_REFERENCE, StaticConf.Input.UP_VECTOR, vDirection);
				bool bValidAngle = VectorUtils.IsAngleWithinThreshold(vDirection, StaticConf.Input.UP_VECTOR, StaticConf.Input.DIRECTION_REFERENCE, StaticConf.Input.ANGLE_TRESHOLD);
				
				//Debug.Log("fDistance = " + fDistance + "fSpeed = " + fSpeed + " bValidAngle = " + bValidAngle + " fAngle = " + fAngle + " iID = " + iID);
				
				if(bValidAngle)
				{
					InternalSlideDetected(fDistance,fAngle);
				}
			}
		}

		m_aoTouchInfos[iID].m_bStarted = false;
	}
}
