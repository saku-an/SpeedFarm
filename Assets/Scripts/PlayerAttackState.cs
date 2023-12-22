using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : IPlayerState
{
    private Player _player;

    public PlayerAttackState(Player player)
    {
        _player = player;
    }

    public void UpdateState()
    {

    }

    public void EnterState()
    {
        _player.Rb.velocity = Vector2.zero;
        _player.PlayerAnim.SetTrigger("Attack");
    }

    public void Water()
    {

    }

    public void Attack()
    {

    }
}
