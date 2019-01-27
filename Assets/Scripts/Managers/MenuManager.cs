using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    static MenuManager _instance;
    public static MenuManager Instance
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
    
    // Menu gestion
    public Selectable[] menuBtns;
    public int selectedIndex = 0;
    public bool isInMenu = false;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (isInMenu)
        {
            if (GameInput.GetInputDown(GameInput.InputType.LEFT) || GameInput.GetInputDown(GameInput.InputType.RIGHT))
            {
                NavigateMenu(GameInput.GetInputDown(GameInput.InputType.LEFT));
            }
        }
    }


    public void NavigateMenu(bool onLeft)
    {
        Debug.Log("NavigateMenu : " + onLeft);
        if (onLeft)
        {
            selectedIndex--;
            if (selectedIndex == -1)
                selectedIndex = 0;
            //else
            //    SoundManager._instance.PlaySound(SoundManager.SoundList.MENU_SELECTION);
        }
        else
        {
            selectedIndex++;
            if (selectedIndex == menuBtns.Length)
                selectedIndex = menuBtns.Length - 1;
            //else
            //    SoundManager._instance.PlaySound(SoundManager.SoundList.MENU_SELECTION);
        }
        menuBtns[selectedIndex].Select();
        
    }

    public void SelectButton(string name)
    {
        int newIndex = 0;
        foreach (Selectable btn in menuBtns)
        {
            //Debug.Log(" NAME ; " + btn.name + " ;WANTED : " + name);
            if (btn.name == name)
            {
                selectedIndex = newIndex;
                break;
            }
            newIndex++;
        }
        menuBtns[selectedIndex].Select();
    }

    public void SubmitButtonAction()
    {
        Transform panel;
        //Debug.Log(menuBtns[selectedIndex].name);
        switch (menuBtns[selectedIndex].name)
        {
            case "StartButton":
                SceneManagement.Instance.ChangeScene(SceneManagement.Scenes.HOUSE);
                break;
            case "ButtonCredits":
                GameObject creditPanelObject = GameObject.Find("Canvas").transform.Find("CreditPanel").gameObject;
                creditPanelObject.SetActive(!creditPanelObject.activeSelf);
                break;
            case "ToMenu":
                SceneManagement.Instance.ChangeScene(SceneManagement.Scenes.MENU);
                break;
            case "QuitButton":
                Application.Quit();
                break;
        }
        //SoundManager._instance.PlaySound(SoundManager.SoundList.MENU_VALIDATION);
    }

    public void SetupMenuBtns(Transform panelMenu, bool activate)
    {
        panelMenu.gameObject.SetActive(activate);

        isInMenu = activate;

        selectedIndex = 0;

        if (activate)
        {
            menuBtns = panelMenu.GetComponent<MenuActions>().menuBtns;
            menuBtns[selectedIndex].Select();
        }
        else
            menuBtns = null;
    }
}
