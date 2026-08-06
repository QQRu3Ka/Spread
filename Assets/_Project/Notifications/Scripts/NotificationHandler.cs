using Cysharp.Threading.Tasks;
using R3;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class NotificationHandler : MonoBehaviour
{
    [Inject] private TurnManager _turnManager;
    [Inject] private SpreadResolver _spreadResolver;
    [Inject] private GameState _gameState;

    [SerializeField] private GameObject _notificationMessagePrefab;
    [SerializeField] private int _maxMessagesOnScreen;

    private List<GameObject> _messages = new();

    private void Start()
    {
        _turnManager.OnNextPlayerSelected
            .Subscribe(async msg => await InvokeNotification(msg))
            .AddTo(this);

        _spreadResolver.OnCoolComboGained
            .Subscribe(async msg => await InvokeNotification(msg))
            .AddTo(this);

        _gameState.OnGameFinished
            .Subscribe(async msg => await InvokeNotification(msg))
            .AddTo(this);

        _turnManager.OnPlayersLost
            .Subscribe(async msg => await InvokeNotification(msg))
            .AddTo(this);
    }

    private async UniTask InvokeNotification(string text)
    {
        if(_messages.Count >= _maxMessagesOnScreen)
        {
            Destroy(_messages[0]);
            _messages.Remove(_messages[0]);
        }

        var notification = Instantiate(_notificationMessagePrefab, transform);
        _messages.Add(notification);
        var message = notification.GetComponent<NotificationMessage>();
        message.SetText(text);

        await message.FadeOut();
    }
}
