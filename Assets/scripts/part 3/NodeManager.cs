using UnityEngine.InputSystem;

using UnityEngine;
using System.Linq;
using System;

public class NodeManager : MonoBehaviour
{
    public GameObject nodePrefab;
    public Canvas canvas;
    private GameObject[] nodes;
    private char currentLetter = 'A';

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                mousePos,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out Vector2 localPoint
            );

            GameObject node = Instantiate(nodePrefab, canvas.transform);
            node.GetComponent<RectTransform>().anchoredPosition = localPoint;
            node.transform.GetChild(5).gameObject.GetComponent<TMPro.TMP_Text>().text = Char.ToString(currentLetter);
            currentLetter++;
            nodes.Append(node);
        }
    }
}
