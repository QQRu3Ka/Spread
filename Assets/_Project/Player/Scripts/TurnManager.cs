using Cysharp.Text;
using R3;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class TurnManager : IInitializable
{
    [Inject] private GameState _gameState;
    [Inject] private ColorConfig _colorConfig;
    [Inject] private PlayerStorage _playerStorage;

    private ReplaySubject<string> _onNextPlayerSelected = new();
    private Subject<string> _onPlayersLost = new();
    private List<Player> _lostPlayers = new();

    public List<Player> Players;
    public int CurrentPlayerIndex;

    public Observable<string> OnNextPlayerSelected => _onNextPlayerSelected;
    public Observable<string> OnPlayersLost => _onPlayersLost;

    public void Initialize()
    {
        Players = new(_playerStorage.PlayerList);
        Players.Shuffle();
        CurrentPlayerIndex = 0;
        AnnounceNextPlayer();
    }

    public void EndTurn()
    {
        if (_gameState.IsGameFinished.Value)
        {
            Debug.Log("Остался один игрок");
            return;
        }

        SelectNextPlayer();
    }

    private void SelectNextPlayer()
    {
        if (!Players[CurrentPlayerIndex].IsMadeFirstTurn)
        {
            Players[CurrentPlayerIndex].IsMadeFirstTurn = true;
            CurrentPlayerIndex = CurrentPlayerIndex == Players.Count - 1 ? 0 : CurrentPlayerIndex + 1;
            AnnounceNextPlayer();
            return;
        }

        CurrentPlayerIndex = CurrentPlayerIndex == Players.Count - 1 ? 0 : CurrentPlayerIndex + 1;

        if(_gameState.Cells.Values.Count(cell => cell.Color.Value == Players[CurrentPlayerIndex].Color) == 0)
        {
            if (!Players[CurrentPlayerIndex].IsLost)
            {
                Players[CurrentPlayerIndex].IsLost = true;
                _lostPlayers.Add(Players[CurrentPlayerIndex]);
            }

            SelectNextPlayer();
            return;
        }

        AnnounceLostPlayers(_lostPlayers);
        AnnounceNextPlayer();
    }

    private void AnnounceLostPlayers(List<Player> players)
    {
        if (players.Count == 0) return;

        if (players.Count == 1)
        {
            _onPlayersLost.OnNext(ZString.Format("У {0} <color=#30D5C8><b>не осталось клеток</b></color>!", players[0].Name.ColorWith(_colorConfig.ColorDictionary[players[0].Color].Color)));
        }

        if (players.Count > 1)
        {
            using var sb = ZString.CreateStringBuilder();
            for (var i = 0; i < players.Count; i++)
            {
                if (i == 0) sb.AppendFormat("У {0}", players[i].Name.ColorWith(_colorConfig.ColorDictionary[players[i].Color].Color));
                if (i != players.Count - 1)
                {
                    sb.AppendFormat(", {0}", players[i].Name);
                }
                else
                {
                    sb.AppendFormat(" и {0} <color=#30D5C8><b>не осталось клеток</b></color>!", players[i].Name.ColorWith(_colorConfig.ColorDictionary[players[i].Color].Color));
                }
            }
            _onPlayersLost.OnNext(sb.ToString());
        }

        _lostPlayers.Clear();
    }

    private void AnnounceNextPlayer()
    {
        _onNextPlayerSelected.OnNext(ZString.Format("Ход {0}", Players[CurrentPlayerIndex].Name.ColorWith(_colorConfig.ColorDictionary[Players[CurrentPlayerIndex].Color].Color)));
    }
}
