using UnityEngine;
using System.Collections;

public class TiggerTest : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collider) {
        print(collider.name);
        
    }


}
