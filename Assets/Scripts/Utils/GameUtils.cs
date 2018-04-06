using UnityEngine;
using System.Collections;

public static class GameUtils  {
	
	public static void ChangePositionAtRandomOnCurt(Transform m_player, Transform m_playerBall)
	{
		Transform m_chest = GameManager.instance.ChestPosition ();

		Vector3 targetToLook = new Vector3 (m_chest.position.x,0.0f,m_chest.position.z);
		float randomAngleMultiplier = Random.Range (StaticConf.Player.MIN_OFFSET_POSITION_ANGLE, StaticConf.Player.MAX_OFFSET_POSITION_ANGLE); 
		float angle = randomAngleMultiplier * 360;
		float randomPositionOffsetMultiplier = Random.Range (StaticConf.Player.MIN_DISTANCE_OFFSET,StaticConf.Player.MAX_DISTANCE_OFFSET);

		Vector3 position;
		position.x = m_chest.position.x + StaticConf.Player.MAX_DISTANCE_TO_CHEST * Mathf.Sin(angle * Mathf.Deg2Rad);
		position.y = StaticConf.Player.Y_POSITION;
		position.z = m_chest.position.z + StaticConf.Player.MAX_DISTANCE_TO_CHEST * Mathf.Cos(angle * Mathf.Deg2Rad);

		Vector3 positionOffset = (m_chest.position - m_player.position).normalized * randomPositionOffsetMultiplier;
		positionOffset = new Vector3 (positionOffset.x, 0.0f, positionOffset.z);
		m_player.position = position + positionOffset;
		m_player.LookAt (targetToLook);

		m_player.eulerAngles = new Vector3(0, m_player.eulerAngles.y, m_player.eulerAngles.z); //Rotation x to 0
		m_playerBall.position = new Vector3 (m_player.position.x, StaticConf.Ball.BALL_Y_POSITION, m_player.position.z);
	}

	public static void ResetPlayerPositionOnTheCurt()
	{
		GameManager.instance.Player ().position = StaticConf.Player.STARTING_POSITION_PLAYER;
		GameManager.instance.Player ().rotation = Quaternion.identity;
		GameManager.instance.PlayerBall ().position = StaticConf.Player.STARTING_POSITION_PLAYER_BALL;

		GameManager.instance.Opponent ().position = StaticConf.Player.STARTING_POSITION_OPPONENT;
		GameManager.instance.Opponent ().rotation = Quaternion.identity;
		GameManager.instance.OpponentBall ().position = StaticConf.Player.STARTING_POSITION_OPPONENT_BALL;
	}

	public static float CalculatePerfectShootNormalizedValue (Transform m_player)
	{
		Vector3 playerPosition = m_player.position;
		float distance = (playerPosition - GameManager.instance.ChestPosition ().position).magnitude;
		float result = distance / StaticConf.Player.MAX_DISTANCE_TO_CHEST_PLUS_OFFSET;
		return result;
	}

	public static bool CalculateIfBackboardBlink()
	{
		return (UnityEngine.Random.value > StaticConf.Gameplay.BAKCBOARD_BONUS_PERC_NORMALIZED);
	}
}
