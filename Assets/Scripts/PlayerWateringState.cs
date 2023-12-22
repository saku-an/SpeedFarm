using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWateringState : IPlayerState
{
    private Player _player;

    public PlayerWateringState(Player player)
    {
        _player = player;
    }

    public void UpdateState()
    {

    }

    // Check for a water source, if found fill the can
    // If no water source, we deplete 1 charge of water
    // If no water in the can, we go back to idle state
    public void EnterState()
    {
        _player.Rb.velocity = Vector2.zero;
        List<Collider2D> colliders = _player.CollidersOnHighlight();
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("WaterSource"))
            {
                _player.WaterInCan = _player.WaterCanMax;
                _player.PlayerAnim.SetTrigger("Watering");

                return;
            }
        }
        if (_player.WaterInCan > 0)
        {
            _player.WaterInCan--;
           _player.PlayerAnim.SetTrigger("Watering");


            GameObject water = Object.Instantiate(_player.WaterTrigger, _player.Highlight.transform.position, Quaternion.identity);
            Object.Destroy(water, 0.5f);
        }
        else
        {
            _player.CurrentState = _player.IdleState;
            _player.CurrentState.EnterState();
        }
    }

    public void Water()
    {

    }

    public void Attack()
    {

    }
}
