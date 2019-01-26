
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    static SceneManagement _instance;
    public static SceneManagement Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);
    }

    const float MAX_FADE_OPACITY = 1.0f;

    public enum Scenes
    {
        OVERWORLD,
        HOUSE,
        MENU,
        END_GAME
    }

    [Header("Fade")]
    [SerializeField] private Image blackFadeObject;
    [SerializeField] private float fadeDuration = 2.0f;
    private float fadeTimer;
    [SerializeField] private float faceOpacity = 1.0f;

    [Header("SceneName")]
    [SerializeField] private string sceneNameMenu = "Menu";
    [SerializeField] private string sceneNameOverworld = "SampleSceneDuncan";
    [SerializeField] private string sceneNameHouse = "SampleSceneDuncan";
    [SerializeField] private string sceneNameEndGame = "EndGame";

    private bool isChangingScene = false;
    private bool isLoadingScene = false;
    private Scenes nextScene;
    AsyncOperation asyncScene;

    // Use this for initialization
    void Start()
    {
        GameObject startPanel = GameObject.Find("StartPanel");
        if(startPanel != null)
            MenuManager.Instance.SetupMenuBtns(startPanel.transform, true);

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (isChangingScene)
        {
            if (!Utility.IsOver(fadeTimer))
            {
                blackFadeObject.gameObject.SetActive(true);
                FadeAnimation(true);
            }
            else if (!isLoadingScene)
            {
                asyncScene = SceneManager.LoadSceneAsync(GetSceneName(nextScene));
                isLoadingScene = true;
            }
            else if (asyncScene.isDone)
            {
                Debug.Log("SetupScene : " + GetSceneName(nextScene));
                //gameManager.SetupScene(nextScene);
                isChangingScene = false;
                isLoadingScene = false;
                fadeTimer = Utility.StartTimer(fadeDuration);
            }
        }
        else if (!Utility.IsOver(fadeTimer))
        {
            FadeAnimation(false);
        }
        else
        {
            blackFadeObject.gameObject.SetActive(false);
        }
    }

    public void ChangeScene(Scenes scene)
    {
        if (!isChangingScene)
        {
            nextScene = scene;
            isChangingScene = true;
            fadeTimer = Utility.StartTimer(fadeDuration);
        }
    }

    private string GetSceneName(Scenes scene)
    {
        string sceneName = "";
        switch (scene)
        {
            case Scenes.OVERWORLD:
                sceneName = sceneNameOverworld;
                break;
            case Scenes.HOUSE:
                sceneName = sceneNameHouse;
                break;
            case Scenes.MENU:
                sceneName = sceneNameMenu;
                break;
            case Scenes.END_GAME:
                sceneName = sceneNameEndGame;
                break;
        }
        return sceneName;
    }

    private void FadeAnimation(bool doAddOpacity)
    {
        Color color = blackFadeObject.color;
        if (doAddOpacity)
            color.a = MAX_FADE_OPACITY - (MAX_FADE_OPACITY * Utility.GetTimerRemainingTime(fadeTimer) / fadeDuration);
        else
            color.a = MAX_FADE_OPACITY * Utility.GetTimerRemainingTime(fadeTimer) / fadeDuration;
        blackFadeObject.color = color;
    }

    private void SetupScene(Scenes sceneName)
    {
        switch (sceneName)
        {
            case Scenes.HOUSE:
                break;
            case Scenes.MENU:
                break;
            case Scenes.OVERWORLD:
                break;
            case Scenes.END_GAME:
                break;
        }
    }
}
