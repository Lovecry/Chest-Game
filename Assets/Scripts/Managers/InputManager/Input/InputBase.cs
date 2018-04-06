using UnityEngine;
using System.Collections;
using System;

public class InputBase
{
	protected event Action<float,float> m_onSlide = null;

	protected PaintTable m_paintTable;
	protected PaintTable.DrawerTool m_drawerTool;
	protected bool m_IsDrawing;
	protected Vector3 m_DrawerStart;
	protected Vector3 m_DrawerCurrent;

	public virtual void Init(PaintTable paintTable)
	{
		m_paintTable = paintTable;
	}

	public void Activate(Action<float, float> onSlideDetectedClb)
	{
		m_onSlide = onSlideDetectedClb;
	}

	public virtual void InputUpdate()
	{
	}

	protected void InternalSlideDetected(float distance, float angle)
	{
		if(m_onSlide != null)
		{
			m_onSlide(distance, angle);
		}
	}

	public void Deactivate()
	{
		m_onSlide =  null;
	}
}
