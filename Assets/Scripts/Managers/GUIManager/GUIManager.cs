using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

	[Header("In Game UI")]
	[SerializeField] private CanvasGroup m_inGameUIContainer;
	[SerializeField] private Text m_gameStartCountdownText;
	[SerializeField] private Text m_gameRemainigTimeCountdownText;
	[SerializeField] private RectTransform m_shootBar;
	[SerializeField] private RectTransform m_perfectShootIndicator; 
	[SerializeField] private RectTransform m_fireBallBar;
	[SerializeField] private Text m_playerScore;
	[SerializeField] private Text m_opponentScore;
	private Slider m_ShootBarSlider;
	private Slider m_fireBallBarSlider;

	[Header("End Game UI")]
	[SerializeField] private CanvasGroup m_endGameContainer;
	[SerializeField] private Text m_resultLabel;
	[SerializeField] private Text m_playerScoreFinal;
	[SerializeField] private Text m_opponentScoreFinal;

	// Use this for initialization
	void Start () {
		m_shootBar.sizeDelta = new Vector2 (m_shootBar.sizeDelta.x, StaticConf.GUI.SHOOTBAR_HEIGHT);
		m_ShootBarSlider = m_shootBar.gameObject.GetComponent<Slider> ();
		m_fireBallBarSlider = m_fireBallBar.gameObject.GetComponent<Slider> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetGameStartingTime(string timeToStart)
	{
		m_gameStartCountdownText.text = timeToStart;
	}
	
	void OnTimeStartingElapsed()
	{
		m_gameStartCountdownText.text = "";
	}
	
	public void SetGameRemainigTime(string remainingTime)
	{
		m_gameRemainigTimeCountdownText.text = remainingTime;
	}

	public void SetPerfectShootY(float normalizedYValue)
	{
		m_perfectShootIndicator.anchoredPosition = new Vector3 (0.0f, 0.0f + normalizedYValue * StaticConf.GUI.SHOOTBAR_HEIGHT, 0.0f);
	}

	public void UpdateShootBarSlider(float normalizedValue)
	{
		m_ShootBarSlider.value = normalizedValue;
	}

	public void ResetShootBarSlider()
	{
		m_ShootBarSlider.value = 0.0f;
	}

	public void UpdateFireBallBarSlider(float normalizedValue)
	{
		m_fireBallBarSlider.value = normalizedValue;
	}

	public void UpdatePlayerScore(int score)
	{
		m_playerScore.text = score.ToString();
	}

	public void UpdateOpponentScore(int score)
	{
		m_opponentScore.text = score.ToString();
	}

	public void SetResultLabel(string resultLabel)
	{
		m_resultLabel.text = resultLabel;
	}

	public void SetResultPlayerScore(int score)
	{
		m_playerScoreFinal.text = score.ToString();
	}
	
	public void SetResultOpponentScore(int score)
	{
		m_opponentScoreFinal.text = score.ToString();
	}

	public void HideInGameUI()
	{
		m_inGameUIContainer.alpha = 0.0f;
	}

	public void ShowInGameUI()
	{
		m_inGameUIContainer.alpha = 1.0f;
	}
	
	public void HideEndGameUI()
	{
		m_endGameContainer.alpha = 0.0f;
	}
	
	public void ShowEndGameUI()
	{
		m_endGameContainer.alpha = 1.0f;
	}
}
