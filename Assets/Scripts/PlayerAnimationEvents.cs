using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    Player _player;
    
    void Start()
    {
        _player = GetComponentInParent<Player>();
    }

    
    void Update()
    {
        
    }

    // Called on the last frame of the watering animation
    public void EndWatering()
    {
        _player.CurrentState = _player.IdleState;
        _player.CurrentState.EnterState();
    }

    // Called on the last frame of the attack animation
    public void EndAttack()
    {
        _player.CurrentState = _player.IdleState;
        _player.CurrentState.EnterState();
    }
}
