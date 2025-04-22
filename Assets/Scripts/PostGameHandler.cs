using UnityEngine;
using UnityEngine.SceneManagement;

public class PostGameHandler : MonoBehaviour
{
    bool lost = true;
    bool ac = false;
    public void Lose()
    {
        ac = true;
    }
    public void Win()
    {
        if (PlayerPrefs.GetInt("MapsBeat", 0) < SceneManager.GetActiveScene().buildIndex)
        {
            PlayerPrefs.SetInt("MapsBeat", PlayerPrefs.GetInt("MapsBeat", 0)+1);
            PlayerPrefs.Save();
        }
        lost = false;
        ac = true;
    }

    public void Update()
    {
        if (!ac) return;
        if (lost)
        {
            if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SceneManager.LoadScene("MainMenu");
        }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("MainMenu");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SceneManager.LoadScene("MainMenu");
        }
        }
    }
}
