using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private Resource _resourcePrefab;
    [SerializeField] private Transform _groundTransform;
    [SerializeField] private Vector3 _center;
    [SerializeField] private float _range = 30;
    [SerializeField] private float _spawnTime = 5f;

    private int _attemptAmount = 3;

    private void Start()
    {
        StartCoroutine(ResourceSpawning());
    }

    private IEnumerator ResourceSpawning()
    {
        var wait = new WaitForSeconds(_spawnTime);

        while(enabled)
        {
            if (IsPointAccessible(transform.position, _range, out Vector3 point))
            {
                Instantiate(_resourcePrefab, point, Quaternion.identity, _groundTransform);
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
            }

            yield return wait;
        }
    }

    private bool IsPointAccessible(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < _attemptAmount; i++)
        {
            Vector3 randomPoint = center + new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));  
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}
