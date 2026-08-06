using Cysharp.Text;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using ZLinq;

public class GameState
{
    [Inject] private MapsConfig _mapsConfig;
    [Inject] private MapHolder _mapHolder;
    [Inject] private DiContainer _container;
    [Inject] private PlayerStorage _playerStorage;
    [Inject] private ColorConfig _colorConfig;

    private CompositeDisposable _disposables = new();
    private Subject<string> _onGameFinished = new();
    private Subject<Player> _onPlayerWin = new();

    public Dictionary<Vector2Int, Cell> Cells;
    public ReactiveProperty<bool> IsGameFinished = new();

    public Observable<string> OnGameFinished => _onGameFinished;
    public Observable<Player> OnPlayerWin => _onPlayerWin;

    [Inject]
    private void Construct()
    {
        var map = _container.InstantiatePrefab(_mapsConfig.Catalog[_mapHolder.Map].Map);
        map.AddComponent<AppearAnimation>();
        Cells = map.Descendants().OfComponent<Cell>().ToDictionary(cell => cell.GridPosition);

        IsGameFinished
            .Where(isFinished => isFinished == true)
            .Subscribe(_ => AnnounceWinner())
            .AddTo(_disposables);
    }

    private void AnnounceWinner()
    {
        var winnerColor = Cells.Values.First(cell => cell.Color.Value != GameColor.NONE).Color.Value;
        var winnerCellsCount = Cells.Values.Count(cell => cell.Color.Value == winnerColor);
        var winnerPlayer = _playerStorage.PlayerList.First(player => player.Color == winnerColor);

        _onPlayerWin.OnNext(winnerPlayer);

        if(winnerCellsCount >= 10 && winnerCellsCount <= 20)
        {
            _onGameFinished.OnNext(ZString.Format("{0} <color=#30D5C8><b>выиграл</b></color> и захватил <color=#30D5C8><b>{1}</b></color> клеток!", winnerPlayer.Name.ColorWith(_colorConfig.ColorDictionary[winnerPlayer.Color].Color), winnerCellsCount));
            return;
        }
        if (winnerCellsCount % 10 == 1)
        {
            _onGameFinished.OnNext(ZString.Format("{0} <color=#30D5C8><b>выиграл</b></color> и захватил <color=#30D5C8><b>{1}</b></color> клетку!", winnerPlayer.Name.ColorWith(_colorConfig.ColorDictionary[winnerPlayer.Color].Color), winnerCellsCount));
            return;
        }
        if (winnerCellsCount % 10 >= 2 && winnerCellsCount % 10 <= 4)
        {
            _onGameFinished.OnNext(ZString.Format("{0} <color=#30D5C8><b>выиграл</b></color> и захватил <color=#30D5C8><b>{1}</b></color> клетки!", winnerPlayer.Name.ColorWith(_colorConfig.ColorDictionary[winnerPlayer.Color].Color), winnerCellsCount));
            return;
        }
        if (winnerCellsCount % 10 >= 5 && winnerCellsCount % 10 <= 9 || winnerCellsCount % 10 == 0)
        {
            _onGameFinished.OnNext(ZString.Format("{0} <color=#30D5C8><b>выиграл</b></color> и захватил <color=#30D5C8><b>{1}</b></color> клеток!", winnerPlayer.Name.ColorWith(_colorConfig.ColorDictionary[winnerPlayer.Color].Color), winnerCellsCount));
            return;
        }
    }
}
