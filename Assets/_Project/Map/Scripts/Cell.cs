using Cysharp.Threading.Tasks;
using R3;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using ZLinq;

public class Cell : MonoBehaviour
{
    [Inject] private ColorConfig _colorConfig;
    [Inject] private TurnManager _turnManager;

    [SerializeField] private MeshRenderer _cellMeshRenderer;
    [SerializeField] private MeshRenderer _centralBlock;
    [SerializeField] private Material _centralBlockOff;
    [SerializeField] private Material _centralBlockOn;

    private Animator _animator;
    private List<Block> _blocks;
    private List<AnimatedBlock> _animatedBlocks;
    private Subject<Unit> _onAllPainted = new();
    private bool _isSpreading;

    public Vector2Int GridPosition;
    public ReactiveProperty<GameColor> Color = new();

    [Header("Соседние клетки")]
    public bool HasNorthNeighbor;
    public bool HasWestNeighbor;
    public bool HasEastNeighbor;
    public bool HasSouthNeighbor;

    public Observable<Unit> OnAllPainted => _onAllPainted;
    public bool IsSpreading => _isSpreading;

    private void Awake()
    {
        Color.Value = GameColor.NONE;
        _blocks = transform.Children().OfComponent<Block>().ToList();
        _animatedBlocks = transform.Children().OfComponent<AnimatedBlock>().ToList();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        foreach(var block in _blocks)
        {
            block.OnClickEvent
                .Where(color => Color.Value == color || Color.Value == GameColor.NONE)
                .Subscribe(color =>
                {
                    Color.Value = color;
                    block.PaintWith(color);
                    if (_blocks.All(block => block.IsPainted))
                    {
                        _isSpreading = true;
                        _onAllPainted.OnNext(Unit.Default);
                    }
                    else
                    {
                        _turnManager.EndTurn();
                    }
                }).AddTo(this);
        }

        Color.Subscribe(color => {
                _cellMeshRenderer.material = _colorConfig.ColorDictionary[color].CellMaterial;
                _centralBlock.material = color != GameColor.NONE ? _centralBlockOn : _centralBlockOff;
            })
            .AddTo(this);
    }

    public void SpreadWith(GameColor color)
    {
        if (_isSpreading) return;

        Color.Value = color;

        var freeBlock = _blocks.FirstOrDefault(block => !block.IsPainted);

        if(freeBlock != null)
        {
            freeBlock.PaintWith(color);

            foreach (var block in _blocks.Where(block => block.IsPainted))
            {
                block.PaintWith(color);
            }
            if (_blocks.All(block => block.IsPainted))
            {
                _isSpreading = true;
                _onAllPainted.OnNext(Unit.Default);
            }
        }
    }

    public async UniTask PlaySpreadAnimation(GameColor color)
    {
        foreach(var animatedBlock in _animatedBlocks)
        {
            animatedBlock.PaintWith(color);
        }
        _animator.SetTrigger("Spread");
        await UniTask.Delay(1000);
        _isSpreading = false;
    }

    public void Clear()
    {
        Color.Value = GameColor.NONE;
        foreach(var block in _blocks)
        {
            block.Clear();
        }
    }
}
