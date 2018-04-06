using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Intro : MonoBehaviour {

	[SerializeField] Button m_startGame;
	[SerializeField] Button m_exitGame;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartGame()
	{
		Application.LoadLevel (1);
	}

	public void ExitGame()
	{
		Application.Quit ();
	}
}
