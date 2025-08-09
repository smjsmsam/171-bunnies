using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(EventStarter());
    }
    IEnumerator EventStarter()
    {
        yield return new WaitForSeconds(1);
        // text function
    }
}
