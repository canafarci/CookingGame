using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    private Animator _animator;
    private Player _player;
    private static readonly int _isWalkingHash = Animator.StringToHash("IsWalking");
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        //pop call stack frame if not the owner
        if (!IsOwner) return;
        _animator.SetBool(_isWalkingHash, _player.IsWalking);
    }
}
