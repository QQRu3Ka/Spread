using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

public class AppearAnimation : MonoBehaviour
{
    private void Start()
    {
        LSequence.Create()
            .Join(LMotion.Create(0f, 1f, 2f).WithEase(Ease.OutBack).BindToLocalScaleX(transform))
            .Join(LMotion.Create(0f, 1f, 2f).WithEase(Ease.OutBack).BindToLocalScaleY(transform))
            .Join(LMotion.Create(0f, 1f, 2f).WithEase(Ease.OutBack).BindToLocalScaleZ(transform))
            .Join(LMotion.Create(0f, 360f, 2f).WithEase(Ease.OutCubic).BindToLocalEulerAnglesY(transform))
            .Run();
    }
}
