using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QTE : MonoBehaviour
{
    public TextMeshProUGUI DisplayBox;
    public TextMeshProUGUI PassBox;
    public int QTEGen;
    public int WaitingForKey;
    public bool CorrectKey;
    public int CountingDown;
    void Start()
    {
        /*DisplayBox = FindObjectOfType<TextMeshProUGUI>();
        PassBox = FindObjectOfType<TextMeshProUGUI>();*/

    }
    void Update()
    {
        if(WaitingForKey == 0)
        {
            QTEGen = Random.Range(1, 4);
            CountingDown = 1;
            StartCoroutine(CountDown());
            if(QTEGen == 1)
            {
                WaitingForKey = 1;
                DisplayBox.text = "[E]";
            }
            if (QTEGen == 2)
            {
                WaitingForKey = 1;
                DisplayBox.text = "[R]";
            }
            if (QTEGen == 3)
            {
                WaitingForKey = 1;
                DisplayBox.text = "[T]";
            }
        }
        if(QTEGen == 1)
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetButton("E"))
                {
                    CorrectKey = true;
                    Debug.Log("Correct!");
                    StartCoroutine(KeyPressing());
                    
                }
                else
                {
                    CorrectKey = false;
                    StartCoroutine(KeyPressing());
                    Debug.Log("INCorrect!");
                }
            }
        }
        if (QTEGen == 2)
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetButton("R"))
                {
                    CorrectKey = true;
                    Debug.Log("Correct!");
                    StartCoroutine(KeyPressing());
                }
                else
                {
                    CorrectKey = false;
                    StartCoroutine(KeyPressing());
                    Debug.Log("INCorrect!");
                }
            }
        }
        if (QTEGen == 3)
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetButton("T"))
                {
                    CorrectKey = true;
                    Debug.Log("Correct!");
                    StartCoroutine(KeyPressing());
                }
                else
                {
                    CorrectKey = false;
                    StartCoroutine(KeyPressing());
                    Debug.Log("INCorrect!");
                }
            }
        }
    }
    IEnumerator KeyPressing()
    {
        QTEGen = 4;
        if(CorrectKey == true)
        {
            CountingDown = 2;
            PassBox.text = "PASS!";
            yield return new WaitForSeconds(1.5f);
            CorrectKey = false;
            PassBox.text = "";
            DisplayBox.text = "";
            yield return new WaitForSeconds(1.5f);
            WaitingForKey = 0;
            CountingDown = 1;
        }
        else if (CorrectKey == false)
        {
            CountingDown = 2;
            PassBox.text = "FAIL!";
            yield return new WaitForSeconds(1.5f);
            CorrectKey = false;
            PassBox.text = "";
            DisplayBox.text = "";
            yield return new WaitForSeconds(1.5f);
            WaitingForKey = 0;
            CountingDown = 1;
        }
    }
    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(3.5f);
        if(CountingDown == 1)
        {
           QTEGen = 4;
            CountingDown = 2;
            PassBox.text = "";
            yield return new WaitForSeconds(1.5f);
            CorrectKey = false;
            PassBox.text = "";
            DisplayBox.text = "";
            yield return new WaitForSeconds(1.5f);
            WaitingForKey = 0;
            CountingDown = 1;
        }
    }
}
