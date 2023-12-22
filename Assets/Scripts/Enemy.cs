using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public Animator EnemyAnim;
    [HideInInspector] public Transform Target;

    public IEnemyState CurrentState;
    public IdleState IdleState;
    public ChaseState ChaseState;

    private void Awake()
    {
        EnemyAnim = GetComponent<Animator>();

        IdleState = new IdleState(this);
        ChaseState = new ChaseState(this);
        CurrentState = IdleState;

        Plant.OnPlantDestroyed += OnPlantDestroyed;
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentState.EnterState();
    }

    // Update is called once per frame
    void Update()
    {
        CurrentState.UpdateState();
    }

    // Checks if the plant that was destroyed was the current target
    private void OnPlantDestroyed(Transform plant)
    {
        if (Target == plant)
        {
            EnemyAnim.SetBool("Walking", false);

            CurrentState = IdleState;
            IdleState.EnterState();
        }
    }
}
