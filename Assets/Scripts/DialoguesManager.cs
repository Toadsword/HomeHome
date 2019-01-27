using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialoguesManager : MonoBehaviour {

    static DialoguesManager _instance;
    public static DialoguesManager Instance
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

    [SerializeField] Image boiteDialogue;
    [SerializeField] Text texteBoiteDialogue;

    [SerializeField] int champignons;

    [SerializeField] int brindilles;

    [SerializeField] int baies;

    int dialogue_courant;//indice du tableau des dialogues
    List<Dialogue> dialogues;
    [SerializeField] Loup loup;
    [SerializeField] private LanterneChan lanternChan;

    //gestion animations
    public enum Etat { ouvert, closed, enOuverture, enFermeture, enEcriture};
    public Etat etat;

    Vector2 tailleInitialeBoite;
    float timer_boite;
    float duree_boite = 0.15f;

    float timer_ecriture;
    float duree_ecriture = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        dialogue_courant = 0;
        etat = Etat.closed;
        boiteDialogue.gameObject.SetActive(false);

        creerDialogues();

        timer_boite = 0;
        tailleInitialeBoite = boiteDialogue.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameInput.GetInputUp(GameInput.InputType.ACTION)) {
            /*lancerDialogue();*/
        }

        gestionAnimations();
    }

    void creerDialogues() {
        Dialogue dialogue1 = new Dialogue(new List<string>
        {
            "I'm so happy you come to visit your grandma, Sweet.", // "Comme je suis ravie que tu rendes visite à ta grand-mère, Rouge.",
            "I'm planning on making you a hot soup to warm you up from this cold !", //"Je comptais te préparer une bonne soupe chaude pour te revigorer du froid hivernale.",
            "It will be good for you. However, I'm out of mushrooms...", // "Cela te ferait du bien, tu es toute maigrelette, mais je n'ai plus de champignons...",
            "Could you go out and find me 4 muschrooms while I'm making the soup please?", //"Peux-tu aller en chercher 3 pendant que je prépare le potage ?",
            "Don't forget to bring the lantern, it is dark outside.", //"N'oublie pas de prendre la lanterne, il fait noir dehors."
        }, 4, 0, 0);

        Dialogue dialogue2 = new Dialogue(new List<string>
        {
            "Thank you Sweet-heart ! I hope you didn't catch cold !", //"Merci beaucoup mon sucre d'orge ! J'espère que tu n'as pas pris froid !",
            "Does the soup smells good to you? That's the one you prefer the most!", //"L'odeur du potage te plait ? C'est celui que tu préfères.",
            "I would love to make some jam tomorrow..",
            "Could you go out again and bring me some berries from the bushes please? 4 will do!"
        }, 0, 4, 0);

        Dialogue dialogue3 = new Dialogue(new List<string>
        {
            "Thank you Sweet-heart ! Fresh berries are the best for a good jam !", //"Merci beaucoup mon sucre d'orge ! J'espère que tu n'as pas pris froid !",
            "Shoot! The firepit dims !", //"Flûte de zut, le feu s'affaiblit !",
            "Could you get out and bring me 4 brushwoods while the mushrooms nurture please?", //"Va donc chercher 4 morceaux de bois pendant que les champignons mijotent."
        }, 0, 0, 4);

        Dialogue dialogue4 = new Dialogue(new List<string>
        {
            "Here you are again Sweet ! I was worried about you.", //"Te voilà, Rouge ! J'allais m'inquiéter.",
            "Gladly, it's a rather calm wood, and wonderfull on daytime !", // "Heureusement, c'est un bois très tranquille. Et magnifique en pleine journée.",
            "Uh Oh, I guess there wasn't enough mushrooms for the stew...", //"J'ai une idée, et si on faisait plus de soupe pour ta mère, elle sera contente !",
            "Could you bring me 5 more? It would be awesome !", //"Je pense qu'avec 5 champignons supplémentaires, il y aura assez."
        }, 5, 0, 0);


        Dialogue dialogue5 = new Dialogue(new List<string>
        {
            "Look how beautiful they are ! Do you remember when we used to gather them together in the old times?", //Comme ces champignons sont beaux. Tu te souviens quand on allait en cueillir ensemble ?",
            "Sadly, I became too old to follow you how...", //Je suis devenue trop vieille pour te suivre, hélas.",
            "We will need more wood for the firepit, 6 pieces will be enough.", //On aura besoin de davantage de bois pour le feu, peut-être 6 morceaux.",
            "I will knitter a scarf in the meanwhile, you seem refregirated !", //Je vais te tricoter une écharpe en t'attendant, tu as l'air frigorifiée."
        }, 0, 0, 6);

        Dialogue dialogue6 = new Dialogue(new List<string>
        {
            "Look how beautiful they are ! Do you remember when we used to gather them together in the old times?", //Comme ces champignons sont beaux. Tu te souviens quand on allait en cueillir ensemble ?",
            "Sadly, I became too old to follow you how...", //Je suis devenue trop vieille pour te suivre, hélas.",
            "We will need more berries for the jam tomorrow.. 5 of them should do the job.", //On aura besoin de davantage de bois pour le feu, peut-être 6 morceaux.",
            "I hope it doesn't bother you much!", //Je vais te tricoter une écharpe en t'attendant, tu as l'air frigorifiée."
        }, 0, 5, 0);

        Dialogue dialogue7 = new Dialogue(new List<string>
        {
            "Thank you so much for helping me out in the house !",
            "Oh, do you still want to help me? You're so Sweet !", 
            "Could you bring me 9 more mushrooms for another stew tomorrow?", 
            "Don't forget the lantern !",
        }, 9, 0, 0);

        Dialogue dialogue8 = new Dialogue(new List<string>
        {
            "Welcome back Sweet-y~ You didn't meet any danger outside? !",
            "Your scarf is going well ! I might finish it really soon for you !",
            "I'm sure you are eager to see it !",
            "... Do you still want you to help me? Don't push yourself too hard !",
            "I would need more berries for the jam... 7 will do I think."
        }, 0, 7, 0);

        Dialogue dialogue9 = new Dialogue(new List<string>
        {
            "You sure like going out at night... You're bringing me so much stuff that's unbelievable !", //Comme ces champignons sont beaux. Tu te souviens quand on allait en cueillir ensemble ?",
            "Wood is a rare ressources around... Could you bring me some?", //Je suis devenue trop vieille pour te suivre, hélas.",
            "I think 8 pieces will be enough for a while.", //On aura besoin de davantage de bois pour le feu, peut-être 6 morceaux.",
        }, 0, 8, 0);

        dialogues = new List<Dialogue> { dialogue1, dialogue2, dialogue3};

        if (Random.Range(0, 1) < 0.5f)
        {
            dialogues.Add(dialogue4);
            dialogues.Add(dialogue5);
        }
        else
        {
            dialogues.Add(dialogue5);
            dialogues.Add(dialogue4);
        }
        dialogues.Add(dialogue6);

        List<Dialogue> remainingOnes = new List<Dialogue>{ dialogue7 , dialogue8, dialogue9 };

        for (int i = 0; i <= remainingOnes.Count; i++)
        {
            Dialogue selectedOne = remainingOnes[Mathf.CeilToInt(Random.Range(0, remainingOnes.Count))];
            dialogues.Add(selectedOne);
            remainingOnes.Remove(selectedOne);
        }
    }

    public Dialogue dialogueEnCours()
    {
        return dialogues[dialogue_courant];
    }

    public Pickable.PickableType CurrentCondition()
    {
        return dialogues[dialogue_courant].GetMissionType();
    }

    public bool lancerDialogue() {
        if (etat==Etat.ouvert) {
            //boite déjà ouverte
            //on passe au prochain texte
            timer_ecriture = 0;
            etat = Etat.enEcriture;

            if(dialogueEnCours().prochainTexte()){
                //on est arrivé au delà de la fin, il faut fermer
                fermeture();
                return true;
            }

        } else if(etat==Etat.closed)
        {
            //boite fermée
            //l'ouvrir
            ouverture();

            //si condition remplie, on ouvre nouveau dialogue
            if (dialogueEnCours().conditionsRemplies(champignons, brindilles, baies)) {
                dialogue_courant++;

                loup.LeveluUp((int)(dialogue_courant / 3) + 1);
                lanternChan.LeveluUp((int)(dialogue_courant / 3) + 1);

                if (dialogue_courant >= dialogues.Count) {
                    //on a terminé la dernière quête... que faire ?
                    dialogue_courant--;
                }
            }
            dialogueEnCours().commencer();
        }

        return false;
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
                etat = Etat.closed;
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

            texteBoiteDialogue.text = txt;

            if (timer_ecriture >= duree_ecriture)
                etat = Etat.ouvert;
        }

        boiteDialogue.transform.localScale = tailleInitialeBoite* (timer_boite/duree_boite);
    }
}
