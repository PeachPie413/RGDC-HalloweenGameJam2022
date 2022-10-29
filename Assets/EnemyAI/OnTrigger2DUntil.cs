using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class onTrigger2DUntil: MonoBehaviour
{
    public string targetTag = "Player"
    public UnityEvent OnTriggerEnterEvent, OnTriggerEnterExit;

    private void OnTriggerEnterEvent(Collider2D collision){
        if (collision.gameObject.CompareTag(targetTag)) {
            onTriggerEnterEvent?.Invoke();
        }
    }

    private void OnTriggerExitEvent(Collider2D collision) {
        if(collision.gameObjectCompareTag(targetTag)){
            onTriggerExitEvent?.Invoke();
        }

    }
}
