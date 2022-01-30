using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MapItemUI : UIBehaviour, IPointerClickHandler
{

    [Serializable]
    public class MapItemEvent : UnityEvent {}

    [FormerlySerializedAs("TextLabel")]
    [SerializeField] private Text _text;
    [SerializeField] private Image _image;
    [SerializeField] private UnityEvent<MapItemUI> _onClick = new UnityEvent<MapItemUI>();

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsActive())
            return;

        onClick.Invoke(this);
    }
    
    public UnityEvent<MapItemUI> onClick
    {
        get => _onClick;
        set => _onClick = value;
    }

    public Text textLabel => _text;
    public Image image => _image;

    public string text 
    {
        get => _text.text;
        set => _text.text = value;
    }

    public Sprite sprite
    {
        get => _image.sprite;
        set => _image.sprite = value;
    }


}
