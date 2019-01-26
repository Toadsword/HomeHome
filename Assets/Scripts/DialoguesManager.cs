using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialoguesManager : MonoBehaviour {

    [SerializeField]
    Image boiteDialogue;
    [SerializeField]
    Text texteBoiteDialogue;


    [SerializeField]
    int champignons;

    [SerializeField]
    int brindilles;

    [SerializeField]
    int baies;

    int dialogue_courant;//indice du tableau des dialogues
    List<Dialogue> dialogues;

    //gestion animations
    enum Etat { ouvert, fermé, enOuverture, enFermeture, enEcriture};
    Etat etat;

    Vector2 tailleInitialeBoite;
    float timer_boite;
    float duree_boite = 0.15f;

    float timer_ecriture;
    float duree_ecriture = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        dialogue_courant = 0;
        etat = Etat.fermé;
        boiteDialogue.gameObject.SetActive(false);

        creerDialogues();

        timer_boite = 0;
        tailleInitialeBoite = boiteDialogue.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space)) {
            lancerDialogue();
        }

        gestionAnimations();
    }

    void creerDialogues() {
        Dialogue dialogue1 = new Dialogue(new List<string>
        {
            "Comme je suis ravie que tu rendes visite à ta grand-mère, Rouge.",
            "Je comptais te préparer une bonne soupe chaude pour te revigorer du froid hivernale.",
            "Cela te ferait du bien, tu es toute maigrelette, mais je n'ai plus de champignons...",
            "Peux-tu aller en chercher 3 pendant que je prépare le potage ?",
            "N'oublie pas de prendre la lanterne, il fait noir dehors."
        }, 3, 0, 0);

        Dialogue dialogue2 = new Dialogue(new List<string>
        {
            "Merci beaucoup mon sucre d'orge ! J'espère que tu n'as pas pris froid !",
            "L'odeur du potage te plait ? C'est celui que tu préfères.",
            "Flûte de zut, le feu s'affaiblit !",
            "Va donc chercher 4 morceaux de bois pendant que les champignons mijotent."
        }, 0, 4, 0);

        Dialogue dialogue3 = new Dialogue(new List<string>
        {
            "Te voilà, Rouge ! J'allais m'inquiéter.",
            "Heureusement, c'est un bois très tranquille. Et magnifique en pleine journée.",
            "J'ai une idée, et si on faisait plus de soupe pour ta mère, elle sera contente !",
            "Je pense qu'avec 5 champignons supplémentaires, il y aura assez."
        }, 5, 0, 0);

        Dialogue dialogue4 = new Dialogue(new List<string>
        {
            "Comme ces champignons sont beaux. Tu te souviens quand on allait en cueillir ensemble ?",
            "Je suis devenue trop vieille pour te suivre, hélas.",
            "On aura besoin de davantage de bois pour le feu, peut-être 6 morceaux.",
            "Je vais te tricoter une écharpe en t'attendant, tu as l'air frigorifiée."
        }, 0, 6, 0);

        dialogues = new List<Dialogue> { dialogue1, dialogue2, dialogue3, dialogue4 };
    }


    public Dialogue dialogueEnCours()
    {
        return dialogues[dialogue_courant];
    }

    public void lancerDialogue() {
        if (etat==Etat.ouvert) {
            //boite déjà ouverte
            //on passe au prochain texte
            timer_ecriture = 0;
            etat = Etat.enEcriture;

            if(dialogueEnCours().prochainTexte()){
                //on est arrivé au delà de la fin, il faut fermer
                fermeture();
                
            }
        } else if(etat==Etat.fermé){
            //boite fermée
            //l'ouvrir
            ouverture();

            //si condition remplie, on ouvre nouveau dialogue
            if (dialogueEnCours().conditionsRemplies(champignons, brindilles, baies)) {
                dialogue_courant++;
                if (dialogue_courant >= dialogues.Count) {
                    //on a terminé la dernière quête... que faire ?
                    dialogue_courant--;
                }
            }
            dialogueEnCours().commencer();
            
        }
    }

    void fermeture() {
        etat = Etat.enFermeture;
        timer_boite = duree_boite;
    
    }

    void ouverture() {
        etat = Etat.enOuverture;
        boiteDialogue.gameObject.SetActive(true);
        texteBoiteDialogue.text = "";
        timer_boite = 0;
        
    }

    void gestionAnimations() {
        if (etat == Etat.enOuverture) {
            timer_boite += Time.deltaTime;

            if (timer_boite >= duree_boite) {
                etat = Etat.enEcriture;


            }
        } else if (etat == Etat.enFermeture) {
            timer_boite -= Time.deltaTime;

            if (timer_boite <= 0) {
                etat = Etat.fermé;
                boiteDialogue.gameObject.SetActive(false);
            }
        } else if (etat == Etat.enEcriture) {
            timer_ecriture += Time.deltaTime;
            //afficher partiellement le texte
            float pourcent = timer_ecriture / duree_ecriture;
            string dialog = dialogueEnCours().texteEnCours();

            string txt = "";
            for (int i = 0; i < Mathf.Min(1,pourcent) * dialog.Length; ++i) {
                txt += dialog[i];
            }

            Debug.Log("Pourcent " + pourcent);
            texteBoiteDialogue.text = txt;

            if (timer_ecriture >= duree_ecriture)
                etat = Etat.ouvert;
        }

        boiteDialogue.transform.localScale = tailleInitialeBoite* (timer_boite/duree_boite);
    }
}
