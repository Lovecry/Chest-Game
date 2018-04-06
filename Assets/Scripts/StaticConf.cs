using UnityEngine;
using System.Collections;

namespace StaticConf 
{
	public static class Player
	{
		public const float Y_POSITION = 5.5f;
		public const float MIN_OFFSET_POSITION_ANGLE = -0.30f;
		public const float MAX_OFFSET_POSITION_ANGLE = -0.70f;

		public const float MAX_DISTANCE_TO_CHEST = 30.0f; 
		public const float MAX_DISTANCE_OFFSET = 10.0f;
		public const float MAX_DISTANCE_TO_CHEST_PLUS_OFFSET = 35.0f; 

		public const float MIN_DISTANCE_TO_CHEST = 25.0f; 
		public const float MIN_DISTANCE_OFFSET = -5.0f;
		public const float MIN_DISTANCE_TO_CHEST_PLUS_OFFSET = 20.0f;  

		public static readonly Vector3 STARTING_POSITION_PLAYER = new Vector3 (0.0f, 5.5f, 0.0f);
		public static readonly Vector3 STARTING_POSITION_PLAYER_BALL = new Vector3 (0.0f, 8.5f, 0.0f);
		public static readonly Vector3 STARTING_POSITION_OPPONENT = new Vector3 (-5.0f, 5.5f, 0.0f);
		public static readonly Vector3 STARTING_POSITION_OPPONENT_BALL = new Vector3 (-5.0f, 8.5f, 0.0f);

		public static readonly Vector3 WINNER_POSITION_ENDING_SCENE = new Vector3 (0.0f, 5.5f, 0.0f);
		public static readonly Vector3 LOSER_POSITION_ENDING_SCENE = new Vector3 (10.0f, 5.5f, 10.0f);
	}

	public static class Gameplay
	{
		public const float PERFECT_SHOOT_POWER_TOLLERANCE_NORMALIZED = 0.07f;
		public const float PERFECT_SHOOT_ANGLE_TOLLERANCE_NORMALIZED = 5.0f;
		public const float SHOOT_ERROR_ANGLE_RIDUCTION = 0.5f;
		public const float FIREBALL_POWER_MAX = 10.0f;
		public const float FIREBALL_TIME_SECONDS = 10.0f;
		public const float BAKCBOARD_BONUS_PERC_NORMALIZED = 0.6f;
		public const float GAME_TIME = 50.0f;
		public const float OPPONENT_AI_ERROR_OFFSET_DISTANCE_SHOOT = 0.2f;
		public const float OPPONENT_AI_ERROR_OFFSET_ANGLE_SHOOT = 10.0f;
	}

	public static class Ball
	{
		public const float BALL_Y_POSITION = 8.5f;
		public const float SHOOT_PREDICT_MAX_HEIGHT = 6f;
		public const float SHOOT_PREDICT_RANGE_OFFSET = 0.3f;
		public const float Y_AFTER_SHOOT_END_TURN = 8.0f;
	}

	public static class GUI
	{
		public const float SHOOTBAR_HEIGHT = 250f;
	}

	public static class Input
	{
		public const int 				ANGLE_TRESHOLD = 60; //Validation Angle gesture
		public const float 				MIN_SPEED_VALIDATE = 0.5f;
		public const float 				MIN_DISTANCE_VALIDATE = 0.15f;
		public static readonly Vector3 	DIRECTION_REFERENCE = new Vector3(0.0f, 1.0f, 0.0f);
		public static readonly Vector3 	UP_VECTOR = new Vector3(0.0f, 0.0f, 1.0f);

		public const int 				MAX_TOUCH_NUMBERS = 1;
	}

	public static class Score
	{
		public const int SCORE_BASIC = 2;
		public const int BACKBOARD_BONUS = 2;
		public const int FIREBALL_MULTIPLIER = 2;
		public const int PERFECT_SHOOT_BONUS = 1;
	}

	public static class Camera
	{
		public static readonly Vector3 DEFAULT_POSIITON = new Vector3 (0.0f, 1.0f, -11.0f);
	}

	public static class CameraAnim
	{
		public const float SHOOT_SCRIPTED_ANIM_POS_Y = 3.0f;
		public const float SHOOT_SCRIPTED_ANIM_POS_Z = -4.0f;
		public const float SHOOT_SCRIPTED_ANIM_POS_SPEED_Z_MIN = 0.1f;
		public const float SHOOT_SCRIPTED_ANIM_POS_SPEED_Z_SLOWING_OFFSET = 0.3f;
		public const float SHOOT_SCRIPTED_ANIM_ROT_X = 4.0f;
		public const float SHOOT_SCRIPTED_ANIM_SPEED_Y_MOV = 0.7f;
		public const float SHOOT_SCRIPTED_ANIM_SPEED_Z_MOV = 0.7f;
		public const float SHOOT_SCRIPTED_ANIM_SPEED_X_ROT = 1.0f;
		public const float SHOOT_SCRIPTED_ANIM_CHEST_DISTANCE_MAX = 25.0f;

		
		public static readonly Vector3 ENDGAME_SCRIPTED_ANIM_WINNER_POS = new Vector3 (0.0f, 5.5f, 0.0f);
		public static readonly Vector3 ENDGAME_SCRIPTED_ANIM_LOSER_POS = new Vector3 (5.0f, 5.5f, 5.0f);
		public static readonly Vector3 ENDGAME_SCRIPTED_ANIM_CAMERA_POS = new Vector3 (3.0f, 10.0f, -25.0f);
		public static readonly Vector3 ENDGAME_SCRIPTED_ANIM_CAMERA_ROT = new Vector3 (6.0f, 358.0f, 0.0f);
	}
}
