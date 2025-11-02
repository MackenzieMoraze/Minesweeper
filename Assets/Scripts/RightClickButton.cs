using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class RightClickButton : MonoBehaviour, IPointerClickHandler
{


    public Button targetButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(targetButton == null)
        {
            targetButton = GetComponent<Button>();
        }
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Rightclicked?:" + eventData.ToString());
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Rightclicked Button");
            //trigger flag
            GameController.instance.RightClick(targetButton);
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            
        }
    }
}
