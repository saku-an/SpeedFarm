using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : IPlayerState
{
    private Player _player;

    public PlayerIdleState(Player player)
    {
        _player = player;
    }

    // Player movement happens here
    public void UpdateState()
    {
        Vector2 move = _player.MoveAction.ReadValue<Vector2>();
        _player.PlayerAnim.SetFloat("X", move.x);
        _player.PlayerAnim.SetFloat("Y", move.y);

        if (move == Vector2.zero)
        {
            _player.PlayerAnim.SetTrigger("Idle");
            _player.Rb.velocity = Vector2.zero;
            return;
        }


        if (Math.Abs(move.y) > Math.Abs(move.x))
        {
            if (move.y > 0)
                _player.FacingDir = Vector3.up;
            else
                _player.FacingDir = Vector3.down;
        }
        else
        {
            if (move.x > 0)
                _player.FacingDir = Vector3.right;
            else
                _player.FacingDir = Vector3.left;
        }

        _player.Rb.velocity = _player.MoveSpeed * move;
    }

    public void EnterState()
    {

    }

    public void Water()
    {
        _player.CurrentState = _player.WateringState;
        _player.CurrentState.EnterState();
    }

    public void Attack()
    {
        _player.CurrentState = _player.AttackState;
        _player.CurrentState.EnterState();
    }
}
