using UnityEngine;
using System.Collections;
using System;

public class ShootRacePlayer
{
	public event Action  						m_onTurnStarted;
	public event Action<ShootRacePlayer>  		m_onTurnEnded;
	public event Action 						m_onScored;
	public event Action 						m_onFireballScoreUpdate;
	public event Action<GameObject> 			m_onFireballActivated;
	public event Action<GameObject, float> 		m_onFireballTimeChange;
	public event Action<GameObject>  			m_onFireballDeactivated;

	GameObject m_player;
	bool m_isBot = false;
	public bool IsBot {get {return m_isBot;} set {m_isBot = value;}}
	GameObject m_ball;
	BasicPlayerAI m_PlayerAI; // maybe better if derive from interace of behaviour

	//Ball bonus
	Ball m_ballScript;
	bool m_ballIsOnFire = false;
	TimeCountdown m_fireBallCountdown;
	bool m_ballShooted = false;

	bool m_turnFinished = true;
	public bool isFinishedTurn {get {return m_turnFinished;}}

	float m_perfectShootNormalizedValue = default(float);
	public float PerfectShootValue {get {return m_perfectShootNormalizedValue;}}

	//Score
	int m_currentScore = default(int);
	public int Score {get {return m_currentScore;}}
	int m_perfectShootBonus = default(int);
	int m_backboardBonus = default(int);
	float m_fireballScore = default(float);
	public float FireballScore {get {return m_fireballScore;}}
	int m_fireballMultiplier = 1;

	private ShootRacePlayer(){
	}

	public ShootRacePlayer(GameObject player, GameObject ball, bool isBot = false){
		
		Assert.Test (player != null, "You have to set a valid player before start turn");
		Assert.Test (ball != null, "You have to set a valid ball before start turn");

		m_player = player;
		m_ball = ball;
		m_ballScript = m_ball.GetComponent<Ball> ();
		m_isBot = isBot;

		if (m_isBot) {
			m_PlayerAI = new BasicPlayerAI(this);
		}
	}

	public void StartTurn()
	{
		m_backboardBonus = default(int);
		m_perfectShootBonus = default(int);

		if (m_fireballScore >= StaticConf.Gameplay.FIREBALL_POWER_MAX)
			ActivateFireball();

		if (m_ballShooted)
		{
			if (m_ballIsOnFire) 
				OnFireballCountdownElapsed();
			else
			{
				m_fireballScore = 0.0f;
				if (m_onFireballScoreUpdate!=null)
					m_onFireballScoreUpdate();
			}
		}

		m_ballShooted = false;
		m_turnFinished = false;

		GameManager.instance.ChestPosition ().gameObject.GetComponent<ScoreTrigger> ().m_onTriggerEnter += onChestTriggered;
		GameManager.instance.Backboard ().m_onBackboardHitted += onBackboardHitted;
		GameManager.instance.Terrain().m_onTriggerEnter += OnBallOnGround;

		GameUtils.ChangePositionAtRandomOnCurt (m_player.transform, m_ball.transform);

		m_perfectShootNormalizedValue = GameUtils.CalculatePerfectShootNormalizedValue (m_player.transform);

		if (m_onTurnStarted != null)
			m_onTurnStarted ();
	}

	public void Shoot(float inputDistance, float inputAngle)
	{
		ShootManager.Shoot (inputDistance, inputAngle, m_perfectShootNormalizedValue, m_ball);

		if (Mathf.Abs (inputDistance - m_perfectShootNormalizedValue) < StaticConf.Gameplay.PERFECT_SHOOT_POWER_TOLLERANCE_NORMALIZED) {
			m_perfectShootBonus = StaticConf.Score.PERFECT_SHOOT_BONUS;
		}

		m_ballShooted = true;
	}

	void EndTurn()
	{
		GameManager.instance.ChestPosition ().gameObject.GetComponent<ScoreTrigger> ().m_onTriggerEnter -= onChestTriggered;
		GameManager.instance.Backboard ().m_onBackboardHitted -= onBackboardHitted;
		GameManager.instance.Terrain().m_onTriggerEnter -= OnBallOnGround; //Here check if shooting

		m_ballScript.Reset();

		m_turnFinished = true;

		if (m_onTurnEnded != null)
			m_onTurnEnded (this);
	}

	void ActivateFireball()
	{
		m_ballIsOnFire = true;

		m_ballScript.ActivateOnFireBonus ();

		m_fireBallCountdown = new TimeCountdown ();

		m_fireBallCountdown.onTimeChanged += OnFireballCountdownTimeChanged;
		m_fireBallCountdown.onTimeElapsed += OnFireballCountdownElapsed;
		
		m_fireBallCountdown.Start (StaticConf.Gameplay.FIREBALL_TIME_SECONDS);

		m_fireballMultiplier = StaticConf.Score.FIREBALL_MULTIPLIER;

		if (m_onFireballActivated != null)
			m_onFireballActivated (m_ball);
	}

	void DeactivateFireball()
	{
		m_fireballScore = 0.0f;

		m_ballScript.DeactivateOnFireBonus ();

		m_ballIsOnFire = false;

		m_fireballMultiplier = 1;

		if (m_onFireballDeactivated != null)
			m_onFireballDeactivated (m_ball);
	}

	#region Callbacks
	void onChestTriggered (Collider colliderObject) //  Update my score if i trig
	{
		if (colliderObject.gameObject.GetInstanceID() == m_ball.GetInstanceID()) 
		{
			if (!m_isBot)
				GameManager.instance.AudioManager().PlayOneShoot (GameManager.instance.AudioReferences().scored);

			int shootScore = (StaticConf.Score.SCORE_BASIC + m_backboardBonus + m_perfectShootBonus) * (m_ballIsOnFire ? m_fireballMultiplier : 1);
			m_currentScore += shootScore;
			m_fireballScore += shootScore;
		
			if (!m_ballIsOnFire) 
				if (m_onFireballScoreUpdate!=null)
					m_onFireballScoreUpdate();

			m_ballShooted = false;

			if (m_onScored != null)
				m_onScored ();
		}
	}

	void onBackboardHitted(Collision collisionObject)
	{
		if (collisionObject.gameObject.GetInstanceID() == m_ball.GetInstanceID()) // Update my bonus if i hit backboard
			m_backboardBonus = StaticConf.Score.BACKBOARD_BONUS;
	}

	void OnBallOnGround(Collider colliderObject)
	{
		if (colliderObject.gameObject.GetInstanceID() == m_ball.GetInstanceID()) // End my turn
			EndTurn ();
	}

	void OnFireballCountdownTimeChanged()
	{
		m_fireballScore = (StaticConf.Gameplay.FIREBALL_TIME_SECONDS - m_fireBallCountdown.GetDecimalsRemainig ());
		if (m_onFireballTimeChange != null)
			m_onFireballTimeChange (m_ball, m_fireBallCountdown.GetDecimalsRemainig());
	}
	
	void OnFireballCountdownElapsed()
	{
		m_fireBallCountdown.onTimeChanged -= OnFireballCountdownTimeChanged;
		m_fireBallCountdown.onTimeElapsed -= OnFireballCountdownElapsed;

		DeactivateFireball ();
	}
	#endregion
}
