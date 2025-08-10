using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MarvinSprite : MonoBehaviour
{
    public static bool isSpeaking;
    [SerializeField] Sprite[] speakingSprites;
    [SerializeField] Sprite newSprite;
    private static bool startedSpeaking;
    private Coroutine speaking;

    void Update()
    {
        if (isSpeaking)
        {
            if (!startedSpeaking)
            {
                speaking = StartCoroutine(Speak());
                startedSpeaking = true;
            }
        }
        else if (speaking != null)
        {
            StopCoroutine(speaking);
            gameObject.GetComponent<Image>().overrideSprite = speakingSprites[0];
            startedSpeaking = false;
        }
    }

    IEnumerator Speak()
    {
        while (true)
        {
            gameObject.GetComponent<Image>().overrideSprite = speakingSprites[1];
            yield return new WaitForSeconds(0.3f);
            gameObject.GetComponent<Image>().overrideSprite = speakingSprites[0];
            yield return new WaitForSeconds(0.3f);
        }
    }
}
