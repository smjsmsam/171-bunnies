using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.Collections;

public class NodeManager : MonoBehaviour
{
    public GameObject nodePrefab;
    public GameObject linePrefab;
    public Canvas canvas;
    public List<Node> allNodes = new List<Node>();
    private char currentLetter = 'A';
    public RectTransform startNode;
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
            Node nodeScript = node.GetComponent<Node>();
            allNodes.Add(nodeScript);
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

        if (endNode == null || endNode == startNode)
        {
            Destroy(currentLine.gameObject);
            startPos = Vector2.zero;
        }
        else
        {
            Node startNodeScript = startNode.GetComponent<Node>();
            Node endNodeScript = endNode.GetComponent<Node>();
            
            bool alreadyConnected = startNodeScript.neighbors.Contains(endNodeScript) || endNodeScript.neighbors.Contains(startNodeScript);
            
            if (alreadyConnected)
            {
                Destroy(currentLine.gameObject);
                startPos = Vector2.zero;
            }
            else
            {
                startNodeScript.neighbors.Add(endNodeScript);
                endNodeScript.neighbors.Add(startNodeScript);
                
                Vector2 startPos = GetRectEdgePoint(startNode, endNode.anchoredPosition, 20f);
                Vector2 endPos = GetRectEdgePoint(endNode, startNode.anchoredPosition, 20f);
                currentLine.SetPositions(startPos, endPos);
            }
        }

            currentLine = null;
            startNode = null;
        }
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

    Vector2 GetRectEdgePoint(RectTransform rect, Vector2 target, float padding = 0f)
    {
        Vector2 center = rect.anchoredPosition;
        Vector2 halfSize = rect.rect.size * 0.5f;

        Vector2 dir = target - center;

        if (dir == Vector2.zero)
        {
            return center;
        }

        float scaleX = halfSize.x / Mathf.Abs(dir.x);
        float scaleY = halfSize.y / Mathf.Abs(dir.y);
        float scale = Mathf.Min(scaleX, scaleY);

        Vector2 edgePoint = center + dir * scale;
        edgePoint += dir.normalized * padding;

        return edgePoint;
    }

}
