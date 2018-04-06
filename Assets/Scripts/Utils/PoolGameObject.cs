using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PoolGameObject {

	private GameObject[] m_aPool;
	private int m_iPoolSize;
	private int m_iNextAvailable;
	
	public PoolGameObject(GameObject poolObject, int poolCapacity)
	{
		m_iPoolSize=poolCapacity;
		m_aPool = new GameObject[poolCapacity];
		for (int i=0; i<poolCapacity; ++i) {
			GameObject InstanceObj = (GameObject.Instantiate(poolObject));
			//InstanceObj.hideFlags = HideFlags.HideInHierarchy;
			m_aPool[i] = InstanceObj;
			m_aPool[i].gameObject.SetActive(false);
		}
	}

	public GameObject getElement()
	{
		int count = m_iNextAvailable;
		
		if (m_aPool[m_iNextAvailable].gameObject.activeInHierarchy)
		{
			Assert.Throw ("GameObjectPool Too Small!!!");
		}

		m_aPool[m_iNextAvailable].gameObject.SetActive(true);

		m_iNextAvailable = (m_iNextAvailable + 1) % m_iPoolSize;
		return m_aPool[count];

	}
	
	public bool Disable(GameObject target)
	{
		bool result = false;
		if (target != null) {
			target.SetActive(false);
			m_iNextAvailable = (m_iNextAvailable - 1) % m_iPoolSize;
			if (m_iNextAvailable == -1 )
				m_iNextAvailable = m_iPoolSize-1;
			result = true;
		}
		return result;
	}
	
	public bool DisableAll()
	{
		bool result = false;
		if (m_aPool != null && m_aPool.Length >= 0) {
			for (int i = 0; i < m_aPool.Length; ++i) {
				m_aPool [i].gameObject.SetActive (false);
			}
			m_iNextAvailable = 0;
			result = true;
		}
		return result;
	}
}