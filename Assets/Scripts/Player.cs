using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    //Private
    [SerializeField] private float _moveSpeed, _rotateSpeed;
    [SerializeField] Transform _kitchenObjectHoldPoint;
    [SerializeField] LayerMask _countersLayerMask;
    private BaseCounter _selectedCounter;
    private GameInput _gameInput;
    private bool _isWalking;
    private KitchenObject _kitchenObject;

    //Constants
    private const float PLAYER_RADIUS = 0.7f;
    private const float PLAYER_HEIGHT = 2f;
    private const float INTERACT_DISTANCE = 2f;
    //Properties
    public bool IsWalking { get { return _isWalking; } }
    //Singleton
    public static Player Instance { get; private set; }
    //EVENTS
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public event Action OnPickedUpObject;
    private void Awake()
    {
        //Initialize singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Player instance is not null!");
            Destroy(gameObject);
        }

        _gameInput = FindObjectOfType<GameInput>();
    }

    private void OnEnable()
    {
        _gameInput.OnInteractAction += InteractHandler;
        _gameInput.OnInteractAlternateAction += InteractAlternateHandler;
    }
    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }
    private void InteractHandler(object sender, EventArgs e) => _selectedCounter?.Interact(this);
    private void InteractAlternateHandler(object sender, EventArgs e) => _selectedCounter?.InteractAlternate(this);
    private void HandleInteractions()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, INTERACT_DISTANCE, _countersLayerMask))
        {
            if (hit.transform.TryGetComponent<BaseCounter>(out BaseCounter baseCounter))
            {
                if (baseCounter != _selectedCounter)
                    SelectedCounterChanged(baseCounter);
            }
            else
                SetSelectedCounterNull();
        }
        else
            SetSelectedCounterNull();
    }
    private void HandleMovement()
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
            canMove = moveDir.x != 0f && CanMove(moveDirX, moveDistance);
            if (canMove)
            {
                //update vector
                moveDir = moveDirX;
            }
            else
            {
                //if cant move on X, try Z
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                canMove = canMove = moveDir.z != 0f && CanMove(moveDirZ, moveDistance);
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
    private bool CanMove(Vector3 moveDir, float moveDistance)
    {
        return !Physics.CapsuleCast(transform.position,
                                 transform.position + Vector3.up * PLAYER_HEIGHT,
                                PLAYER_RADIUS,
                                moveDir,
                                moveDistance);
    }
    private void SetSelectedCounterNull()
    {
        if (_selectedCounter == null) return;
        _selectedCounter = null;
        SelectedCounterChanged(_selectedCounter);
    }
    private void SelectedCounterChanged(BaseCounter counter)
    {
        _selectedCounter = counter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { SelectedCounter = _selectedCounter });
    }
    //Kitchen object parent interface contract
    public void ClearKitchenObject() => _kitchenObject = null;
    public KitchenObject GetKitchenObject() => _kitchenObject;
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;

        if (kitchenObject != null)
            OnPickedUpObject?.Invoke();
    }
    public bool HasKitchenObject() => _kitchenObject != null;
    public Transform GetKitchenObjectFollowTransform() => _kitchenObjectHoldPoint;
}
public class OnSelectedCounterChangedEventArgs : EventArgs
{
    public BaseCounter SelectedCounter;
}