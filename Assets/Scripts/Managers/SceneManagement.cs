
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    const float MAX_FADE_OPACITY = 1.0f;

    public enum Scenes
    {
        OVERWORLD,
        HOUSE,
        MENU,
        END_GAME
    }

    [Header("Fade")]
    [SerializeField] Image blackFadeObject;
    [SerializeField] float fadeDuration = 2.0f;
    private float fadeTimer;
    private float faceOpacity = 1.0f;

    private string sceneNameMenu = "MainMenu";
    private string sceneNameGame = "MainGame";
    private string sceneNameMapGeneration = "MapGeneration";
    private string sceneNameEndGame = "EndGame";

    private bool isChangingScene = false;
    private bool isLoadingScene = false;
    private Scenes nextScene;
    AsyncOperation asyncScene;

    //private GameManager gameManager;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        //gameManager = FindObjectOfType<GameManager>();
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
                sceneName = sceneNameMenu;
                break;
            case Scenes.HOUSE:
                sceneName = sceneNameGame;
                break;
            case Scenes.MENU:
                sceneName = sceneNameMapGeneration;
                break;
            case Scenes.END_GAME:
                sceneName = sceneNameEndGame;
                break;
        }
        return sceneName;
    }

    private void FadeAnimation(bool DoAddOpacity)
    {
        Color color = blackFadeObject.color;
        if (DoAddOpacity)
        {
            color.a = MAX_FADE_OPACITY - (MAX_FADE_OPACITY * Utility.GetTimerRemainingTime(fadeTimer) / fadeDuration);
        }
        else
        {
            color.a = MAX_FADE_OPACITY * Utility.GetTimerRemainingTime(fadeTimer) / fadeDuration;
        }
        blackFadeObject.color = color;
    }
}
