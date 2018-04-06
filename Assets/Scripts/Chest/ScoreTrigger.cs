using UnityEngine;
using System.Collections;
using System;

public class ScoreTrigger : MonoBehaviour {

	public event Action<Collider> m_onTriggerEnter; 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (m_onTriggerEnter != null)
			m_onTriggerEnter (other); //Better if use id
	}
}
