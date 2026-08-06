using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private Image _colorImage;
    [SerializeField] private TMP_InputField _nameInputField;

    public Player _dedicatedPlayer;

    public void SetPlayer(Player player)
    {
        _dedicatedPlayer = player;

        _nameInputField.OnValueChangedAsObservable()
            .Subscribe(name => _dedicatedPlayer.Name = name)
            .AddTo(this);
    }

    public void SetColor(Color color)
    {
        _colorImage.color = color;
    }
}
