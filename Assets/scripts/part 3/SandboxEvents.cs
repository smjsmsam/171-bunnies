using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SandboxEvents : MonoBehaviour
{
    public GameObject nextButton;
    public GameObject previousButton;
    public GameObject numberOptions;
    public GameObject oneButton;
    public GameObject twoButton;
    public GameObject threeButton;
    public GameObject fourButton;
    public GameObject fiveButton;
    public GameObject sixButton;
    [SerializeField] int eventPos;
    public GameObject nodePrefab;
    public GameObject linePrefab;
    public Canvas canvas;
    public List<Node> allNodes = new List<Node>();
    private char currentLetter = 'A';
    public RectTransform startNode;
    private UILine currentLine;
    private Vector2 startPos;

    private static string[] dialogue = {
        "Now that you understand how MRV heuristics work, make your own problem now!",
        "Start by selecting the number of bunnies you want.",
        "Now let's make neighbors! Drag your mouse from bunny to bunny to connect them. Click <sprite=0> when you're finished!",
        "Alright, let's choose what color each bunny prefers. Be careful here! If it's unsolvable, you'll have to start all over again."
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
        if (eventPos == dialogue.Length)
        {
            SceneManager.LoadScene(0);
        }
        if (eventPos == 1)
        {
            numberOptions.SetActive(true);
            nextButton.SetActive(false);
        }
        else if (eventPos == 2)
        {
            nextButton.SetActive(true);
            StartCoroutine(DragToConnect());
        }

        yield return SetDialogue(dialogue[eventPos]);
    }

    void CreateNodeAt(int x, int y)
    {
        Vector2 point = new Vector2(x, y);
        GameObject node = Instantiate(nodePrefab, canvas.transform);
        node.GetComponent<RectTransform>().anchoredPosition = point;
        node.transform.GetChild(5).gameObject.GetComponent<TMPro.TMP_Text>().text = Char.ToString(currentLetter);
        currentLetter++;
        Node nodeScript = node.GetComponent<Node>();
        allNodes.Add(nodeScript);
    }

    IEnumerator DragToConnect()
    {
        while (true)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Debug.Log("Mouse down");
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                pointerData.position = Mouse.current.position.ReadValue();

                var results = new List<RaycastResult>();
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

                var results = new List<RaycastResult>();
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
            yield return null;
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

    public void OneButton()
    {
        numberOptions.SetActive(false);
        CreateNodeAt(0, 175);
        eventPos++;
        EventMaster();
    }

    public void TwoButton()
    {
        numberOptions.SetActive(false);
        CreateNodeAt(-244, 175);
        CreateNodeAt(244, 175);
        eventPos++;
        EventMaster();
    }

    public void ThreeButton()
    {
        numberOptions.SetActive(false);
        CreateNodeAt(-244, 68);
        CreateNodeAt(244, 68);
        CreateNodeAt(0, 301);
        eventPos++;
        EventMaster();
    }

    public void FourButton()
    {
        numberOptions.SetActive(false);
        CreateNodeAt(-295, 147);
        CreateNodeAt(295, 147);
        CreateNodeAt(0, 339);
        CreateNodeAt(0, 3);
        eventPos++;
        EventMaster();
    }

    public void FiveButton()
    {
        numberOptions.SetActive(false);
        CreateNodeAt(-345, 219);
        CreateNodeAt(345, 219);
        CreateNodeAt(0, 339);
        CreateNodeAt(168, 3);
        CreateNodeAt(-168, 3);
        eventPos++;
        EventMaster();
    }

    public void SixButton()
    {
        numberOptions.SetActive(false);
        CreateNodeAt(-451, 180);
        CreateNodeAt(451, 180);
        CreateNodeAt(-168, 374);
        CreateNodeAt(168, 374);
        CreateNodeAt(-168, -20);
        CreateNodeAt(168, -20);
        eventPos++;
        EventMaster();
    }

    public void NextButton()
    {
        if (eventPos < dialogue.Length)
        {
            eventPos++;
            EventMaster();
        }
    }

    public void PreviousButton()
    {
        if (eventPos >= 1)
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
