using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName="CreateAsset/AudioReferences", fileName="AudioReferences")]
public class AudioReferences : ScriptableObject
{
	[Header("Background Audio")]
	[SerializeField] AudioClip m_Background1;
	[SerializeField] AudioClip m_Background2;

	[Header("Background Audio FX")]
	[SerializeField] AudioClip m_BallOnFire;

	[Header("OneShot Clip")]
	[SerializeField] AudioClip m_ButtonPressed;
	[SerializeField] AudioClip m_Scored;

	//getters
	public AudioClip background1 {get {return m_Background1;}}
	public AudioClip background2 {get {return m_Background2;}}

	public AudioClip ballOnFire {get {return m_BallOnFire; }}

	public AudioClip buttonPressed {get {return m_ButtonPressed; }}
	public AudioClip scored {get {return m_Scored; }}
}
