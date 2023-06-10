using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
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
        _animator.SetBool(_isWalkingHash, _player.IsWalking);
    }
}
