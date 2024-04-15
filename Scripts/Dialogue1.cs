using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Dialogue1 : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] startupLines;
    string[] currentLines;
    public float textSpeed;

    public UnityEvent<int> LineComplete;

    [SerializeField] UnityEvent initialEvent;
    [SerializeField] UnityEvent[] dialogueEvents;
    

    private int index;
    // Start is called before the first frame update
    void Start()
    {
        BeginDialogue(startupLines);
        initialEvent.Invoke();
        LineComplete.AddListener((int line) => {
            if (line < dialogueEvents.Length)
                dialogueEvents[line].Invoke();
        });
    }

    public void BeginDialogue(string[] inputLines) {
        LineComplete.RemoveAllListeners();
        currentLines = inputLines;
        StopAllCoroutines();
        textComponent.text = string.Empty;
        if (currentLines.Length > 0) {
            gameObject.SetActive(true);
            StartDialogue();
        }
        else {
            gameObject.SetActive(false);
        }
    }

    public void ClearDialogue() {
        BeginDialogue(new string[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump"))
        {
            if(textComponent.text == currentLines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = currentLines[index];
                LineComplete.Invoke(index);
            }
        }
    }
    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }
    IEnumerator TypeLine()
    {
        foreach (char c in currentLines[index].ToCharArray())
        {
            yield return new WaitForSeconds(textSpeed);
            textComponent.text += c;
        }
        LineComplete.Invoke(index);
    }
    void NextLine()
    {
        if(index < currentLines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
