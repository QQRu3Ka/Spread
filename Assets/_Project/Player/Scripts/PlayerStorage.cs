using R3;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using Zenject;

public class PlayerStorage
{
    [Inject] private ColorConfig _colorConfig;

    private List<Player> _playerList = new();
    private ReplaySubject<Player> _onNewPlayerCreated = new();

    public ReadOnlyCollection<Player> PlayerList => _playerList.AsReadOnly();
    public Observable<Player> OnNewPlayerCreated => _onNewPlayerCreated;

    public void CreateNewPlayer()
    {
        var takenColors = _playerList.Select(player => player.Color).ToList();
        var freeColor = _colorConfig.ColorDictionary.Keys
            .First(color => color != GameColor.NONE && !takenColors.Contains(color));

        var player = new Player($"Player {_playerList.Count + 1}", freeColor);

        AddPlayer(player);

        _onNewPlayerCreated.OnNext(player);
    }

    public void AddPlayer(Player player)
    {
        _playerList.Add(player);
    }
}
