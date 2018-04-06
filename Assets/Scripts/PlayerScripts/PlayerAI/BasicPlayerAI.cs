using UnityEngine;
using System.Collections;
using System;

public class BasicPlayerAI {

	ShootRacePlayer m_player;

	BasicPlayerAI(){
	}

	public BasicPlayerAI(ShootRacePlayer player) //Better if create and use interface for players, not ShootRace
	{
		Assert.Test (player != null, "Here is needed a valid Player");
		m_player = player;	
		m_player.m_onTurnStarted += Shoot;
	}

	private void Shoot()
	{
		float inputDistanceError = UnityEngine.Random.Range (-StaticConf.Gameplay.OPPONENT_AI_ERROR_OFFSET_DISTANCE_SHOOT, StaticConf.Gameplay.OPPONENT_AI_ERROR_OFFSET_DISTANCE_SHOOT);
		float inputAngleError = UnityEngine.Random.Range (-StaticConf.Gameplay.OPPONENT_AI_ERROR_OFFSET_ANGLE_SHOOT, StaticConf.Gameplay.OPPONENT_AI_ERROR_OFFSET_ANGLE_SHOOT);
		m_player.Shoot (m_player.PerfectShootValue + inputDistanceError, inputAngleError);
	}
}
