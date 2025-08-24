using TMPro;
using UnityEngine;

public class NodeView : MonoBehaviour
{
    public GameObject nodeName;
    public GameObject bunny;
    public GameObject oCarrot;
    public GameObject rCarrot;
    public GameObject yCarrot;

    void updateName(string newName)
    {
        nodeName.GetComponent<TMP_Text>().text = newName;
    }

}
