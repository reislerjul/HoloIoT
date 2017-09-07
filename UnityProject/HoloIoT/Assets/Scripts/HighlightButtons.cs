using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighlightButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Material startcolor;
    public Material mat;

    void Start()
    {
        startcolor = GetComponent<Image>().material;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().material = mat;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().material = startcolor;
    }
}