using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int numBaie = 0;
    [SerializeField] private int numBrindille = 0;
    [SerializeField] private int numChampignon = 0;

    [SerializeField] private int maxInventory = 20;

    [SerializeField] private List<Transform> closestPickable;

    // Start is called before the first frame update
    void Start()
    {
        numBaie = 0;
        numBrindille = 0;
        numChampignon = 0;
    }

    void Update()
    {
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

            if (closestOne != null)
            {
                switch (closestOne.gameObject.GetComponent<Pickable>().typePickable)
                {
                    case Pickable.PickableType.BAIE:
                        numBaie++;
                        break;
                    case Pickable.PickableType.BRINDILLE:
                        numBrindille++;
                        break;
                    case Pickable.PickableType.CHAMPIGNON:
                        numChampignon++;
                        break;
                }
                closestOne.gameObject.SetActive(false);
                closestPickable.Remove(closestOne);
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
