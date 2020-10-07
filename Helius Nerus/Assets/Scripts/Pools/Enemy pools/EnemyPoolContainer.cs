﻿using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolContainer : MonoBehaviour
{
    public static EnemyPoolContainer Instance
    {
        get;
        private set;
    }

    [Tooltip("ДОЛЖНЫ ИДТИ В ТОМ ЖЕ ПОРЯДКЕ, ЧТО И ТИПЫ РВАГВО В ПЕРЕЧИСЛЕНИИ")]
    [SerializeField] private GameObject[] _enemiesPrefabs = null;
    private List<EnemyPool> _pools = new List<EnemyPool>();
    private Transform _transform;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        _transform = transform;

        for (int i = 0; i < _enemiesPrefabs.Length; ++i)
        {
            _pools.Add(new EnemyPool(_enemiesPrefabs[i], _transform));
        }
    }

    public GameObject GetObjectFromPool(EnemyTypes type)
    {
        return _pools[(int)type].GetObjectFromPool();
    }

    public void ReturnObjectToPool(EnemyTypes type, GameObject go)
    {
        _pools[(int)type].ReturnObjectToPool(go);
    }
}

public class EnemyPool
{
    private GameObject _prefab = null;
    private Transform _transform = null;
    private Queue<GameObject> _pool = new Queue<GameObject>();

    public EnemyPool(GameObject prefab, Transform transform)
    {
        _prefab = prefab;
        _transform = transform;
    }

    private void AddObjectToPool(int amount)
    {
        GameObject go;
        for (int i = 0; i < amount; ++i)
        {
            go = GameObject.Instantiate(_prefab);
            go.transform.parent = _transform;
            go.SetActive(false);
            _pool.Enqueue(go);
        }
    }

    public GameObject GetObjectFromPool()
    {
        if (_pool.Count > 0)
        {
            GameObject go;
            go = _pool.Dequeue();
            go.SetActive(true);
            // Re-enable processor
            // Reset hp
            go.GetComponent<Enemy>().Reset();
            return go;
        }
        else
        {
            AddObjectToPool(3);
            return GetObjectFromPool();
        }
    }

    public void ReturnObjectToPool(GameObject go)
    {
        go.transform.parent = _transform;
        go.SetActive(false);
        _pool.Enqueue(go);
    }
}

public enum EnemyTypes
{
    SquareEnemy,
    RoundEnemy,
    TriangleEnemy,
}