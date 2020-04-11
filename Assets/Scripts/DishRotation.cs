﻿using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;


public class DishRotation : MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject petridish;
    private Quaternion targetAngle;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Change cursor texture to hand or something
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Change cursor texture to default
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Change cursor texture to hold or something
        targetAngle = petridish.transform.rotation;
    }

    public void OnDrag(PointerEventData eventData)
    {
        petridish.transform.Rotate(new Vector2(-eventData.delta.y, -eventData.delta.x) * Time.deltaTime * 5f, Space.World);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Change cursor texture to back to hand or something
        StartCoroutine(Release());
    }

    IEnumerator Release()
    {
        while (petridish.transform.rotation != targetAngle)
        {
            petridish.transform.rotation = Quaternion.Slerp(petridish.transform.rotation, targetAngle, 0.25f);
            yield return null;
        }
        yield return null;
    }
}
