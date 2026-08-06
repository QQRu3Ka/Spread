using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ZLinq;

public class MapsPanel : MonoBehaviour
{
    [Inject] private MapsConfig _mapsConfig;
    [Inject] private MapHolder _mapHolder;

    [SerializeField] private Transform _mapButtonContainer;
    [SerializeField] private GameObject _mapButtonPrefab;
    [SerializeField] private GameObject _selectedMapContainer;

    private List<Button> _buttons = new();

    private void Start()
    {
        foreach(var map in _mapsConfig.Catalog)
        {
            var obj = Instantiate(_mapButtonPrefab, _mapButtonContainer);
            var button = obj.GetComponent<Button>();
            obj.Children().OfComponent<TextMeshProUGUI>().First().text = map.Value.Name;

            _buttons.Add(button);

            button.onClick.AddListener(() => SelectMap(map.Key, map.Value.Map));
        }
    }

    private void SelectMap(Map map, GameObject mapPrefab)
    {
        _mapHolder.Map = map;
        //Destroy(_selectedMapContainer.Children().FirstOrDefault());
        //Instantiate(mapPrefab, _selectedMapContainer.transform);
    }

    private void OnDestroy()
    {
        foreach(var button in _buttons)
        {
            button.onClick.RemoveAllListeners();
        }
    }
}
