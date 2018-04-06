using UnityEngine;
using System.Collections;
using System;

public interface IGame {

	event Action onGameStarted;
	event Action<GameObject,GameObject, bool> onGameEnded;

	void StartGame(); 
	void EndGame(); 
}
