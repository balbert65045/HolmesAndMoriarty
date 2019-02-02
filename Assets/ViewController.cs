using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ViewController : MonoBehaviour {

    public Transform ZoomedCardTransform;
    public LayerMask cardlayer;
    public ScrollRect scrollRect;

    private Vector3 selectedCardOldPosition;
    private Vector3 selectedCardOldScale;
    private Transform selectedCardOldParent;
    private ViewCard selectedCard;

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    // Use this for initialization
    void Start () {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
        {
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.GetComponent<ViewCard>() != null)
                {
                    scrollRect.enabled = false;

                    selectedCard = result.gameObject.GetComponent<ViewCard>();
                    selectedCardOldPosition = selectedCard.transform.position;
                    selectedCardOldScale = selectedCard.transform.localScale;
                    selectedCardOldParent = selectedCard.transform.parent;

                    selectedCard.transform.position = ZoomedCardTransform.position;
                    selectedCard.transform.localScale = ZoomedCardTransform.localScale;
                    selectedCard.transform.SetParent(ZoomedCardTransform);
                }
            }
        }
  
        else if (Input.GetMouseButtonUp(0))
        {
            if (selectedCard != null)
            {
                scrollRect.enabled = true;

                selectedCard.transform.SetParent(selectedCardOldParent);
                selectedCard.transform.position = selectedCardOldPosition;
                selectedCard.transform.localScale = selectedCardOldScale;
                selectedCard = null;
            }
        }
	}
}
