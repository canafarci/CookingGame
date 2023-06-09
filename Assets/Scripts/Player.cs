using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool IsWalking { get { return _isWalking; } }
    [SerializeField] private float _moveSpeed, _rotateSpeed;
    const float PLAYER_RADIUS = 0.7f;
    const float PLAYER_HEIGHT = 2f;
    private GameInput _gameInput;
    private bool _isWalking;
    private void Awake()
    {
        _gameInput = FindObjectOfType<GameInput>();
    }
    private void Update()
    {
        Vector2 inputVector = _gameInput.GetInputVectorNormalized();

        //set variables

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float moveDistance = _moveSpeed * Time.deltaTime;
        //check can move
        bool canMove = CanMove(moveDir, moveDistance);

        if (!canMove)
        {
            //cant move, try only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
            canMove = CanMove(moveDirX, moveDistance);
            if (canMove)
            {
                //update vector
                moveDir = moveDirX;
            }
            else
            {
                //if cant move on X, try Z
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                canMove = CanMove(moveDirZ, moveDistance);
                if (canMove)
                {
                    //update direction
                    moveDir = moveDirZ;
                }
            }
        }
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        _isWalking = canMove && inputVector != Vector2.zero;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * _rotateSpeed);
    }

    bool CanMove(Vector3 moveDir, float moveDistance)
    {
        return !Physics.CapsuleCast(transform.position,
                                 transform.position + Vector3.up * PLAYER_HEIGHT,
                                PLAYER_RADIUS,
                                moveDir,
                                moveDistance);
    }
}
