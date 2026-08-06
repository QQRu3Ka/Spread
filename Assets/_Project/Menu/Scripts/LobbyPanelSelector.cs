using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ZLinq;

public class LobbyPanelSelector : MonoBehaviour
{
    [Inject] private PlayerStorage _playerStorage;

    [SerializeField] private Button _toLobbyButton;
    [SerializeField] private TextMeshProUGUI _gameTitle;
    [SerializeField] private GameObject _buttonPanel;
    [SerializeField] private float _animationDuration;
    [SerializeField] private GameObject _lobbyPanel;

    private void Start()
    {
        _toLobbyButton.onClick.AddListener(ToLobby);
    }

    private async void ToLobby()
    {
        await PlayAnimation();
        gameObject.SetActive(false);
        _lobbyPanel.SetActive(true);
        _playerStorage.CreateNewPlayer();
    }

    private async UniTask PlayAnimation()
    {
        var animationSequence = LSequence.Create();
        var titleAnimation = LMotion.Create(_gameTitle.fontSize, _gameTitle.fontSize - 40f, _animationDuration)
            .BindToFontSize(_gameTitle);
        animationSequence.Append(titleAnimation);

        var tmp = _buttonPanel.Children().OfComponent<Transform>().Count() / 10f;

        foreach (var obj in _buttonPanel.Children().OfComponent<Transform>())
        {
            var buttonAnimation = LMotion.Create(obj.localPosition.y, obj.localPosition.y - 400f, _animationDuration)
                .WithEase(Ease.InBack)
                .BindToLocalPositionY(obj);
            animationSequence.Insert(tmp, buttonAnimation);
            tmp -= 0.1f;
        }

        await animationSequence.Run().ToUniTask();
    }

    private void OnDestroy()
    {
        _toLobbyButton.onClick.RemoveAllListeners();
    }
}
