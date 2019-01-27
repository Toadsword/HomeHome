using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{
    private bool unSurDeux = false;

    int indice_courant;
    public List<string> textes_fr { get; private set; }
    //conditions de réussite
    public int nb_champignons { get; private set; }
    public int nb_brindilles { get; private set; }
    public int nb_baies { get; private set; }

    public Dialogue(List<string> txt_fr, int _nb_champignons, int _nb_baies, int _nb_brindilles) {
        indice_courant = 0;
        textes_fr = txt_fr;

        nb_champignons = _nb_champignons;
        nb_brindilles = _nb_brindilles;
        nb_baies = _nb_baies;
    }

    public bool conditionsRemplies(int _nb_champignons, int _nb_brindilles, int _nb_baies) {
        //prend en argument les items possedés par le joueur et retourne si ce dialogue peut se lancer
        return (nb_champignons <= _nb_champignons && nb_brindilles <= _nb_brindilles && nb_baies <= _nb_baies);
    }

    public string texteEnCours()
    {
        if(unSurDeux)
            SoundManager.Instance.PlaySound(SoundManager.SoundList.DIALOGUE);
        unSurDeux = !unSurDeux;
        return textes_fr[indice_courant];
    }

    public void commencer() {
        indice_courant = 0;
    }
    public bool prochainTexte() {
        //passe au prochain texte,
        //si c'était le dernier texte, renvoie true (signal pour fermer le dialogue)

        indice_courant++;
        if (indice_courant >= textes_fr.Count){
            indice_courant--;
            return true;
        }
        return false;
    }

    public bool conditionEstChampignons() {
        return nb_champignons > 0;
    }
    public bool conditionEstBaies() {
        return nb_baies > 0;
    }
    public bool conditionEstBrindilles() {
        return nb_brindilles > 0;
    }

}
