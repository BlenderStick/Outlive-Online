using System.Collections;
using System.Collections.Generic;
using Outlive.Map;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapLoadBehaviour : UIBehaviour
{
    [SerializeField] private RectTransform _content;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private MapLoader _mapLoader;
    [SerializeField] private UnityEvent<MapItemUI> _onItemChanged;

    protected int _currentSelect = -1;

    private IList<GameObject> _itemObjects = new List<GameObject>();


    protected override void Start() 
    {
        base.Start();
        if (_mapLoader != null && _content != null && _itemPrefab != null)
            LoadMapSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadMapSettings()
    {
        _itemObjects.Clear();
        float lastY = 0;

        foreach (var item in _mapLoader.maps)
        {
            GameObject currentItem = Instantiate(_itemPrefab, _content.transform);
            _itemObjects.Add(currentItem);
            RectTransform rT = currentItem.GetComponent<RectTransform>();
            Vector3 v = rT.localPosition;
            v.y = lastY;
            rT.localPosition = v;
            lastY -= rT.rect.height;
            MapItemUI mapItemUI;
            if (currentItem.TryGetComponent<MapItemUI>(out mapItemUI))
            {
                mapItemUI.onClick.AddListener(ItemChangedListener);
                mapItemUI.sprite = item.sprite;
                mapItemUI.text = item.mapName;
            }
        }

    }

    private void ItemChangedListener(MapItemUI map)
    {
        _currentSelect = _itemObjects.IndexOf(map.gameObject);
        _onItemChanged.Invoke(map);
        Debug.Log(currentSelect);
    }

    public int currentSelect
    {
        get => _currentSelect;
        set
        {
            _currentSelect = value;
            if (value > -1)
            {
                GameObject select = _itemObjects[_currentSelect];
                Selectable selectable;
                if (select.TryGetComponent<Selectable>(out selectable))
                {
                    selectable.Select();
                }
                MapItemUI mapItemUI;
                if (select.TryGetComponent<MapItemUI>(out mapItemUI))
                {
                    ItemChangedListener(mapItemUI);
                }
            }
        }
    }

    public UnityEvent<MapItemUI> onItemChanged => _onItemChanged;

    public MapLoader mapLoader
    {
        get => _mapLoader;
        set
        {
            _mapLoader = value;
            if (_mapLoader != null && _content != null && _itemPrefab != null)
                LoadMapSettings();
        }
    }
}
