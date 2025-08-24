using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class NodeManager : MonoBehaviour
{
    public GameObject nodePrefab;
    public GameObject linePrefab;
    public Canvas canvas;
    // private GameObject[] nodes;
    private char currentLetter = 'A';
    private RectTransform startNode;
    private UILine currentLine;
    private Vector2 startPos;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        // if (Mouse.current.leftButton.wasPressedThisFrame)
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
            // nodes.Append(node);
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Mouse down");
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Mouse.current.position.ReadValue();

            var results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (var r in results)
            {
                GameObject nodeRoot = GetRootNode(r.gameObject);
                if (nodeRoot != null)
                {
                    startNode = nodeRoot.GetComponent<RectTransform>();

                    GameObject lineObj = Instantiate(linePrefab, canvas.transform);
                    currentLine = lineObj.GetComponent<UILine>();

                    startPos = canvas.GetComponent<RectTransform>()
                                        .InverseTransformPoint(startNode.position);
                    currentLine.SetPositions(startPos, startPos);

                    break;
                }
            }
        }

        if (currentLine != null && Mouse.current.leftButton.isPressed)
        {
            Vector2 mouseLocalPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.GetComponent<RectTransform>(),
                Mouse.current.position.ReadValue(),
                null,
                out mouseLocalPos
            );

            currentLine.SetPositions(startPos, mouseLocalPos);
        }

        if (currentLine != null && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            RectTransform endNode = null;

            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Mouse.current.position.ReadValue();

            var results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (var r in results)
            {
                GameObject nodeRoot = GetRootNode(r.gameObject);

                if (nodeRoot != null && nodeRoot.GetComponent<RectTransform>() != startNode)
                {
                    endNode = nodeRoot.GetComponent<RectTransform>();
                    break;
                }
            }

            if (endNode != null)
            {
                Vector2 startPos = canvas.GetComponent<RectTransform>()
                                        .InverseTransformPoint(startNode.position);
                Vector2 endPos   = canvas.GetComponent<RectTransform>()
                                        .InverseTransformPoint(endNode.position);

                currentLine.SetPositions(startPos, endPos);
            }
            else
            {
                Destroy(currentLine.gameObject);
                startPos = Vector2.zero;
            }

            currentLine = null;
            startNode = null;
        }
    }

    Vector2 CanvasPositionFromNode(RectTransform node, RectTransform canvas)
    {
        Vector3 worldPos = node.position; // node’s world position
        Vector2 localPos = canvas.InverseTransformPoint(worldPos);
        return localPos;
    }
    
    GameObject GetRootNode(GameObject clicked)
{
    Transform t = clicked.transform;
    while (t != null)
    {
        if (t.CompareTag("Node"))
            return t.gameObject;
        t = t.parent;
    }
    return null;
}
}
