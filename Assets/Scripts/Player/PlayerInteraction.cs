using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerInteraction : NetworkBehaviour
{
    [SerializeField] LayerMask _countersLayerMask;
    private BaseCounter _selectedCounter;
    private IKitchenObjectParent _player;
    //Singleton
    public static PlayerInteraction LocalInstance { get; private set; }
    //Constants
    private const float PLAYER_RADIUS = 0.7f;
    private const float PLAYER_HEIGHT = 2f;
    private const float INTERACT_DISTANCE = 2f;
    //Events
    public static event EventHandler OnAnyPlayerInteracterSpawned;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    private void Awake()
    {
        _player = GetComponent<IKitchenObjectParent>();
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

        OnAnyPlayerInteracterSpawned?.Invoke(this, EventArgs.Empty);
    }
    private void Update()
    {
        //pop call stack frame if client is not the owner of the object
        if (!IsOwner) return;

        HandleInteractions();
    }
    private void HandleInteractions()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, INTERACT_DISTANCE, _countersLayerMask))
        {
            if (hit.transform.TryGetComponent<BaseCounter>(out BaseCounter baseCounter))
            {
                if (baseCounter != _selectedCounter)
                    ChangeSelectedCounter(baseCounter);
            }
            else
                SetSelectedCounterNull();
        }
        else
            SetSelectedCounterNull();
    }
    private void SetSelectedCounterNull()
    {
        if (_selectedCounter == null) return;
        _selectedCounter = null;
        ChangeSelectedCounter(_selectedCounter);
    }
    private void ChangeSelectedCounter(BaseCounter counter)
    {
        _selectedCounter = counter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { SelectedCounter = _selectedCounter });
    }

    private void InteractHandler(object sender, EventArgs e)
    {
        //pop call stack frame if game is not playing
        if (!GameManager.Instance.IsGamePlaying()) return;
        _selectedCounter?.Interact(_player);
    }
    private void InteractAlternateHandler(object sender, EventArgs e)
    {

        //pop call stack frame if game is not playing
        if (!GameManager.Instance.IsGamePlaying()) return;
        _selectedCounter?.InteractAlternate(_player);
    }

}
public class OnSelectedCounterChangedEventArgs : EventArgs
{
    public BaseCounter SelectedCounter;
}