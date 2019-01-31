using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] public int numBaie = 0;
    [SerializeField] public int numBrindille = 0;
    [SerializeField] public int numChampignon = 0;
    [SerializeField] public int numCailloux = 0;
    
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private List<Transform> closestPickable;

    [SerializeField] Pickable caillouPrefab1;
    [SerializeField] Pickable caillouPrefab2;
    [SerializeField] Pickable caillouPrefab3;
    [SerializeField] Pickable caillouPrefab4;
    [SerializeField] Pickable caillouPrefab5;
    [SerializeField] Pickable caillouPrefab6;

    [SerializeField] GameObject surbrillance;

    float duree_pick = 0.5f;//sec
    float timer_pick;
    Transform pickableEnCours = null;
    public bool isPicking { get; private set; }


    float direction_lancer = 1;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Start playerInventory");

        numChampignon = DialoguesManager.Instance.getChampignons();
        numBaie = DialoguesManager.Instance.getBaies();
        numBrindille = DialoguesManager.Instance.getBrindilles();
        numCailloux = DialoguesManager.Instance.getCailloux();

        isPicking = false;
        timer_pick = duree_pick;
    }

    void Update()
    {
        //récupération pickable plus proche
        Transform closestOne = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        Pickable.PickableType currentMission = DialoguesManager.Instance.CurrentCondition();
        foreach (Transform t in closestPickable)
        {
            if (t.GetComponent<Pickable>().typePickable != currentMission &&
                t.GetComponent<Pickable>().typePickable != Pickable.PickableType.CAILLOU &&
                t.GetComponent<Pickable>().typePickable != Pickable.PickableType.DOOR &&
                t.GetComponent<Pickable>().typePickable != Pickable.PickableType.GRANNY)
                continue;

            float dist = Vector3.Distance(t.position, currentPos);
            if (dist < minDist)
            {

                closestOne = t;
                minDist = dist;
            }
        }

        //afficher surbrillance closestOne
        if (closestOne != null && ( closestOne.GetComponent<Pickable>().typePickable==Pickable.PickableType.BAIE ||
           closestOne.GetComponent<Pickable>().typePickable == Pickable.PickableType.BRINDILLE ||
           closestOne.GetComponent<Pickable>().typePickable == Pickable.PickableType.CHAMPIGNON ||
           closestOne.GetComponent<Pickable>().typePickable == Pickable.PickableType.GRANNY ||
           closestOne.GetComponent<Pickable>().typePickable == Pickable.PickableType.CAILLOU) ) {
            surbrillance.SetActive(true);

            surbrillance.transform.position = closestOne.transform.position;
            surbrillance.GetComponent<SpriteRenderer>().sortingOrder = closestOne.GetComponent<SpriteRenderer>().sortingOrder - 1;
        } else {
            surbrillance.SetActive(false);
        }

         if (closestOne !=null && closestOne.gameObject.GetComponent<Pickable>().typePickable == Pickable.PickableType.DOOR) {
            // Changement de scene vers la maison ou vers dehors, sans action
            timer_pick = duree_pick;
            isPicking = false;
            if (SceneManagement.Instance.currentScene == SceneManagement.Scenes.HOUSE)
                SceneManagement.Instance.ChangeScene(SceneManagement.Scenes.OVERWORLD);
            else
                SceneManagement.Instance.ChangeScene(SceneManagement.Scenes.HOUSE);
        }


        if (GameInput.GetInputDown(GameInput.InputType.ACTION)) {
            if (closestOne != null)
            {
                pickableEnCours = closestOne;
                if (pickableEnCours.gameObject.GetComponent<Pickable>().typePickable == Pickable.PickableType.GRANNY)
                {
                    isPicking = true;
                    if(!DialoguesManager.Instance.lancerDialogue())
                        return;
                    isPicking = false;
                }

                timer_pick = 0;

                if (pickableEnCours.gameObject.GetComponent<Pickable>().typePickable != Pickable.PickableType.DOOR &&
                    pickableEnCours.gameObject.GetComponent<Pickable>().typePickable != Pickable.PickableType.GRANNY &&
                    !isPicking)
                {
                    isPicking = true;
                    playerAnimation.animator.SetBool("Pickup", true);
                    playerAnimation.animator.speed = 1;
                }
                else if (pickableEnCours.gameObject.GetComponent<Pickable>().typePickable == Pickable.PickableType.GRANNY)
                {
                    timer_pick = duree_pick;
                }

            } else if(numCailloux>0){
                numCailloux--;
                Pickable caillou = null;
                int hasard = (Random.Range(0,6));
                if (hasard==0) caillou= GameObject.Instantiate(caillouPrefab1);
                if (hasard == 1) caillou = GameObject.Instantiate(caillouPrefab2);
                if (hasard == 2) caillou = GameObject.Instantiate(caillouPrefab3);
                if (hasard == 3) caillou = GameObject.Instantiate(caillouPrefab4);
                if (hasard == 4) caillou = GameObject.Instantiate(caillouPrefab5);
                if (hasard == 5) caillou = GameObject.Instantiate(caillouPrefab6);

                float rayon = 3;

                direction_lancer = -direction_lancer;

                Vector3 deplacement = new Vector3(direction_lancer, 0, 0) * rayon;
                caillou.transform.position = transform.position;
                caillou.GetComponent<Pickable>().lancerAnimation(deplacement);
            }
        }

        //gestion temps d'attente pour pick
        if (timer_pick < duree_pick) {
            timer_pick += Time.deltaTime;
            if (timer_pick > duree_pick) {
                SoundManager.Instance.PlaySound(SoundManager.SoundList.GRAB);
                switch (pickableEnCours.gameObject.GetComponent<Pickable>().typePickable) {
                    case Pickable.PickableType.BAIE:
                        numBaie++;
                        break;
                    case Pickable.PickableType.BRINDILLE:
                        numBrindille++;
                        break;
                    case Pickable.PickableType.CHAMPIGNON:
                        numChampignon++;
                        break;
                    case Pickable.PickableType.CAILLOU:
                        numCailloux++;
                        break;
                }
                DialoguesManager.Instance.setChampignons(numChampignon);
                DialoguesManager.Instance.setBaies(numBaie);
                DialoguesManager.Instance.setBrindilles(numBrindille);
                DialoguesManager.Instance.setCailloux(numCailloux);

                if (pickableEnCours.gameObject.GetComponent<Pickable>().typePickable != Pickable.PickableType.DOOR)
                {
                    playerAnimation.animator.SetBool("Pickup", false);

                    isPicking = false;
                    if (pickableEnCours.gameObject.GetComponent<Pickable>().typePickable !=
                        Pickable.PickableType.GRANNY)
                    {
                        Destroy(pickableEnCours.gameObject);
                        closestPickable.Remove(pickableEnCours);
                    }
                    pickableEnCours = null;
                }
                
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Pickable" && !closestPickable.Contains(collider.transform))
        {
            closestPickable.Add(collider.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Pickable" && closestPickable.Contains(collider.transform))
        {
            closestPickable.Remove(collider.transform);
        }
    }
}
