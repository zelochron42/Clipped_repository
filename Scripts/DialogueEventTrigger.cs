using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueEventTrigger : PhysicalTrigger
{
    Dialogue1 dialogueHandler;
    [SerializeField] UnityEvent[] eventList;
    private void Awake() {
        dialogueHandler = FindObjectOfType<Dialogue1>();
        
    }
    protected override void OnTriggerEnter2D(Collider2D collision) {
        bool disableOnFinish = false;
        if (singleUse) {
            disableOnFinish = true;
            singleUse = false;
        }
        base.OnTriggerEnter2D(collision);
        if (dialogueHandler) {
            dialogueHandler.LineComplete.AddListener((int line) => {
                if (line < eventList.Length)
                    eventList[line].Invoke();
            });
        }
        if (disableOnFinish)
            GetComponent<Collider2D>().enabled = false;
    }
}
