using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent((typeof(Button)), (typeof(Image)))]
public class DefaultButton : MonoBehaviour, IPointerEnterHandler
{
    private Image image;

    protected virtual void Start()
    {
        image = GetComponent<Image>();

        image.sprite = UIManager.Instance.DefaultButtonImage;
        image.color = UIManager.Instance.DefaultButtonTint;

    }
    public virtual void OnPointerEnter(PointerEventData eventData) 
    { 
        AudioManager.Instance?.PlayOneShot("ButtonHover"); 
    }
}
