using UnityEngine;
using Zenject;

public class AnimatedBlock : MonoBehaviour
{
    [Inject] private ColorConfig _colorConfig;
    [SerializeField] private MeshRenderer _meshRenderer;

    public void PaintWith(GameColor color)
    {
        _meshRenderer.material = _colorConfig.ColorDictionary[color].BlockMaterial;
    }
}
