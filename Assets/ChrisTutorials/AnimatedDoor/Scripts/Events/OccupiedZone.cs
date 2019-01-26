using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OccupiedZone : MonoBehaviour
{
    private List<Collider2D> occupants;

    public UnityEvent ZoneIsOccupied, ZoneContinuesToBeOccupied, LastOccupantLeft;

    public void Awake()
    {
        occupants = new List<Collider2D>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        occupants.Add(collision);

        // First Occupant Trigger
        if (occupants.Count == 1 && ZoneIsOccupied.GetPersistentEventCount() != 0)
        {
            ZoneIsOccupied.Invoke();
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (ZoneContinuesToBeOccupied.GetPersistentEventCount() != 0)
            ZoneContinuesToBeOccupied.Invoke();
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        occupants.Remove(collision);

        if (occupants.Count == 0 && LastOccupantLeft.GetPersistentEventCount() != 0)
        {
            LastOccupantLeft.Invoke();
        }
    }
}