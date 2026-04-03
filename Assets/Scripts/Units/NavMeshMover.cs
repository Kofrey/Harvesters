using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class NavMeshMover : MonoBehaviour
{
    [SerializeField] private UnityEngine.AI.NavMeshAgent _agent;
    [SerializeField] private AnimatorAgent _animatorAgent;

    private float _distanceToReachpoint = 0.9f;
    private Vector3 _targetPosition;
    private Vector3 _lastPosition;
    private float _inaccuracyComparing = 0.01f;

    private WaitForSeconds _wait;
    private float _checkTimer = 0.5f;
    private Coroutine _CheckAchieving;

    public event Action<Vector3> ReachedTarget;

    private void Start()
    {
        _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        _lastPosition = transform.position;
        _wait = new WaitForSeconds(_checkTimer);
    }

    public void SetTarget(Transform targetTransform)
    {
        if (targetTransform == null)
        {
            _agent.isStopped = true;
        }    
        else
        {
            _targetPosition = targetTransform.position;
            _agent.isStopped = false;
            _agent.SetDestination(_targetPosition);  
            _CheckAchieving = StartCoroutine(CheckAchieveTargetPoint());
        }    
    }

    private bool IsTargetReached(Vector3 distance, float distanceToCheck)
    {
        return distance.sqrMagnitude < distanceToCheck * distanceToCheck;
    }

    private IEnumerator CheckAchieveTargetPoint()
    {   
        while(enabled)
        {
            if (IsTargetReached(_targetPosition - transform.position, _distanceToReachpoint))
            {
                
                ReachedTarget?.Invoke(_targetPosition);
                StopCoroutine(_CheckAchieving);
            }

            if ((transform.position - _lastPosition).magnitude >= _inaccuracyComparing)
                _animatorAgent.SetBool(_animatorAgent.IsMoveSelf, true);
            else
                _animatorAgent.SetBool(_animatorAgent.IsMoveSelf, false);
        
            _lastPosition = transform.position;

            yield return _wait;
        }
    }
}
