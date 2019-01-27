using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int numBaie = 0;
    [SerializeField] private int numBrindille = 0;
    [SerializeField] private int numChampignon = 0;
    [SerializeField] private int numCailloux = 0;

    [SerializeField] private int maxInventory = 20;
    [SerializeField] private PlayerAnimation playerAnimation;

    [SerializeField] private List<Transform> closestPickable;

    [SerializeField] Pickable caillouPrefab;

    float duree_pick = 1.0f;//sec
    float timer_pick = 1.0f;
    Transform pickableEnCours = null;
    public bool isPicking { get; private set; }
     
    // Start is called before the first frame update
    void Start()
    {
        numBaie = 0;
        numBrindille = 0;
        numChampignon = 0;
        isPicking = false;
    }

    void Update()
    {
        //récupération pickable plus proche
        if (GameInput.GetInputDown(GameInput.InputType.ACTION))
        {
            Transform closestOne = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = transform.position;
            foreach (Transform t in closestPickable)
            {
                float dist = Vector3.Distance(t.position, currentPos);
                if (dist < minDist)
                {
                    closestOne = t;
                    minDist = dist;
                }
            }

            if (closestOne != null) {
                pickableEnCours = closestOne;
                timer_pick = 0;
                isPicking = true;
                if (pickableEnCours.gameObject.GetComponent<Pickable>().typePickable != Pickable.PickableType.DOOR)
                {
                    SoundManager.Instance.PlaySound(SoundManager.SoundList.GRAB);
                    playerAnimation.animator.SetBool("Pickup", true);
                    playerAnimation.animator.speed = 1;
                }
            } else if(numCailloux>0){
                numCailloux--;
                Pickable caillou = GameObject.Instantiate(caillouPrefab);
                float rayon = 3;
                float x = 1;
                if (Random.value > 0.5) x = -1;
                Vector3 deplacement = new Vector3(x,0, 0) * rayon;
                caillou.transform.position = transform.position;
                caillou.GetComponent<Pickable>().lancerAnimation(deplacement);
            }
        }

        //gestion temps d'attente pour pick
        if (timer_pick < duree_pick) {
            timer_pick += Time.deltaTime;
            if (timer_pick > duree_pick) {
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

                if (pickableEnCours.gameObject.GetComponent<Pickable>().typePickable != Pickable.PickableType.DOOR)
                {
                    playerAnimation.animator.SetBool("Pickup", false);

                    isPicking = false;
                    pickableEnCours.gameObject.SetActive(false);
                    pickableEnCours = null;
                    closestPickable.Remove(pickableEnCours);
                }
                else
                {
                    // Changement de scene vers la maison ou vers dehors
                    if(SceneManagement.Instance.currentScene == SceneManagement.Scenes.HOUSE)
                        SceneManagement.Instance.ChangeScene(SceneManagement.Scenes.OVERWORLD);
                    else
                        SceneManagement.Instance.ChangeScene(SceneManagement.Scenes.HOUSE);
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


    public int nombreBaies() {
        return numBaie;
    }
    public int nombreBrindille() {
        return numBrindille;
    }
    public int nombreChampignon() {
        return numChampignon;
    }
    public int nombreCailloux() {
        return numCailloux;
    }
}
