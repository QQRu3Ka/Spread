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
        var id = 0;
        var takenColors = _playerList.Select(player => player.Color).ToList();
        var freeColor = _colorConfig.ColorDictionary.Keys
            .First(color => color != GameColor.NONE && !takenColors.Contains(color));

        var lastPlayer = _playerList.LastOrDefault();
        if (lastPlayer != null)
        {
            id = lastPlayer.Id + 1;
        }

        var player = new Player { Id = id, Color = freeColor };

        AddPlayer(player);

        _onNewPlayerCreated.OnNext(player);
    }

    public void AddPlayer(Player player)
    {
        _playerList.Add(player);
    }
}
