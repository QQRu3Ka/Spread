using Coffee.UIEffects;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

public class LobbyPanelAnimation : MonoBehaviour
{
    [SerializeField] private Transform _mapsPanel;
    [SerializeField] private Transform _playersPanel;
    [SerializeField] private Transform _buttonsPanel;
    [SerializeField] private UIEffect _selectedMapPanelUIEffect;

    private void OnEnable()
    {
        PlayAppearAnimation();
    }

    private void PlayAppearAnimation()
    {
        var mapsPanelMotion = LMotion.Create(_mapsPanel.localPosition.x - 500f, _mapsPanel.localPosition.x, 2f)
            .WithEase(Ease.OutBack)
            .BindToLocalPositionX(_mapsPanel);

        var playerPanelMotion = LMotion.Create(_playersPanel.localPosition.x + 500f, _playersPanel.localPosition.x, 2f)
            .WithEase(Ease.OutBack)
            .BindToLocalPositionX(_playersPanel);

        var buttonsPanelMotion = LMotion.Create(_buttonsPanel.localPosition.y - 400f, _buttonsPanel.localPosition.y, 2f)
            .WithEase(Ease.OutBack)
            .BindToLocalPositionY(_buttonsPanel);

        var selectedMapPanelMotion = LMotion.Create(1f, 0f, 2f).Bind(x => _selectedMapPanelUIEffect.transitionRate = x);

        LSequence.Create()
            .Join(mapsPanelMotion)
            .Join(playerPanelMotion)
            .Join(buttonsPanelMotion)
            .Join(selectedMapPanelMotion)
            .Run();
    }
}
