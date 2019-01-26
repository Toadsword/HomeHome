using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonBehavior : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler
{

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        //Output to console the GameObject's name and the following message
        //Debug.Log("Cursor Entering " + name + " GameObject");

        SelectButton(name);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Output to console the GameObject's name and the following message
        //Debug.Log("Cursor Pressed " + name + " GameObject");

        SubmitButtonAction();
    }

    private void SelectButton(string name)
    {
        MenuManager.Instance.SelectButton(name);
    }

    private void SubmitButtonAction()
    {
        MenuManager.Instance.SubmitButtonAction();
    }
}
