using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Pickable : MonoBehaviour
{
    public enum PickableType
    {
        BAIE,
        BRINDILLE,
        CHAMPIGNON
    }

    [SerializeField] public PickableType typePickable;
}
