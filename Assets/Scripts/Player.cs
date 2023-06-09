using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool IsWalking { get { return _isWalking; } }
    [SerializeField] private float _moveSpeed, _rotateSpeed;
    private GameInput _gameInput;
    private bool _isWalking;
    private void Awake()
    {
        _gameInput = FindObjectOfType<GameInput>();
    }
    private void Update()
    {
        Vector2 inputVector = _gameInput.GetInputVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        transform.position += moveDir * Time.deltaTime * _moveSpeed;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * _rotateSpeed);

        //set variables
        _isWalking = inputVector != Vector2.zero;
    }
}
