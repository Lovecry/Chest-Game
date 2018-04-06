using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;  //Want Singleton

	//Here application managers References
	[SerializeField] private Camera m_PaintTableCamera;
	[SerializeField] private InputManager m_InputManager;
	public InputManager InputManager() {return m_InputManager;}
	[SerializeField] private GUIManager m_GuiManager;
	public GUIManager GUIManager() {return m_GuiManager;}
	[SerializeField] private AudioManager m_AudioManager;
	public AudioManager AudioManager() {return m_AudioManager;}
	[SerializeField] private AudioReferences m_AudioReferences;
	public AudioReferences AudioReferences() {return m_AudioReferences;}

	private PaintTable m_paintTable; 
	public PaintTable GetPaintTable(){return m_paintTable;}

	//Here the game
	IGame m_shootRaceGameManager;  //TODO :Here i can create what minigame i want , next feature to do
	bool m_gameWon = false;

	//Here useful references for all games
	[SerializeField] CameraMovement m_Camera;
	public CameraMovement Camera() {return m_Camera;}

	[SerializeField] Transform m_chestPosition; 	
	public Transform ChestPosition() {return m_chestPosition;}
	[SerializeField] Backboard m_backboard;
	public Backboard Backboard() {return m_backboard;}

	[SerializeField] TerrainTrigger m_terrain;
	public TerrainTrigger Terrain() {return m_terrain;}

	[SerializeField] Transform m_player;
	public Transform Player() {return m_player;}
	[SerializeField] Rigidbody m_playerBall;  
	public Rigidbody PlayerBall() {return m_playerBall;}
	[SerializeField] Transform m_opponent;
	public Transform Opponent() {return m_opponent;}
	[SerializeField] Rigidbody m_opponentBall;  
	public Rigidbody OpponentBall() {return m_opponentBall;}

	// Use this for initialization
	void Awake () {

		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);    
		
		DontDestroyOnLoad(gameObject);

		m_paintTable = new PaintTable(m_PaintTableCamera);
		m_InputManager.SetPaintTable(m_paintTable);
		m_shootRaceGameManager = new ShootRaceGame (); //TODO :Here i want create what minigame i want , next feature to do
		m_shootRaceGameManager.onGameStarted += OnGameStart;
		m_shootRaceGameManager.onGameEnded += OnGameEnd;

		m_AudioManager.PlayPersistent (m_AudioReferences.background2);
	}

	void Start()
	{
		m_gameWon = false;
		m_shootRaceGameManager.StartGame ();
	}

	public void StartNewGame()
	{
		m_shootRaceGameManager.EndGame ();
		GameUtils.ResetPlayerPositionOnTheCurt ();
		m_shootRaceGameManager = new ShootRaceGame (); //TODO :Here i want create what minigame i want , next feature to do
		m_shootRaceGameManager.onGameStarted += OnGameStart;
		m_shootRaceGameManager.onGameEnded += OnGameEnd;
		m_gameWon = false;
		m_shootRaceGameManager.StartGame ();
	}

	#region Callbacks
	void OnGameStart()
	{
		m_GuiManager.ShowInGameUI ();
		m_GuiManager.HideEndGameUI ();
	}
	
	void OnGameEnd(GameObject winnerPlayer, GameObject loserPlayer, bool gameWon)
	{
		m_gameWon = gameWon;
		m_GuiManager.SetGameRemainigTime(default(float).ToString());
		winnerPlayer.transform.position = StaticConf.CameraAnim.ENDGAME_SCRIPTED_ANIM_WINNER_POS;
		loserPlayer.transform.position = StaticConf.CameraAnim.ENDGAME_SCRIPTED_ANIM_LOSER_POS;

		Camera ().m_onEndAnimation += ActivateEndOfGameUI;
		Camera ().StartEndGameMovement ();
		m_shootRaceGameManager.onGameStarted -= OnGameStart;
		m_shootRaceGameManager.onGameEnded -= OnGameEnd;
		m_GuiManager.SetGameStartingTime("END");	
	}

	void ActivateEndOfGameUI()
	{
		m_GuiManager.HideInGameUI ();
		m_GuiManager.ShowEndGameUI ();
		if (m_gameWon)
			m_GuiManager.SetResultLabel ("YOU WON");
		else
			m_GuiManager.SetResultLabel ("YOU LOSE");
		Camera ().m_onEndAnimation -= ActivateEndOfGameUI;
	}
	#endregion
}
