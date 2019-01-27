using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UIManager : MonoBehaviour
{
    [SerializeField]
    Image ImageACueillir;

    [SerializeField]
    Text QuantACueillir;

    [SerializeField]
    Text QuantCailloux;

    [SerializeField]
    DialoguesManager dialogueManager;

    [SerializeField]
    PlayerInventory playerInventory;


    [SerializeField]
    Sprite champignon;

    [SerializeField]
    Sprite baies;

    [SerializeField]
    Sprite brindilles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //décider quelle image mettre à chercher (champi, baies ou brindille)
        if (dialogueManager.dialogueEnCours().conditionEstChampignons()) {
            ImageACueillir.sprite = champignon;
            QuantACueillir.text = "x" + playerInventory.nombreChampignon();
        } else if (dialogueManager.dialogueEnCours().conditionEstBaies()) {
            ImageACueillir.sprite = baies;
            QuantACueillir.text = "x" + playerInventory.nombreBaies();
        } else if (dialogueManager.dialogueEnCours().conditionEstBrindilles()) {
            ImageACueillir.sprite = brindilles;
            QuantACueillir.text = "x" + playerInventory.nombreBrindille();
        }

        //trouver quantité cailloux
        QuantCailloux.text = "x" + playerInventory.nombreCailloux();
    }
}
