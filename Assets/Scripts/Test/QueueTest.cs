using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QueueTest : MonoBehaviour {
	
	public Text text;
	public PlayerController m_player;

	void Start() {	
	}
	public void onConfirmClicked() {
		int[] queue = new int[text.text.Length];
		for(int i=0; i< text.text.Length; i++) {
			queue[i] = int.Parse(text.text.Substring(i,1));
		}
		m_player.setComboQueue(queue);
	}
	
}
