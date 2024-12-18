using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.VFX;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [field: Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private AudioClip buttonClickClip;


    [field: Header("In Scene Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingMenu;

    void Awake()
    {
        startButton.onClick.AddListener(OnStartButtonClick);
        settingButton.onClick.AddListener(OnSettingButtonClick);
    }

    void OnStartButtonClick()
    {
        Debug.Log("Going to Start menu");
        SceneManager.LoadScene("Tutorial");
    }

    void OnSettingButtonClick()
    {
        settingMenu.SetActive(true);
    }
}
