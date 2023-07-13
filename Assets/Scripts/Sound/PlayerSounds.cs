using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Mover _mover;
    private float _footstepTimer;
    private const float FOOTSTEP_TIMER_MAX = .15f;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
    }

    private void Update()
    {
        _footstepTimer += Time.deltaTime;
        if (_footstepTimer > FOOTSTEP_TIMER_MAX)
        {
            _footstepTimer = 0f;

            if (_mover.IsWalking)
                SoundManager.Instance.PlayFootstepSound(transform.position);
        }

    }
}
