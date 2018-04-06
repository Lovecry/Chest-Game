using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ShootRaceGame : IGame {

	public event Action onStartingCountdownElapsed;
	public event Action onGameStarted;
	public event Action<GameObject, GameObject, bool> onGameEnded;

	//Time
	TimeCountdown m_gameStartCountdown;
	TimeCountdown m_gameRemainingTimeCountdown;
	bool m_gameTimeElapsed = false;

	ShootRacePlayer m_Player;
	ShootRacePlayer m_Opponent;
	List<ShootRacePlayer> m_PlayersInGame = new List<ShootRacePlayer>();

	public void StartGame()
	{
		Init ();
		GameManager.instance.InputManager ().SetEnable (false);
		GameManager.instance.GUIManager().SetGameStartingTime("");

		m_gameStartCountdown = new TimeCountdown ();
		m_gameStartCountdown.onTimeElapsed += OnStartingTimeCountdownElapsed;
		m_gameStartCountdown.onTimeChanged += OnStartingTimeCountdownChanged;

		GameManager.instance.InputManager ().onSlideDetected += OnBallShooted;

		m_gameStartCountdown.Start(4);
		m_gameTimeElapsed = false;

		if (onGameStarted != null)
			onGameStarted ();
	}

	void Init()
	{		
		m_Player = new ShootRacePlayer (GameManager.instance.Player().gameObject, GameManager.instance.PlayerBall().gameObject);
		m_Player.m_onTurnStarted += OnNewTurnStarted;
		m_Player.m_onTurnEnded += OnTurnEnded;
		m_Player.m_onScored += OnScored;
		m_Player.m_onFireballScoreUpdate += OnFireballScoreUpdated;
		m_Player.m_onFireballActivated += OnFireballActivated;
		m_Player.m_onFireballActivated += OnFireballDeactivated;
		m_Player.m_onFireballTimeChange += OnFireballCountdownChanged;
		
		m_PlayersInGame.Add (m_Player);
		
		m_Opponent = new ShootRacePlayer (GameManager.instance.Opponent().gameObject, GameManager.instance.OpponentBall().gameObject, true);
		m_Opponent.m_onTurnEnded += StartNewTurn;
		m_Opponent.m_onScored += OnScored;
		m_PlayersInGame.Add (m_Opponent);
		
		m_gameRemainingTimeCountdown = new TimeCountdown ();
		m_gameRemainingTimeCountdown.onTimeChanged += OnGameRemainingTimeChanged;
		m_gameRemainingTimeCountdown.onTimeElapsed += EndGame;

	}

	public void EndGame()
	{
		m_gameTimeElapsed = true;
			
		m_gameStartCountdown.onTimeElapsed -= OnStartingTimeCountdownElapsed;
		m_gameStartCountdown.onTimeChanged -= OnStartingTimeCountdownChanged;

		GameManager.instance.InputManager ().onSlideDetected -= OnBallShooted;
		
		m_Player.m_onTurnStarted -= OnNewTurnStarted;
		m_Player.m_onTurnEnded -= OnTurnEnded;
		m_Player.m_onScored -= OnScored;
		m_Player.m_onFireballScoreUpdate -= OnFireballScoreUpdated;
		m_Player.m_onFireballActivated -= OnFireballActivated;
		m_Player.m_onFireballActivated -= OnFireballDeactivated;
		m_Player.m_onFireballTimeChange -= OnFireballCountdownChanged;

		m_Opponent.m_onTurnEnded -= StartNewTurn;
		m_Opponent.m_onScored -= OnScored;

		GameManager.instance.Camera ().Reset ();
		GameManager.instance.PlayerBall ().GetComponent<Ball> ().Reset();
		GameManager.instance.GUIManager ().ResetShootBarSlider ();
		GameManager.instance.GUIManager ().UpdateFireBallBarSlider (0); //Reset Fireball bar

		m_gameRemainingTimeCountdown.onTimeChanged -= OnGameRemainingTimeChanged;
		m_gameRemainingTimeCountdown.onTimeElapsed -= EndGame;

		GameObject winnerPlayer ;
		GameObject loserPlayer ;
		bool gameWon = false;

		if (m_Player.Score > m_Opponent.Score) 
		{
			winnerPlayer = GameManager.instance.Player().gameObject;
			loserPlayer =  GameManager.instance.Opponent().gameObject;
			gameWon = true;
		} 
		else 
		{
			winnerPlayer =  GameManager.instance.Opponent().gameObject;
			loserPlayer =  GameManager.instance.Player().gameObject;
		}

		GameManager.instance.GUIManager().SetResultPlayerScore(m_Player.Score);
		GameManager.instance.GUIManager().SetResultOpponentScore(m_Opponent.Score);

		if (onGameEnded != null)
			onGameEnded (winnerPlayer, loserPlayer, gameWon);
	}

	void StartNewTurn(ShootRacePlayer player)
	{
		if (!m_gameTimeElapsed) {
			GameManager.instance.InputManager ().SetEnable (true);

			if (GameUtils.CalculateIfBackboardBlink ())  //backboard bonus can become active every start of every players (maybe % in static conf should be low)
				GameManager.instance.Backboard ().ActivateBonus ();

			player.StartTurn ();
		}
	}

	#region Callbacks
	
	void OnStartingTimeCountdownElapsed()
	{
		m_gameRemainingTimeCountdown.Start (StaticConf.Gameplay.GAME_TIME);
		GameManager.instance.GUIManager().SetGameStartingTime("");
		
		if (onStartingCountdownElapsed != null)
			onStartingCountdownElapsed ();
		
		for (int i=0; i< m_PlayersInGame.Count; ++i)
			StartNewTurn (m_PlayersInGame[i]);
	}

	void OnNewTurnStarted ()
	{
		GameManager.instance.GUIManager ().SetPerfectShootY (m_Player.PerfectShootValue);
	}

	void OnTurnEnded (ShootRacePlayer player)
	{
		GameManager.instance.Camera ().Reset ();
		GameManager.instance.GUIManager ().ResetShootBarSlider ();
		GameManager.instance.Backboard ().DeactivateBonus ();

		StartNewTurn (player);
	}

	void OnBallShooted(float inputDistance, float inputAngle)
	{
		GameManager.instance.InputManager ().SetEnable (false);
		GameManager.instance.GUIManager ().UpdateShootBarSlider (inputDistance);
		GameManager.instance.Camera ().StartShootMovement ();
		m_Player.Shoot (inputDistance, inputAngle);
	}
	
	void OnScored()
	{
		GameManager.instance.GUIManager ().UpdatePlayerScore (m_Player.Score);
		GameManager.instance.GUIManager ().UpdateOpponentScore (m_Opponent.Score);
	}

	void OnFireballScoreUpdated()
	{
		GameManager.instance.GUIManager ().UpdateFireBallBarSlider (m_Player.FireballScore / StaticConf.Gameplay.FIREBALL_POWER_MAX);
	}

	void OnFireballActivated (GameObject ball)
	{
	}

	void OnFireballDeactivated(GameObject ball)
	{
		GameManager.instance.GUIManager ().UpdateFireBallBarSlider (m_Player.FireballScore);
	}

	void OnFireballCountdownChanged(GameObject ball, float remainigTime)
	{
		GameManager.instance.GUIManager ().UpdateFireBallBarSlider (remainigTime / StaticConf.Gameplay.FIREBALL_TIME_SECONDS);
	}

	void OnStartingTimeCountdownChanged()
	{
		GameManager.instance.GUIManager().SetGameStartingTime(m_gameStartCountdown.GetSecondRemainig().ToString());
	}
	
	void OnGameRemainingTimeChanged ()
	{
		GameManager.instance.GUIManager().SetGameRemainigTime(m_gameRemainingTimeCountdown.GetDecimalsRemainig ().ToString("F1"));
	}
	#endregion
}
