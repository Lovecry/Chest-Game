using UnityEngine;
using System.Collections;

public class InputFactory 
{
	public static InputBase GetInput()
	{
		InputBase oInputImplementation = null;

		if(Input.touchSupported)
		{
			oInputImplementation = new InputPlayerTouch();
		}
		else
		{
			oInputImplementation = new InputMouse();
		}
		
		if(oInputImplementation == null)
		{
			Debug.LogError("Input implementation not available!");
		}

		return oInputImplementation;
	}
}
