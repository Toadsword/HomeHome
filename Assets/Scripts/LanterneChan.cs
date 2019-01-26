using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanterneChan : MonoBehaviour
{
    [SerializeField] float maxScale = 2.5f;
    [SerializeField] float minScale = 0.0f;
    private float currentScale;
    
    [SerializeField] private float timeForOneScale = 120;

    [SerializeField] private float timeRemaining;

    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = timeForOneScale * maxScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining <= 0.0f)
        {
            transform.localScale = new Vector3(0,0,0);
        }
        else
        {
            timeRemaining -= Time.deltaTime;
            currentScale = timeRemaining / timeForOneScale;
            if (currentScale > maxScale)
                currentScale = maxScale;
            if (currentScale < minScale)
                currentScale = minScale;

            currentScale += Mathf.Sin(Time.time * 1.5f) * 0.2f;
            transform.localScale = new Vector3(currentScale, currentScale);
        }
    }

    public void ResetLanterneTimer()
    {
        timeRemaining = timeForOneScale * maxScale;
    }
}
