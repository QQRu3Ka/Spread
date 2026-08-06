using R3;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class CellStatsHandler : MonoBehaviour
{
    [Inject] private GameState _gameState;
    [Inject] private PlayerStorage playerStorage;
    [Inject] private ColorConfig _colorConfig;

    [SerializeField] private Transform _panelTransform;
    [SerializeField] private GameObject _statPrefab;

    private Dictionary<Player, GameObject> _stats = new();

    private void Start()
    {
        foreach(var player in playerStorage.PlayerList)
        {
            var obj = Instantiate(_statPrefab, _panelTransform);
            _stats.Add(player, obj);
            var stat = obj.GetComponent<PlayerCellStat>();
            stat.SetData(player.Name, 0, _colorConfig.ColorDictionary[player.Color].Color);
        }

        Observable.CombineLatest(_gameState.Cells.Values.Select(cell => cell.Color))
            .Skip(1)
            .Subscribe(_ => UpdateStats())
            .AddTo(this);
    }

    private void UpdateStats()
    {
        var scores = new Dictionary<Player, int>();

        foreach(var playerStat in _stats)
        {
            var stat = playerStat.Value.GetComponent<PlayerCellStat>();
            var cellCount = _gameState.Cells.Values.Count(x => x.Color.Value == playerStat.Key.Color);
            scores.Add(playerStat.Key, cellCount);
            stat.SetScore(cellCount);
        }

        var sorted = _stats.OrderByDescending(pair => scores[pair.Key]).ToList();

        for (var i = 0; i < sorted.Count; i++)
        {
            sorted[i].Value.transform.SetSiblingIndex(i);
        }
    }
}
