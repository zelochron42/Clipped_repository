using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIDropdownSelection : MonoBehaviour, ISelectHandler
{
    ScrollRect srect;
    EventSystem eSystem;

    float currentScroll = 1;

    // Start is called before the first frame update
    void Start()
    {
        srect = GetComponentInParent<ScrollRect>(false);
        eSystem = FindObjectOfType<EventSystem>();

        int childcount = srect.content.transform.childCount - 1;
        int childindex = transform.GetSiblingIndex();

        childindex = childindex < ((float)childcount / 2f) ? childindex - 1 : childindex;

        currentScroll = 1f - ((float)childindex / childcount);
    }
    public void OnSelect(BaseEventData eventData) {
        if (srect)
            srect.verticalScrollbar.value = currentScroll;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
