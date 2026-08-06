using R3;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerAdder : MonoBehaviour
{
    [Inject] private PlayerStorage _playerStorage;
    [Inject] private ColorConfig _colorConfig;

    [SerializeField] private Button _addNewPlayerButton;
    [SerializeField] private GameObject _playerInfoPanelPrefab;
    [SerializeField] private Transform _playersPanel;

    private void Start()
    {
        _addNewPlayerButton.onClick.AddListener(AddPlayer);
        _playerStorage.OnNewPlayerCreated
            .Subscribe(player => OnNewPlayerCreated(player))
            .AddTo(this);
    }

    private void AddPlayer()
    {
        _playerStorage.CreateNewPlayer();
    }

    private void OnNewPlayerCreated(Player player)
    {
        var obj = Instantiate(_playerInfoPanelPrefab, _playersPanel);
        var playerInfo = obj.GetComponent<PlayerInfo>();
        playerInfo.SetPlayer(player);
        playerInfo.SetColor(_colorConfig.ColorDictionary[player.Color].Color);
    }

    private void OnDestroy()
    {
        _addNewPlayerButton.onClick.RemoveAllListeners();
    }
}
