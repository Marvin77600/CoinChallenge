using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneManager : MonoBehaviour
{
    Player player;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player = FindObjectOfType<Player>();
    }

    public void Retry()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Stop()
    {
        Application.Quit();
    }
}