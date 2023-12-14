using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private IEnemyState _currentState;
    private IdleState _idleState;
    private ChaseState _chaseState;

    private void Awake()
    {
        _idleState = new IdleState(this);
        _chaseState = new ChaseState(this);
        _currentState = _idleState;
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentState.EnterState();
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.UpdateState();
    }
}
