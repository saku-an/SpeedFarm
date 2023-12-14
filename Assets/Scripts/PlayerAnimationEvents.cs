using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    Player _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndWatering()
    {
        _player.IsWatering = false;
    }
}
