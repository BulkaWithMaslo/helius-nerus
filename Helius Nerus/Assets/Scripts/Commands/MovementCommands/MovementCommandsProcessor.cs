﻿using UnityEngine;

[System.Serializable]
public class MovementCommandsProcessor
{
    [SerializeField] private ProcessorThread[] _threads = null;


    private Transform _enemyShip = null;

    public void Initialize(Transform enemyShip)
    {
        _enemyShip = enemyShip;

        for (int i = 0; i < _threads.Length; ++i)
            _threads[i].Initialize(_enemyShip);
    }

    public void Tick()
    {
        for (int i = 0; i < _threads.Length; ++i)
            _threads[i].Tick();
    }
}