using UnityEditor.VersionControl;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Unit : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _carryingPoint;
    [SerializeField] private Transform _baseStock;
    [SerializeField] private Animator _animator;
    [SerializeField] private UnityEngine.AI.NavMeshAgent _agent;

    private Vector3 _lastPosition;
    private float _checkTargetTime = 0.3f;
    private float _checkTimer;
    private float _distanceToTakeResource = 0.8f;
    private float _distanceToRaycastResource = 5f;
    private Coroutine _moveCoroutine;
    private Vector3 _targetPosition;
    private bool _isReturning = false;

    private Resource _carryingResource;

    private static int s_isMoveSelf;

    public bool IsActive => _target != null;
    public bool HaveResource => _carryingResource != null;
    public bool IsReturning => _isReturning;

    private void Awake()
    {
        s_isMoveSelf = Animator.StringToHash("isMoveSelf");
        _lastPosition = transform.position;
        _checkTimer = 0;
        _moveCoroutine = StartCoroutine(MoveToTarget(_checkTargetTime));
    }

    public void SetTarget(Transform transform)
    {
        _target = transform;
        _targetPosition = transform.position;
        _agent.SetDestination(_target.position);      
    }

    public void SetBase(Transform transform)
    {
        _baseStock = transform;
    }

    public void Refresh()
    {
        _target = null;
        _isReturning = false;
    }

    public Resource GetResource()
    {
        Resource resource = _carryingResource;
        _carryingResource = null;
        return resource;
    }

    private void TryTakeResource()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(_carryingPoint.position, (_targetPosition - _carryingPoint.position), _distanceToRaycastResource);

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

        _isReturning = true;
        SetTarget(_baseStock);
    }

    private IEnumerator MoveToTarget(float checkTime)
    {   
        if (_target == null)
            yield return null;

        while(enabled)
        {
            if (_checkTimer < checkTime)
            {
                _checkTimer += Time.deltaTime;
            }
            else
            {
                if (IsTargetReached(_targetPosition - transform.position, _distanceToTakeResource) && !_isReturning)
                {
                    TryTakeResource();
                }

                if (transform.position != _lastPosition)
                    _animator.SetBool(s_isMoveSelf, true);
                else
                    _animator.SetBool(s_isMoveSelf, false);
        
                _lastPosition = transform.position;
                _checkTimer += Time.deltaTime - checkTime;
            }

            yield return null;
        }    
    }

    private bool IsTargetReached(Vector3 distance, float distanceToCheck)
    {
        return distance.sqrMagnitude < distanceToCheck * distanceToCheck;
    }
}