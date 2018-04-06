using UnityEngine;
using System.Collections;

public class OptionMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RestartGame()
	{
		GameManager.instance.StartNewGame ();
		GameManager.instance.AudioManager().PlayOneShoot (GameManager.instance.AudioReferences().buttonPressed);
	}

	public void GoToMenu()
	{
		Application.LoadLevel (0);
		GameManager.instance.AudioManager().PlayOneShoot (GameManager.instance.AudioReferences().buttonPressed);
	}
}
