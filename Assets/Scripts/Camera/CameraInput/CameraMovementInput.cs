using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovementInput : CameraMovementInputBase
{
    [SerializeField] private LayerMask _clickableMask;

    private bool _dragEnabled;
    private Camera _camera;

    protected override void Awake()
    {
        base.Awake();

        _camera = GetComponent<Camera>();
    }

    protected override ICameraMovementHandler CreateMovementHandler()
    {
        return new SmoothCameraMovementHandler(_properties);
    }

    protected override Vector3 ReadInputDelta()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(IsClickedOnGround())
            {
                _dragEnabled = true;
            }

            return Vector3.zero;
        }

        if(Input.GetMouseButtonUp(0))
        {
            _dragEnabled = false;
            return Vector3.zero;
        }

        if (_dragEnabled && Input.GetMouseButton(0))
        {
            return Input.mousePositionDelta;
        }

        return Vector3.zero;
    }

    private bool IsClickedOnGround()
    {
        var pointerScreenPosition = Input.mousePosition;
        var ray = _camera.ScreenPointToRay(pointerScreenPosition);
        bool result = Physics.Raycast(ray, out var hit, float.MaxValue, _clickableMask.value);
        return result;
    }
}
