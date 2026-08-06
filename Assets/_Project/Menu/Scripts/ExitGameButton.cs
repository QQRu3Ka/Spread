using UnityEngine;
using UnityEngine.UI;

public class ExitGameButton : MonoBehaviour
{
    [SerializeField] private Button _exitGameButton;

    private void Start()
    {
        _exitGameButton.onClick.AddListener(ExitGame);
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        _exitGameButton.onClick.RemoveAllListeners();
    }
}
