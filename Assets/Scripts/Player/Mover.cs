using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Mover : NetworkBehaviour
{
    [SerializeField] private float _moveSpeed, _rotateSpeed;
    private CharacterController _characterController;
    private bool _isWalking;
    //Properties
    public bool IsWalking { get { return _isWalking; } }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        //pop call stack frame if client is not the owner of the object
        if (!IsOwner) return;

        HandleMovement();
    }
    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetInputVectorNormalized();
        //set variables
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        CollisionFlags collisionFlag = CollisionFlags.None;
        if (moveDir != Vector3.zero)
        {
            float moveDistance = _moveSpeed * Time.deltaTime;
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * _rotateSpeed);
            collisionFlag = _characterController.Move(moveDir * moveDistance);
        }
        _isWalking = collisionFlag == CollisionFlags.None && inputVector != Vector2.zero;
    }
}
