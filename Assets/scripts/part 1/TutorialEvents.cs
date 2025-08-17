using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialEvents : MonoBehaviour
{
    public GameObject nextButton;
    public GameObject previousButton;
    public GameObject display;
    [SerializeField] int eventPos;
    [SerializeField] Sprite[] displaySprites;

    private static string[] dialogue = {
        "Use the leaves <sprite=0> to progress through the tutorial!",
        "Hi friend! Today, we're going to play a game with bunnies and carrots to help us learn how to solve puzzles using something called MRV (that stands for Minimum Remaining Values), but don’t worry...we’ll explain it in a fun way!",
        "In our game, each bunny wants a carrot. But they’re a little picky… and a little jealous too! Some bunnies don’t like having the same color carrot as a nearby bunny. And each bunny has favorite carrot colors they will eat. Your job is to give every bunny a carrot they like, without making any bunny jealous. Let’s hop in!",
        "These are the pieces of the game. A bunny is a puzzle piece we need to figure out (a variable). A carrot color is a choice we can give the bunny (a value). And a line between bunnies is a rule: those bunnies can’t have the same colored carrot (a constraint).",
        "If we only have one bunny and one carrot color, it’s easy. Just give them that one carrot!",
        "Now we have two bunnies, but only one carrot color. The rule says they can’t have the same color...But uh-oh — that’s all we have!  That means there’s no way to make both bunnies happy. Sometimes puzzles don’t have a solution!",
        "Let’s try a bigger challenge. We’ve got 5 bunnies: Bunny A: only likes orange, Bunny B: likes orange and red, Bunny C: likes red and yellow, Bunny D: likes orange, red and yellow, Bunny E: also likes orange, red and yellow. Bunny A is near B and C, Bunny B is near Bunny D, and Bunny D is near Bunny E.",
        "Let’s use the MRV trick! Find the pickiest bunny (the one with the fewest choices) and start there. Bunny A is super picky since he only wants one carrot color: orange! So we start by giving A the orange carrot.",
        "Now let’s look at the bunnies that are near A...Bunny B is near A, so B can’t have an orange carrot now. But B still likes red, so we give B a red carrot.",
        "Bunny C was already fine since it didn’t want orange anyway. It still has two choices, red and yellow. D also only has two choices. Hmm, now we have a tie C, D, and all have two choices. To break the tie, we’ll go in alphabetical order. So C goes next! C isn’t near any bunny that has red, so we can give C the red carrot.",
        "Next is D. D can still have orange, so we give it that.",
        "And E? E takes the red carrot. Now all bunnies have a carrot they like, and no bunny is jealous!",
        "Notice that sometimes, there’s more than one way to solve the puzzle. Maybe you could’ve given E a yellow carrot too and that would’ve worked! But if you gave B the orange carrot at the start, the puzzle wouldn’t work out at all. That’s why MRV helps us make the smartest first choice!",
        "Now that you’ve seen an example of the MRV heuristic in action, try giving carrots to bunnies on your own! Just remember: Give the pickiest bunny their carrot first and update the carrots bunnies next to it won’t have. That way every bunny can eat a carrot they like and not get jealous. We want happy bunnies!",
        "Thanks for following along!"
    };

    void Start()
    {
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
        display.GetComponent<Image>().overrideSprite = displaySprites[eventPos];
        yield return SetDialogue(dialogue[eventPos]);
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
        StopAllCoroutines();
        StartCoroutine(EventCoroutine());
    }
}
