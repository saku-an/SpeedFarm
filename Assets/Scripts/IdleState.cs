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
        //_idleTimer = _enemy.StartCoroutine(IdleTimer());
    }

    private IEnumerator IdleTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("heh");
        }
    }
}
