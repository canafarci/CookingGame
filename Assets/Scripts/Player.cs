using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour, IKitchenObjectParent
{
    //Private
    [SerializeField] private float _moveSpeed, _rotateSpeed;
    [SerializeField] Transform _kitchenObjectHoldPoint;
    [SerializeField] LayerMask _countersLayerMask;
    private BaseCounter _selectedCounter;
    private bool _isWalking;
    private KitchenObject _kitchenObject;
    private CharacterController _characterController;

    //Constants
    private const float PLAYER_RADIUS = 0.7f;
    private const float PLAYER_HEIGHT = 2f;
    private const float INTERACT_DISTANCE = 2f;
    //Properties
    public bool IsWalking { get { return _isWalking; } }
    //Singleton
    public static Player LocalInstance { get; private set; }
    //EVENTS
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public static event EventHandler OnAnyPlayerSpawned;
    public static event EventHandler OnAnyPlayerPickedUpObject;
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        GameInput.Instance.OnInteractAction += InteractHandler;
        GameInput.Instance.OnInteractAlternateAction += InteractAlternateHandler;
    }
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            LocalInstance = this;
        }

        OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        //pop call stack frame if client is not the owner of the object
        if (!IsOwner) return;

        HandleMovement();
        HandleInteractions();
    }
    private void InteractHandler(object sender, EventArgs e)
    {
        //pop call stack frame if game is not playing
        if (!GameManager.Instance.IsGamePlaying()) return;
        _selectedCounter?.Interact(this);
    }
    private void InteractAlternateHandler(object sender, EventArgs e)
    {

        //pop call stack frame if game is not playing
        if (!GameManager.Instance.IsGamePlaying()) return;
        _selectedCounter?.InteractAlternate(this);
    }
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
            OnAnyPlayerPickedUpObject?.Invoke(this, EventArgs.Empty);
    }
    public bool HasKitchenObject() => _kitchenObject != null;
    public Transform GetKitchenObjectFollowTransform() => _kitchenObjectHoldPoint;

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
}
public class OnSelectedCounterChangedEventArgs : EventArgs
{
    public BaseCounter SelectedCounter;
}