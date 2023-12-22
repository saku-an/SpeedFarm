using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IEnemyState
{

    private Enemy _enemy;

    public ChaseState(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void UpdateState()
    {

    }

    public void EnterState()
    {
        _enemy.Target = Plant.GrowingPlants[0];
        _enemy.GetComponent<AIDestinationSetter>().target = _enemy.Target;

        _enemy.EnemyAnim.SetBool("Walking", true);
    }
}