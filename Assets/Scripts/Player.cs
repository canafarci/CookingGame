using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool IsWalking { get { return _isWalking; } }
    [SerializeField] private float _moveSpeed, _rotateSpeed;
    private bool _isWalking;
    private void Update()
    {
        Vector2 inputVector = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y -= 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x += 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x -= 1f;
        }

        inputVector = inputVector.normalized;
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        transform.position += moveDir * Time.deltaTime * _moveSpeed;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * _rotateSpeed);

        //set variables
        _isWalking = inputVector != Vector2.zero;
    }
}
