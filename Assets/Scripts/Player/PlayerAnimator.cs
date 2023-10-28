using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    private Animator _animator;
    private Vector3 _lastPosition;
    private float _lastSpeed = 0f;
    private const float _interpolationFactor = 100f;
    private static readonly int _isWalkingSpeedHash = Animator.StringToHash("Speed");
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        //pop call stack frame if not the owner
        if (!IsOwner) return;

        float speed = GetSpeed(transform.position);

        _animator.SetFloat(_isWalkingSpeedHash, speed);
    }

    private float GetSpeed(Vector3 currentPosition)
    {
        float speed;

        if (_lastPosition == currentPosition)
        {
            speed = 0f;
        }
        else
        {
            speed = 1f;
        }

        speed = Mathf.Lerp(speed, _lastSpeed, Time.deltaTime * _interpolationFactor);

        _lastPosition = transform.position;
        _lastSpeed = speed;

        return speed;
    }
}
