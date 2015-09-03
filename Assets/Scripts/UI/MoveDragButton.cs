using UnityEngine;
using System.Collections;

public class MoveDragButton : MonoBehaviour {
	public float draglimit;
	public GameObject button;
	// Use this for initialization
	void Start () {
		UIEventListener.Get(button).onDrag = ButtonMove;
		UIEventListener.Get(button).onPress = ButtonPress;

	}
	void ButtonMove(GameObject buttongameobj,Vector2 delta)
	{

		button.transform.localPosition+=new Vector3(delta.x/transform.localScale.x,0,0);
		if(button.transform.localPosition.x>draglimit/4)
		{
			KeyManager.instance.ReceiveMoveDrag(KeyManager.KeyCode.Walk_Right,true);
		}else if(button.transform.localPosition.x<-draglimit/4){
			KeyManager.instance.ReceiveMoveDrag(KeyManager.KeyCode.Walk_Left,true);
		}else{
			KeyManager.instance.ReceiveMoveDrag(KeyManager.KeyCode.Walk_Left,false);
			KeyManager.instance.ReceiveMoveDrag(KeyManager.KeyCode.Walk_Right,false);
		}
		if(button.transform.localPosition.x>draglimit)
		{
			button.transform.localPosition=new Vector3(draglimit,0,0);
			KeyManager.instance.ReceiveMoveDrag(KeyManager.KeyCode.Run_Right,true);
			KeyManager.instance.ReceiveMoveDrag(KeyManager.KeyCode.Walk_Right,false);
		}else if(button.transform.localPosition.x<-draglimit)
		{
			button.transform.localPosition=new Vector3(-draglimit,0,0);
			KeyManager.instance.ReceiveMoveDrag(KeyManager.KeyCode.Run_Left,true);
			KeyManager.instance.ReceiveMoveDrag(KeyManager.KeyCode.Walk_Left,false);
		}else{
			KeyManager.instance.ReceiveMoveDrag(KeyManager.KeyCode.Run_Left,false);
			KeyManager.instance.ReceiveMoveDrag(KeyManager.KeyCode.Run_Right,false);
		}
	}
	void ButtonPress(GameObject buttongameobj,bool ispress)
	{
		if(ispress)
		{}
		else{
			button.GetComponent<SpringPosition>().enabled=true;
			KeyManager.instance.ReceiveMoveDrag(KeyManager.KeyCode.Walk_Left,false);
			KeyManager.instance.ReceiveMoveDrag(KeyManager.KeyCode.Walk_Right,false);
			KeyManager.instance.ReceiveMoveDrag(KeyManager.KeyCode.Run_Left,false);
			KeyManager.instance.ReceiveMoveDrag(KeyManager.KeyCode.Run_Right,false);
		}
	}
	void GetButtonStatu()
	{

	}
}
