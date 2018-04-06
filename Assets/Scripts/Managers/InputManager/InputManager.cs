using UnityEngine;
using System.Collections;
using System;

public class InputManager : MonoBehaviour 
{
	public event Action<float, float> onSlideDetected = null;
	private bool m_isEnabled = true;

	private InputBase m_oInput;
	private PaintTable m_paintTable;

	public void SetPaintTable(PaintTable paintTable)
	{
		m_paintTable = paintTable;
	}

	public void SetEnable(bool enabled)
	{
		m_isEnabled = enabled;
	}

	public bool IsEnabled()
	{
		return m_isEnabled;
	}

	void Start()
	{
		InitInput();
	}
	
	void Update()
	{
		if(m_oInput != null)
		{
			m_oInput.InputUpdate();
		}
	}

	private void InitInput()
	{
		m_oInput = InputFactory.GetInput();
		
		if(m_oInput != null)
		{
			m_oInput.Init(m_paintTable);

			m_oInput.Activate(
				OnSlideDetected
			);
		}
	}

	private void OnSlideDetected(float distance, float angle)
	{
		if(m_isEnabled)
			if(onSlideDetected != null)		
				onSlideDetected(distance, angle);
	}
}
