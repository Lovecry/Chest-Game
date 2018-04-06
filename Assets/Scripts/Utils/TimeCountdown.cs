using UnityEngine;
using System.Collections;
using System;

public class TimeCountdown
{
	int m_seconds = default(int);
	float m_secondsDecimals = default(float);
	CJM.CoroutineJob m_CountdownCoroutine;

	public event Action onTimeElapsed;
	public event Action onTimeChanged;
	public event Action onCountdownPaused;
	public event Action onCountdownUnpaused;
	public event Action onCountdownStopped;

	public void Start(int seconds)
	{
		if (seconds <= 0)
			Assert.Throw ("Time Countdown : Maybe is better set time > 0");
		if (onTimeElapsed == null)
			Assert.Throw ("Time Countdown : You should set at least on time elapsed callback");

		m_seconds = seconds;
		m_CountdownCoroutine = CJM.CoroutineJob.Start (CountdownSeconds (), true); 

		PrepareCallbacks ();
	}

	public void Start(float decimals)
	{
		if (decimals <= 0.0f)
			Assert.Throw ("Time Countdown : Maybe is better set time > 0");
		if (onTimeElapsed == null)
			Assert.Throw ("Time Countdown : You should set at least on time elapsed callback");
		
		m_secondsDecimals = decimals;
		m_CountdownCoroutine = CJM.CoroutineJob.Start (CountdownDecimals (), true); 
		
		PrepareCallbacks ();
	}

	void PrepareCallbacks ()
	{
		m_CountdownCoroutine.OnJobCompleted += onTimeElapsed;
		if (onCountdownPaused != null)
			m_CountdownCoroutine.OnJobPaused += onCountdownPaused;
		if (onCountdownUnpaused != null)
			m_CountdownCoroutine.OnJobPaused += onCountdownUnpaused;
		if (onCountdownStopped != null)
			m_CountdownCoroutine.OnJobPaused += onCountdownStopped;
	}

	public int GetSecondRemainig()
	{
		return m_seconds;
	}

	public float GetDecimalsRemainig()
	{
		return m_secondsDecimals;
	}

	public void Pause()
	{
		Assert.Test (m_CountdownCoroutine!=null, "You Should call Start before");
		m_CountdownCoroutine.Pause ();
	}

	public void Unpause()
	{
		Assert.Test (m_CountdownCoroutine!=null, "You Should call Start before");
		m_CountdownCoroutine.Unpause ();
	}

	public void Stop()
	{
		Assert.Test (m_CountdownCoroutine!=null, "You Should call Start before");
		m_CountdownCoroutine.Kill ();
	}
	
	IEnumerator CountdownSeconds()
	{
		while (m_seconds > 0) {
			yield return new WaitForSeconds (1f);
			m_seconds--;
			if (onTimeChanged != null)
				onTimeChanged();
		}
	}
	IEnumerator CountdownDecimals()
	{
		while (m_secondsDecimals > 0) {
			yield return new WaitForSeconds (0.1f);
			m_secondsDecimals -= 0.1f;
			if (onTimeChanged != null)
				onTimeChanged();
		}
	}
}
