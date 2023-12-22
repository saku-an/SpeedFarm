using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{
    private float _walkRange;
    private Enemy _enemy;
    private Coroutine _idleTimer;

    public IdleState(Enemy enemy)
    {
        _enemy = enemy;
        _walkRange = 2f;
    }

    public void UpdateState()
    {
        
    }

    public void EnterState()
    {
        _idleTimer = _enemy.StartCoroutine(IdleTimer());
    }

    private IEnumerator IdleTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            SearchForPlants();
        }
    }

    // Searches for plants, if found enter chase state
    private void SearchForPlants()
    {
        if (Plant.GrowingPlants.Count > 0)
        {
            _enemy.StopCoroutine(_idleTimer);

            _enemy.CurrentState = _enemy.ChaseState;
            _enemy.CurrentState.EnterState();
        }
    }
}
