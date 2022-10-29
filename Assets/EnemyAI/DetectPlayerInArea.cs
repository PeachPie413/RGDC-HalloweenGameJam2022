using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerInArea: MonoBehaviour
{
    [field: SerializedField]

    public bool PlayerInArea {get; private set;}
    public Transform Player {get; private set;}
    
    [SerializedField]
    private string detectionTag = "Player"

    private void onTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag(detectionTag)) {
            PlayerInArea = true;
            Player = collision.gameObject.transform;
        }
    }

    private void onTriggerExit2D(Collider2D collision) {
        if(collision.CompareTag(detectionTag)){
            PlayerInArea = false;
            Player = null;
        }

    }
}
