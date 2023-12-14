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

    public void EnterState()
    {
        _player.Rb.velocity = Vector2.zero;
        Debug.Log("mo");
    }

    public void Water()
    {

    }
}
