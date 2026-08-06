using UnityEngine;
using Zenject;

public class CameraRotation : MonoBehaviour
{
    [Inject] private GameInputSystem _inputSystem;

    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;

    private Transform _cameraTransform;

    private void Start()
    {
        _cameraTransform = GetComponent<Transform>();
        _inputSystem.Enable();
    }

    private void Update()
    {
        if (_inputSystem.Camera.RightClick.IsPressed())
        {
            var moveValue = _inputSystem.Camera.Move.ReadValue<Vector2>();

            if(moveValue != Vector2.zero)
            {
                var angles = _cameraTransform.eulerAngles;

                angles.x -= moveValue.y * _rotationSpeed * Time.deltaTime;
                angles.x = Mathf.Clamp(angles.x, _minX, _maxX);

                angles.y += moveValue.x * _rotationSpeed * Time.deltaTime;
                
                _cameraTransform.eulerAngles = angles;
            }
        }
    }

    private void OnDestroy()
    {
        _inputSystem.Disable();
    }
}
