using UnityEngine;
using System.Collections;
using System;

public class Backboard : MonoBehaviour {

	[SerializeField] GameObject ScoreBonus;
	public event Action<Collision> m_onBackboardHitted;

	bool m_bonusActive;
	public bool IsActiveBonus { get { return m_bonusActive; } }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ActivateBonus()
	{	
		m_bonusActive = true;
		ScoreBonus.SetActive (true);
	}

	public void DeactivateBonus()
	{
		m_bonusActive = false;
		ScoreBonus.SetActive (false);
	}

	void OnCollisionEnter(Collision collision) {
		if (m_bonusActive)
			if (m_onBackboardHitted != null)
				m_onBackboardHitted (collision); //Maybe instance id better but give me different values
	}
}
