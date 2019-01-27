using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanterneChan : MonoBehaviour
{
    [SerializeField] float maxScale = 2.5f;
    [SerializeField] float minScale = 0.0f;
    private float currentScale;

    [SerializeField] private float[] timeForOneScale = {40, 50, 60};
    private float currentTimeForOneScale;


    [SerializeField] private float timeRemaining;

    // Start is called before the first frame update
    void Start()
    {
        currentTimeForOneScale = timeForOneScale[1];
        timeRemaining = currentTimeForOneScale * maxScale;
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
            currentScale = timeRemaining / currentTimeForOneScale;
            if (currentScale > maxScale)
                currentScale = maxScale;
            if (currentScale < minScale)
                currentScale = minScale;

            currentScale += Mathf.Sin(Time.time * 4.2f) * 0.2f;
            transform.localScale = new Vector3(currentScale, currentScale);
        }
    }

    public void LeveluUp(int newLevel)
    {
        currentTimeForOneScale = timeForOneScale[newLevel];
    }

    public void ResetLanterneTimer()
    {
        timeRemaining = currentTimeForOneScale * maxScale;
    }
}
