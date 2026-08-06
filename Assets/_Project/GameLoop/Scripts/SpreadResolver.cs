using Cysharp.Text;
using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class SpreadResolver : IInitializable, IDisposable
{
    [Inject] private GameState _gameState;
    [Inject] private TurnManager _turnManager;
    [Inject] private ColorConfig _colorConfig;

    private CompositeDisposable _disposables = new();
    private Queue<Cell> _pendingCells = new();
    private Subject<string> _onCoolComboGained = new();

    public ReactiveProperty<int> ComboCounter = new(0);
    public ReactiveProperty<bool> IsSpreading = new(false);

    public Observable<string> OnCoolComboGained => _onCoolComboGained;

    public void Initialize()
    {
        foreach(var cell in _gameState.Cells.Values)
        {
            cell.OnAllPainted
                .Where(_ => !IsGameFinished())
                .Subscribe(_ => EnqueueCell(cell))
                .AddTo(_disposables);
        }

        ComboCounter
            .Where(combo => combo != 0 && combo % 10 == 0)
            .Subscribe(combo => AnnounceCoolCombo(combo))
            .AddTo(_disposables);


        IsSpreading
            .Skip(1)
            .Where(isSpreading => !isSpreading)
            .Subscribe(_ => _turnManager.EndTurn())
            .AddTo(_disposables);
    }

    private void EnqueueCell(Cell cell)
    {
        _pendingCells.Enqueue(cell);
        if (!IsSpreading.Value)
        {
            ComboCounter.Value = 0;
            ProcessQueue().Forget(ex => Debug.LogError(ex));
        }
    }

    private async UniTask ProcessQueue()
    {
        IsSpreading.Value = true;

        while (_pendingCells.Count > 0)
        {
            var waveSize = _pendingCells.Count;
            ComboCounter.Value++;

            var waveCells = new Cell[waveSize];
            for (int i = 0; i < waveSize; i++)
                waveCells[i] = _pendingCells.Dequeue();

            var tasks = new UniTask[waveSize];
            for (int i = 0; i < waveSize; i++)
                tasks[i] = SpreadFrom(waveCells[i]);

            await UniTask.WhenAll(tasks);

            if (IsGameFinished())
            {
                _gameState.IsGameFinished.Value = true;
                break;
            }
        }

        IsSpreading.Value = false;
    }

    private async UniTask SpreadFrom(Cell cell)
    {
        var neighbors = new (Vector2Int direction, bool hasNeighbor)[]
        {
            (Vector2Int.up, cell.HasNorthNeighbor),
            (Vector2Int.left, cell.HasWestNeighbor),
            (Vector2Int.right, cell.HasEastNeighbor),
            (Vector2Int.down, cell.HasSouthNeighbor)
        };

        var color = cell.Color.Value;

        cell.Clear();

        await cell.PlaySpreadAnimation(color);

        foreach (var (direction, hasNeighbor) in neighbors)
        {
            if (hasNeighbor)
            {
                var neighborPosition = cell.GridPosition + direction;
                _gameState.Cells[neighborPosition].SpreadWith(color);
            }
        }
    }

    private bool IsGameFinished()
    {
        if (_gameState.Cells.Values
            .Where(cell => cell.Color.Value != GameColor.NONE)
            .Select(cell => cell.Color.Value)
            .Distinct()
            .Count() == 1 
            && _turnManager.Players.All(player => player.IsMadeFirstTurn))
        {
            return true;
        }
        return false;
    }

    private void AnnounceCoolCombo(int combo)
    {
        _onCoolComboGained.OnNext(ZString.Format("{0} сделал <color=#30D5C8><b>{1}x Комбо</b></color>!", _turnManager.Players[_turnManager.CurrentPlayerIndex].Name.ColorWith(_colorConfig.ColorDictionary[_turnManager.Players[_turnManager.CurrentPlayerIndex].Color].Color), combo));
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}
