using UnityEditor.VersionControl;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{
    [SerializeField] private Transform _carryingPoint;
    [SerializeField] private Transform _baseStock;
    [SerializeField] private NavMeshMover _mover;

    private float _distanceToTakeResource = 0.8f;
    private float _distanceToRaycastResource = 5f;
    private State _state;
    private Resource _carryingResource;    

    public bool IsActive => _state != State.Idle;
    public bool HaveResource => _carryingResource != null;

    public event Action<Unit> ReturnToBase;

    private void Awake()
    {
        _state = State.Idle;
    }

    private void OnEnable()
    {
        _mover.ReachedTarget += OnReachedTarget;
    }

    private void OnDisable()
    {
        _mover.ReachedTarget -= OnReachedTarget;
    }

    public void SetBase(Transform transform)
    {
        _baseStock = transform;
    }

    public void Refresh()
    {
        _mover.SetTarget(null);
        _state = State.Idle;
    }

    public Resource GetResource()
    {
        Resource resource = _carryingResource;
        _carryingResource = null;
        return resource;
    }

    public void SetGoal(Transform targetTransform, State state)
    {
        _state = state;
        _mover.SetTarget(targetTransform);
    }

    public enum State
    {
        Idle,
        Harvesting,
        Returning
    }

    private void OnReachedTarget(Vector3 targetPosition)
    {   
        switch(_state)
        {
            case State.Harvesting:
                TryTakeResource(targetPosition);
                break;

            case State.Returning:
                ReturnToBase?.Invoke(this);
                break;
        }
    }

    private void TryTakeResource(Vector3 targetPosition)
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(_carryingPoint.position, (targetPosition - _carryingPoint.position), _distanceToRaycastResource);

        foreach (RaycastHit hit in hits)
        {
            if(hit.transform.TryGetComponent(out Resource resource))
            {
                resource.transform.position = _carryingPoint.position;
                resource.transform.SetParent(_carryingPoint, true);
                _carryingResource = resource;
                break;
            }
        }

        SetGoal(_baseStock, State.Returning);
    }
}