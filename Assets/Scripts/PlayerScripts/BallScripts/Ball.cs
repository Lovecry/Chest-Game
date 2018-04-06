using UnityEngine;
using System.Collections;
using System;

public class Ball : MonoBehaviour {

	public event Action m_onFireBallActivated;
	public event Action m_onFireBallDeactivated;

	[SerializeField] Material m_fireMaterial;
	[SerializeField] ParticleSystem m_particleFire;
	MeshRenderer m_ballRenderer;
	Material m_defaultMaterial;
	Rigidbody m_rigidbody;

	// Use this for initialization
	void Start () {
		m_rigidbody = GetComponent<Rigidbody> ();
		m_ballRenderer = GetComponent<MeshRenderer> ();
		m_defaultMaterial = m_ballRenderer.material;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void ActivateOnFireBonus()
	{
		if (gameObject.GetInstanceID() == GameManager.instance.PlayerBall ().gameObject.GetInstanceID ()) {
			GameManager.instance.AudioManager().PlayPersistentFX(GameManager.instance.AudioReferences().ballOnFire);
		}
		m_particleFire.Play ();
		m_ballRenderer.material = m_fireMaterial;
		if (m_onFireBallActivated != null)
			m_onFireBallActivated ();
	}

	public void DeactivateOnFireBonus()
	{
		GameManager.instance.AudioManager ().StopPersistentFX ();
		m_particleFire.Stop ();
		m_ballRenderer.material = m_defaultMaterial;
		if (m_onFireBallDeactivated != null)
			m_onFireBallDeactivated ();
	}

	public void Reset()
	{
		m_rigidbody.velocity = Vector3.zero;
		m_rigidbody.angularVelocity = Vector3.zero;
		m_rigidbody.transform.rotation = Quaternion.Euler(new Vector3(0f,0f,0f));
		m_rigidbody.inertiaTensorRotation = Quaternion.identity;
		m_rigidbody.inertiaTensor = Vector3.one;
		m_rigidbody.isKinematic = true;
		m_rigidbody.isKinematic = false;
	}
}
