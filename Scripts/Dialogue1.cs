using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue1 : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] startupLines;
    string[] currentLines;
    public float textSpeed;

    private int index;
    // Start is called before the first frame update
    void Start()
    {
        BeginDialogue(startupLines);
    }

    public void BeginDialogue(string[] inputLines) {
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(textComponent.text == currentLines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = currentLines[index];
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
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed)
;        }
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
