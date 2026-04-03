using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private T _prefab;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 20;

    protected ObjectPool<T> Pool;

    private void Awake()
    {
        Pool = new ObjectPool<T>(
        createFunc: () => Create(_prefab),
        actionOnGet: (T) => ActionOnGet(T),
        actionOnRelease: (T) => ActionOnRelease(T),
        actionOnDestroy: (T) => OnObjectDestroy(T),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolMaxSize);
    }

    public void Reset()
    {
        T[] allActive = _container.GetComponentsInChildren<T>(); 

        foreach (T child in allActive)
        {
            Pool.Release(child);  
        }  
    }

    public T GetObject()
    {
        return Pool.Get();
    }

    protected virtual void OnObjectDestroy(T obj) 
    {
        (obj as SpawnedObject).Consumed -= OnObjectConsumed;
        Destroy(obj.gameObject);
    }

    protected virtual void OnObjectConsumed(SpawnedObject obj)
    {
        Pool.Release(obj as T);
    }  

    protected virtual void ActionOnRelease(T obj)
    {
       obj.gameObject.SetActive(false); 
       obj.transform.SetParent(_container); 
    }

    protected virtual T Create(T obj)
    {
        T newObj = Instantiate(obj, _container);
        (newObj as SpawnedObject).Consumed += OnObjectConsumed;
        return newObj;
    }

    private void ActionOnGet(T obj)
    {
        obj.gameObject.SetActive(true);
    }
}
