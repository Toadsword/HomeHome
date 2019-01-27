using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{

    [SerializeField] GameObject menuOptions;

    [SerializeField] Button btnResume;
    [SerializeField] Button btnNewGame;
    [SerializeField] Button btnQuit;

    SoundManager soundManager=null;

    // Start is called before the first frame update
    void Start() {
        soundManager = SoundManager.Instance;

        btnQuit.onClick.AddListener(() => {
            soundManager.PlaySound(SoundManager.SoundList.CLIC);
            Application.Quit();

        });

        btnNewGame.onClick.AddListener(() => {
            //?
            soundManager.PlaySound(SoundManager.SoundList.CLIC);
        });

        btnResume.onClick.AddListener(() => {
            //switcher menu pause
            menuOptions.SetActive(!menuOptions.activeSelf);
            soundManager.PlaySound(SoundManager.SoundList.CLIC);
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            //switcher menu pause
            menuOptions.SetActive(!menuOptions.activeSelf);
            soundManager.PlaySound(SoundManager.SoundList.CLIC);
        }
    }
}
