using UnityEngine;

public class SmoothCameraMovementHandler : MonoBehaviour
{
    [SerializeField] private CameraMovementProperties _properties;
    [SerializeField] private Vector3 _cachedCameraPosition;
    [SerializeField] private CameraMovementInput _cameraInput;

    private void Start()
    {
        _cachedCameraPosition = _properties.Pivot.position;
    }

    private void OnEnable()
    {
        _cameraInput.InputPerformed += OnInputPerformed;
    }

    private void OnDisable()
    {
        _cameraInput.InputPerformed -= OnInputPerformed;
    }

    private void OnInputPerformed(Vector3 inputDelta)
    {
        _cachedCameraPosition += new Vector3(inputDelta.x, 0, inputDelta.y) * _properties.Speed;

        _properties.Pivot.position = Vector3.Lerp(_properties.Pivot.position, _cachedCameraPosition,
            Time.deltaTime / _properties.Smoothness);
    }
}
