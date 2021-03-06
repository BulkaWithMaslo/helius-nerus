﻿using UnityEngine;

class HorizontalMoveCommand : MoveCommand
{
	private float _timeElapsed = 0.0f;

    public HorizontalMoveCommand()
    {

    }

    public override bool IsEnded()
	{
		return _timeElapsed > CommandData.EndParameter;
	}

	public override void Reset()
	{
		_timeElapsed = 0.0f;
	}

	public override void Tick(Transform ship)
	{
        _timeElapsed += TimeManager.EnemyDeltaTime * CommandData.TimeScale;
		ship.Translate(Vector3.right * CommandData.MovementMultiplier * TimeManager.EnemyDeltaTime
            * CommandData.TimeScale, Space.World);
	}
}