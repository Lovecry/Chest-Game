using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	[SerializeField] AudioSource m_musicAudioSource;
	[SerializeField] AudioSource m_fxAudioSource;


	//Music AudioSource
	public void PlayPersistent(AudioClip music)
	{
		m_musicAudioSource.clip = music;
		m_musicAudioSource.Play ();
	}

	public void StopPersistent()
	{
		m_musicAudioSource.Stop ();
	}


	// FX Audio Source
	public void PlayPersistentFX(AudioClip music)
	{
		m_fxAudioSource.clip = music;
		m_fxAudioSource.Play ();
	}

	public void StopPersistentFX()
	{
		m_fxAudioSource.Stop ();
	}

	public void PlayOneShoot(AudioClip clip)
	{
		m_fxAudioSource.PlayOneShot (clip);
	}
}
