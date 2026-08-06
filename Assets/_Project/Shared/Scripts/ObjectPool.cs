using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool
{
    private ObjectPool<GameObject> _pool;
    private GameObject _prefab;

    public ObjectPool(GameObject prefab, int initObjectCount)
    {
        _prefab = prefab;
        _pool = new(OnCreate, OnGet, OnRelease, OnDestroy, false, initObjectCount);
    }

    public GameObject Get()
    {
        return _pool.Get();
    }

    public void Release(GameObject obj)
    {
        if (obj == null) return;
        _pool.Release(obj);
    }

    private GameObject OnCreate()
    {
        return GameObject.Instantiate(_prefab);
    }

    private void OnGet(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnRelease(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void OnDestroy(GameObject obj)
    {
        GameObject.Destroy(obj);
    }
}
