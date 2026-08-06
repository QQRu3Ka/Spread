using R3;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class Block : MonoBehaviour, IPointerDownHandler
{
    [Inject] private ColorConfig _colorConfig;
    [Inject] private TurnManager _turnManager;
    [Inject] private SpreadResolver _spreadResolver;
    [Inject] private GameState _gameState;

    [SerializeField] private MeshRenderer _blockMeshRenderer;

    private Subject<GameColor> _onClickEvent = new();

    public bool IsPainted;

    public Observable<GameColor> OnClickEvent => _onClickEvent;

    private void Start()
    {
        _gameState.IsGameFinished
            .Where(isGameFinished => isGameFinished)
            .Subscribe(_ => _onClickEvent.OnCompleted())
            .AddTo(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && !_spreadResolver.IsSpreading.Value)
        {
            _onClickEvent.OnNext(_turnManager.Players[_turnManager.CurrentPlayerIndex].Color);
        }
    }

    public void PaintWith(GameColor color)
    {
        IsPainted = true;
        _blockMeshRenderer.material = _colorConfig.ColorDictionary[color].BlockMaterial;
    }

    public void Clear()
    {
        IsPainted = false;
        _blockMeshRenderer.material = _colorConfig.ColorDictionary[GameColor.NONE].BlockMaterial;
    }
}
