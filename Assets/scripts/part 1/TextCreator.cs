using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextCreator : MonoBehaviour
{
    public static TMPro.TMP_Text viewText;
    public static string transferText;
    public static bool runTextPrint;
    public static bool isSkipped;
    public static int charCount;
    [SerializeField] Coroutine rollText;
    void Update()
    {
        charCount = GetComponent<TMPro.TMP_Text>().text.Length;
        if (runTextPrint == true)
        {
            runTextPrint = false;
            viewText = GetComponent<TMPro.TMP_Text>();
            viewText.text = "";
            if (rollText != null)
            {
                StopCoroutine(rollText);
            }
            rollText = StartCoroutine(RollText());
        }
    }

    IEnumerator RollText()
    {
        foreach (char c in transferText)
        {
            if (isSkipped)
            {
                viewText.text = transferText;
                isSkipped = false;
                break;
            }
            viewText.text += c;
            yield return new WaitForSeconds(0.02f);
        }
    }
}
