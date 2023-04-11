using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class InGameMenuController : MonoBehaviour
{
    private bool isMenuActive;
    public GameObject inGameMenu;
    public Button mainMenu;
    private void Start()
    {
        isMenuActive = false;
        inGameMenu.SetActive(false);
        mainMenu.onClick.AddListener(BackToMainMenu);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleInGameMenu();
        }
    }

    private void ToggleInGameMenu()
    {
        isMenuActive = !isMenuActive;
        inGameMenu.SetActive(isMenuActive);
        Time.timeScale = isMenuActive ? 0 : 1;
    }

    public void BackToMainMenu()
    {
        Debug.Log("Test");
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
