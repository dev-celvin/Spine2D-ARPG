using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class KeyManager : MonoSingleton<KeyManager> {
	public GameObject[] button;
	public bool[] boolcheck;

	public enum KeyCode{
		Hitback,
		Combo,
		Evade,
		Attack,
		Jump,
		Walk_Left,
		Walk_Right,
		Run_Left,
		Run_Right

	}
	Dictionary<KeyCode, bool> keyMessage = new Dictionary<KeyCode, bool>();
	// Use this for initialization
	void Start () {
		Init();
		for(int i=0;i<button.Length;i++)
			UIEventListener.Get(button[i]).onPress = ButtonPress;

	}

	int statuNum;
	void Init()
	{
		statuNum =System. Enum.GetNames(typeof(KeyCode)).GetLength(0); 
		for(int i=0;i<statuNum;i++)
		{
			keyMessage.Add((KeyCode)i,false);
		}


	}

	public void ReceiveMoveDrag(KeyCode key,bool value)
	{
		keyMessage[key]=value;
	}

	void ButtonPress(GameObject button,bool ispress)
	{
		for(int i=0;i<statuNum;i++)
		{
			if(button.name==((KeyCode)i).ToString())
			{
				keyMessage[(KeyCode)i]=ispress;
			}
		}

	}
	public bool GetKeyMessage(KeyCode key)
	{
		return keyMessage[key];
	}

	// Update is called once per frame
	void LateUpdate () {
		for(int i=0;i<statuNum;i++)
		{
			boolcheck[i]= keyMessage[(KeyCode)i];
		}
	}

}
