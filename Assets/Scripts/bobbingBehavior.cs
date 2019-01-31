using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bobbingBehavior : MonoBehaviour
{
    [SerializeField] private float timetoStopBobbling = 3f;
    private float timeSinceHorizontal = 0;

    private float bobbingScale;
    // Update is called once per frame
    void Update()
    {
        timeSinceHorizontal += Time.deltaTime;
        // SI je bouge
        if (!Mathf.Approximately(GameInput.GetAxisRaw(GameInput.AxisType.VERTICAL), 0) ||
            !Mathf.Approximately(GameInput.GetAxisRaw(GameInput.AxisType.HORIZONTAL), 0))
        {
            timeSinceHorizontal -= Time.deltaTime * 4;
        }

        if (timeSinceHorizontal < 0f)
            timeSinceHorizontal = 0f;
        if (timeSinceHorizontal > timetoStopBobbling)
            timeSinceHorizontal = timetoStopBobbling;

        bobbingScale = 1.2f - (timeSinceHorizontal / timetoStopBobbling);



        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Sin(Time.time * 5f) * 30 * bobbingScale));
    }
}
