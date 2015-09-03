using UnityEngine;
using System.Collections;

public class KeyTestSimple : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		for(int i=0;i<9;i++)
		{
			if(KeyManager.instance.GetKeyMessage((KeyManager.KeyCode)i))
			{
				print ((KeyManager.KeyCode)i);
			}
		}

	}
}
