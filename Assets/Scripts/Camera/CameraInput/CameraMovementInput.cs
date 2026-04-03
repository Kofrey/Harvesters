using UnityEngine;
using System;

[RequireComponent(typeof(Camera))]
public class CameraMovementInput : MonoBehaviour
{
    [SerializeField] private LayerMask _clickableMask;

    private KeyCode _buttonKey = KeyCode.Mouse0;
    private bool _dragEnabled;
    private Camera _camera;

    public event Action<Vector3> InputPerformed;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        InputPerformed?.Invoke(ReadInput());     
    }

    private Vector3 ReadInput()
    {
        if(Input.GetKeyDown(_buttonKey))
        {
            if(IsClickedOnGround())
            {
                _dragEnabled = true;
            }

            return Vector3.zero;
        }

        if(Input.GetKeyUp(_buttonKey))
        {
            _dragEnabled = false;

            return Vector3.zero;
        }

        if (_dragEnabled && Input.GetKey(_buttonKey))
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
