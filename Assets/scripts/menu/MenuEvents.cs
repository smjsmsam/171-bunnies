using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEvents : MonoBehaviour
{
    public void Part1Button()
    {
        SceneManager.LoadScene(1);
    }
    public void Part2Button()
    {
        SceneManager.LoadScene(2);
    }
    public void Part3Button()
    {
        SceneManager.LoadScene(3);
    }
}
