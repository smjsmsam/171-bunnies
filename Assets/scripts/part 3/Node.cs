using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Node : MonoBehaviour
{
    public GameObject nodeName;
    public GameObject bunny;
    public GameObject oCarrot;
    public GameObject rCarrot;
    public GameObject yCarrot;
    public List<Node> neighbors = new List<Node>();

    private void Awake()
    {
        neighbors = new List<Node>();
    }

}
