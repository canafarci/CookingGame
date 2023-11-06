using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    [SerializeField] private AnimatorOverrideController _carryingController;
    [SerializeField] private AnimatorController _defaultController;

    private Animator _animator;
    private Vector3 _lastPosition;
    private float _lastSpeed = 0f;
    private const float _interpolationFactor = 20f;
    private static readonly int _isWalkingSpeedHash = Animator.StringToHash("Speed");
    private PlayerKitchenObjectParent _playerKitchenObjectParent;
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerKitchenObjectParent = GetComponent<PlayerKitchenObjectParent>();
    }

    private void Start()
    {
        _playerKitchenObjectParent.OnPlayerKitchenObjectChanged += PlayerKitchenObjectParent_PlayerKitchenObjectChangedHandler;
    }

    private void PlayerKitchenObjectParent_PlayerKitchenObjectChangedHandler(object sender, PlayerKitchenObjectChangedArgs e)
    {
        if (e.IsHoldingItem)
        {
            _animator.runtimeAnimatorController = _carryingController;
        }
        else
        {
            _animator.runtimeAnimatorController = _defaultController;
        }
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

        float t = Time.deltaTime * _interpolationFactor;

        speed = Mathf.Lerp(_lastSpeed, speed, t);

        _lastPosition = transform.position;
        _lastSpeed = speed;

        return speed;
    }
}
