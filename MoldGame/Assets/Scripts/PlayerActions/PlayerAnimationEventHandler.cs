using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    private PlayerController _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponentInParent<PlayerController>();
    }

    public void PlayFootstep()
    {
        if (_player != null)
        {
            _player.AnimationEventTrigger(PlayerController.AnimationTriggerType.PlayFootstep);
        }
    }
}
