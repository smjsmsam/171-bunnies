using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEvents : MonoBehaviour
{
    public GameObject nextButton;
    public GameObject previousButton;
    [SerializeField] int eventPos;

    void Start()
    {
        StartCoroutine(EventStarter());
    }

    IEnumerator EventStarter()
    {
        yield return new WaitForSeconds(1.5f);
        yield return SetDialogue("Can I interest you in extending your car's warranty?");
        eventPos = 1;
    }

    IEnumerator EventOne()
    {
        yield return SetDialogue("Can I interest you in extending your car's warranty?");
    }

    IEnumerator EventTwo()
    {
        yield return SetDialogue("Twinkle twinkle little star");
    }

    IEnumerator SetDialogue(string transferText)
    {
        TextCreator.transferText = transferText;
        TextCreator.runTextPrint = true;
        yield return new WaitForSeconds(1.05f);
        yield return new WaitUntil(() => TextCreator.charCount == transferText.Length);
        yield return new WaitForSeconds(0.5f);
    }

    public void NextButton()
    {
        eventPos++;
        EventMaster();
    }

    public void PreviousButton()
    {
        if (eventPos != 1)
        {
            eventPos--;
            EventMaster();
        }
    }

    public void SkipTextAnimation()
    {
        TextCreator.isSkipped = true;
    }

    public void EventMaster()
    {
        if (eventPos == 1)
        {
            StartCoroutine(EventOne());
        }
        else if (eventPos == 2)
        {
            StartCoroutine(EventTwo());
        }
    }
}
