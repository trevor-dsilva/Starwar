using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Menu class used on the Start scene. It assigns TaskOnClick to a button and, when pressed, loads the game scene, where the game starts.
public class Menu : MonoBehaviour
{
    public Button start;
    public Button exit;

    private void Start()
    {
        start.onClick.AddListener(TaskOnClick);
        exit.onClick.AddListener(EndGame);

    }
    // Update is called once per frame
    public void TaskOnClick()
    {
        SceneManager.LoadScene(1);
    }

    public void EndGame()
    {
        Application.Quit();
    }
}