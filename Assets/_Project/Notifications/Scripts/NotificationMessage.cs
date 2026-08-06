using Cysharp.Threading.Tasks;
using LitMotion;
using System;
using System.Threading;
using TMPro;
using UnityEngine;

public class NotificationMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _notificationText;
    [SerializeField] private int DelayTime;
    [SerializeField] private float FadeTime;

    private CancellationTokenSource _cts;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _cts = new CancellationTokenSource();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetText(string text)
    {
        _notificationText.text = text;
    }

    public async UniTask FadeOut()
    {
        //_cts = new CancellationTokenSource();

        try
        {
            await UniTask.Delay(DelayTime, cancellationToken: _cts.Token);

            await LMotion.Create(1f, 0f, FadeTime)
                .Bind(x => _canvasGroup.alpha = x)
                .AddTo(this)
                .ToUniTask(_cts.Token);
        } 
        catch (OperationCanceledException)
        {
            return;
        }

        Destroy(gameObject);
    }

    //public void Clear()
    //{
    //    if(_canvasGroup != null)
    //    {
    //        _canvasGroup.alpha = 1f;
    //    }
    //    _cts?.Cancel();
    //}

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }
}
