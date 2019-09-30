using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LookItem : MonoBehaviour
                        , IPointerEnterHandler
                        , IPointerExitHandler
                        , IGvrPointerHoverHandler
{
    
    private void Start() {
        
    }

    public void OnLookItemBox(bool isLookAt)
    {
        // Debug.Log(isLookAt);
        // MoveCtrl.isStopped = isLookAt;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("point enter");
        MoveCtrl.isStopped = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("pointer exit");
        MoveCtrl.isStopped = false;
    }

    public void OnGvrPointerHover(PointerEventData eventData)
    {
        Debug.Log("Reticle on");
    }


}
