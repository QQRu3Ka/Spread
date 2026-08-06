using R3;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;
using ZLinq;

public class WinPanel : MonoBehaviour
{
    [Inject] private ColorConfig _colorConfig;
    [Inject] private GameState _gameState;

    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private GameObject _hidableObject;

    private void Start()
    {
        _hidableObject.SetActive(false);

        _gameState.OnPlayerWin
            .Subscribe(player =>
            {
                _hidableObject.SetActive(true);
                SetName(player);
            })
            .AddTo(this);
    }

    private void SetName(Player player)
    {
        _playerName.text = player.Name.ColorWith(_colorConfig.ColorDictionary[player.Color].Color);
    }
}
