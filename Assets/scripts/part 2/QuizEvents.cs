using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuizEvents : MonoBehaviour
{
    public GameObject nextButton;
    public GameObject display;
    public GameObject colorOptions;
    public GameObject classOptions;
    public GameObject hintButton;
    public GameObject aButton;
    public GameObject bButton;
    public GameObject cButton;
    public GameObject dButton;
    public GameObject eButton;
    public GameObject oButton;
    public GameObject rButton;
    public GameObject yButton;
    [SerializeField] int eventPos;
    [SerializeField] Sprite[] displaySprites;
    [SerializeField] Sprite[] hintSprites;
    private string buttonPressed = "";

    private static string[] dialogue = {
        "It’s your turn now! We’re going to go through an example and test your understanding.",
        "Which bunny do you give a carrot to first?",
        "The answer is A because it only wants 1 carrot color, the orange one.",
        "What color can Bunny B NOT get?",
        "Orange because Bunny A was given Orange.",
        "What color can Bunny C get?",
        "Red and Yellow because Bunny A was given Orange.",
        "Which bunny do you give a carrot to next?",
        "The answer is C because it only has one option, Red.",
        "What color can Bunny B get?",
        "Yellow because Bunny C was given Red.",
        "What color can Bunny D NOT get?",
        "Red because Bunny C was given Red.",
        "Which bunny has the least amount of options?",
        "Bunny B and D both have one option.",
        "Which bunny comes first alphabetically?",
        "That’s right! Bunny B.",
        "Which bunny do you give a carrot to next?",
        "Bunny B comes first alphabetically, we’ll give it its carrot first.",
        "Which bunny do you give a carrot to next?",
        "Bunny D has only one option, Yellow.",
        "What color options are available for the last bunny?",
        "Orange and Red since Bunny D has the Yellow carrot.",
        "Which bunny do you give a carrot to next?",
        "Bunny E is the last one to give a carrot to.",
        "Now all the bunnies are happy! Good work!"
    };
    private Dictionary<int, List<string>> correctAnswer = new Dictionary<int, List<string>>
    {
        {1, new List<string>{"A", "Class", "0", "112" } },
        {3, new List<string>{"O", "Color", "1"} },
        {5, new List<string>{"RY", "Color", "2"} },
        {7, new List<string>{"C", "Class", "3"} },
        {9, new List<string>{"Y", "Color", "4"} },
        {11, new List<string>{"R", "Color", "5" } },
        {13, new List<string>{"BD", "Class", "6" } },
        {15, new List<string>{"B", "Class", "7"} },
        {17, new List<string>{"B", "Class", "8" } },
        {19, new List<string>{"D", "Class", "9"} },
        {21, new List<string>{"OR", "Color", "10" } },
        {23, new List<string>{"E", "Class", "11" } }
    };

    void Start()
    {
        hintButton.SetActive(false);
        colorOptions.SetActive(false);
        classOptions.SetActive(false);
        EventMaster();
    }

    IEnumerator EventCoroutine()
    {
        if (eventPos == 0)
        {
            yield return new WaitForSeconds(1);
        }
        if (eventPos == displaySprites.Length)
        {
            SceneManager.LoadScene(0);
        }
        if (eventPos % 2 == 0 || eventPos == displaySprites.Length - 1)
        {
            display.GetComponent<Image>().overrideSprite = displaySprites[eventPos];
            yield return SetDialogue(dialogue[eventPos]);
        }
        else
        {
            if (buttonPressed == "")
            {
                nextButton.SetActive(false);
                hintButton.SetActive(true);
                display.GetComponent<Image>().overrideSprite = displaySprites[eventPos];
                yield return SetDialogue(dialogue[eventPos]);
                if (correctAnswer[eventPos][1] == "Color")
                {
                    colorOptions.SetActive(true);
                }
                else
                {
                    classOptions.SetActive(true);
                }
            }
            else
            {
                bool correct = true;
                String currentCorrect = correctAnswer[eventPos][0];
                if (buttonPressed.Length == currentCorrect.Length)
                {
                    foreach (char c in buttonPressed)
                    {
                        if (!correctAnswer[eventPos][0].Contains(c))
                        {
                            correct = false;
                            break;
                        }
                    }
                }
                else
                {
                    correct = false;
                }
                if (correct)
                {
                    nextButton.SetActive(true);
                    hintButton.SetActive(false);
                    buttonPressed = "";
                    if (correctAnswer[eventPos][1] == "Color")
                    {
                        colorOptions.SetActive(false);
                        oButton.GetComponent<Shadow>().enabled = false;
                        rButton.GetComponent<Shadow>().enabled = false;
                        yButton.GetComponent<Shadow>().enabled = false;
                    }
                    else
                    {
                        classOptions.SetActive(false);
                        aButton.GetComponent<Shadow>().enabled = false;
                        bButton.GetComponent<Shadow>().enabled = false;
                        cButton.GetComponent<Shadow>().enabled = false;
                        dButton.GetComponent<Shadow>().enabled = false;
                        eButton.GetComponent<Shadow>().enabled = false;
                    }
                    eventPos++;
                    HintButton();
                    EventMaster();
                }
            }
        }
    }

    IEnumerator SetDialogue(string transferText)
    {
        TextCreator.transferText = transferText;
        TextCreator.runTextPrint = true;
        MarvinSprite.isSpeaking = true;
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => TextCreator.charCount == transferText.Length);
        MarvinSprite.isSpeaking = false;
        yield return new WaitForSeconds(0.5f);
    }

    public void NextButton()
    {
        if (eventPos < displaySprites.Length)
        {
            eventPos++;
            EventMaster();
        }
    }

    public void AButton()
    {
        if (buttonPressed.Contains("A"))
        {
            buttonPressed = buttonPressed.Replace("A", String.Empty);
            aButton.GetComponent<Shadow>().enabled = false;
        }
        else
        {
            buttonPressed += "A";
            aButton.GetComponent<Shadow>().enabled = true;
        }
        EventMaster();
    }

    public void BButton()
    {
        if (buttonPressed.Contains("B"))
        {
            buttonPressed = buttonPressed.Replace("B", String.Empty);
            bButton.GetComponent<Shadow>().enabled = false;
        }
        else
        {
            buttonPressed += "B";
            bButton.GetComponent<Shadow>().enabled = true;
        }
        EventMaster();
    }

    public void CButton()
    {
        if (buttonPressed.Contains("C"))
        {
            buttonPressed = buttonPressed.Replace("C", String.Empty);
            cButton.GetComponent<Shadow>().enabled = false;
        }
        else
        {
            buttonPressed += "C";
            cButton.GetComponent<Shadow>().enabled = true;
        }
        EventMaster();
    }

    public void DButton()
    {
        if (buttonPressed.Contains("D"))
        {
            buttonPressed = buttonPressed.Replace("D", String.Empty);
            dButton.GetComponent<Shadow>().enabled = false;
        }
        else
        {
            buttonPressed += "D";
            dButton.GetComponent<Shadow>().enabled = true;
        }
        EventMaster();
    }

    public void EButton()
    {
        if (buttonPressed.Contains("E"))
        {
            buttonPressed = buttonPressed.Replace("E", String.Empty);
            eButton.GetComponent<Shadow>().enabled = false;
        }
        else
        {
            buttonPressed += "E";
            eButton.GetComponent<Shadow>().enabled = true;
        }
        EventMaster();
    }

    public void OButton()
    {
        if (buttonPressed.Contains("O"))
        {
            buttonPressed = buttonPressed.Replace("O", String.Empty);
            oButton.GetComponent<Shadow>().enabled = false;
        }
        else
        {
            buttonPressed += "O";
            oButton.GetComponent<Shadow>().enabled = true;
        }
        EventMaster();
    }

    public void RButton()
    {
        if (buttonPressed.Contains("R"))
        {
            buttonPressed = buttonPressed.Replace("R", String.Empty);
            rButton.GetComponent<Shadow>().enabled = false;
        }
        else
        {
            buttonPressed += "R";
            rButton.GetComponent<Shadow>().enabled = true;
        }
        EventMaster();
    }


    public void YButton()
    {
        if (buttonPressed.Contains("Y"))
        {
            buttonPressed = buttonPressed.Replace("Y", String.Empty);
            yButton.GetComponent<Shadow>().enabled = false;
        }
        else
        {
            buttonPressed += "Y";
            yButton.GetComponent<Shadow>().enabled = true;
        }
        EventMaster();
    }

    public void HintButton()
    {
        Sprite newSprite;
        if (eventPos % 2 != 0)
        {
            newSprite = hintSprites[Int32.Parse(correctAnswer[eventPos][2])];
        }
        else
        {
            newSprite = hintSprites[12];
        }
        hintButton.GetComponent<Image>().sprite = newSprite;
        hintButton.GetComponent<RectTransform>().sizeDelta = new Vector2(newSprite.rect.width, newSprite.rect.height);
    }

    public void SkipTextAnimation()
    {
        TextCreator.isSkipped = true;
    }

    public void EventMaster()
    {
        StopAllCoroutines();
        StartCoroutine(EventCoroutine());
    }
}
