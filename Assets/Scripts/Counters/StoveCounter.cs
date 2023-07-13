using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public enum State { Idle, Frying, Burned }
    [SerializeField] private FryingRecipeScriptableObject[] _fryingRecipeSOs;
    private State _state;
    private NetworkVariable<float> _fryingTimer = new NetworkVariable<float>(0f);
    private bool _outputCanBeBurned = false;
    private FryingRecipeScriptableObject _fryingRecipeSO;
    private static Dictionary<KitchenObjectScriptableObject, FryingRecipeScriptableObject> _fryingRecipeDict;
    //events
    public event EventHandler<OnStoveStateChangedEventArgs> OnStoveStateChanged;
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public override void Interact(Player player)
    {
        if (!HasKitchenObject()) //table is empty
        {
            if (player.HasKitchenObject())
            {
                //cache KO
                KitchenObject kitchenObject = player.GetKitchenObject();
                //player has a KO and table is empty, transfer player item to the table
                kitchenObject.SetKitchenObjectParent(this);
                //if KO can be fried, change state to frying and reset timer
                if (_fryingRecipeDict.ContainsKey(kitchenObject.GetKitchenObjectSO()))
                {
                    int kitchenObjectIndex = CookingGameMultiplayer.Instance.GetIndexFromKitchenObjectSO(kitchenObject.GetKitchenObjectSO());
                    OnObjectPlacedOnStoveServerRpc(kitchenObjectIndex);
                }
            }
            else
            {
                //player hasn't got anything
            }
        }
        else //there is a KO on table
        {
            //player has a KO
            if (player.HasKitchenObject())
            {
                //KO is a plate
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        CookingGameMultiplayer.Instance.DestroyKitchenObject(GetKitchenObject());
                    }
                }
            }
            else
            {
                //player hasn't got anything
                GetKitchenObject().SetKitchenObjectParent(player);
                ResetStateToIdleServerRpc();
            }
        }
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _fryingTimer.OnValueChanged += FryingTimerValueChangedHandler;
    }
    private void FryingTimerValueChangedHandler(float previousValue, float newValue)
    {
        if (_fryingRecipeSO != null)
        {
            //fire event
            FireOnProgressChangedEvent(_fryingTimer.Value / _fryingRecipeSO.FryingTimerMax);
        }
        else
        {
            FireOnProgressChangedEvent(0f);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void OnObjectPlacedOnStoveServerRpc(int kitchenObjectIndex)
    {
        OnObjectPlacedOnStoveClientRpc(kitchenObjectIndex);
    }
    [ClientRpc]
    private void OnObjectPlacedOnStoveClientRpc(int kitchenObjectIndex)
    {
        _state = State.Frying;
        _fryingTimer.Value = 0f;
        KitchenObjectScriptableObject kitchenObjectScriptableObject = CookingGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectIndex);
        _fryingRecipeSO = _fryingRecipeDict[kitchenObjectScriptableObject];

        OnStoveStateChanged?.Invoke(this, new OnStoveStateChangedEventArgs { State = _state });
    }
    [ServerRpc]
    private void ObjectBurnedServerRpc()
    {
        ObjectBurnedClientRpc();
    }
    [ClientRpc]
    private void ObjectBurnedClientRpc()
    {
        _state = State.Burned;
        OnStoveStateChanged?.Invoke(this, new OnStoveStateChangedEventArgs { State = _state });
        FireOnProgressChangedEvent(0f);
    }
    [ServerRpc(RequireOwnership = false)]
    private void ResetStateToIdleServerRpc()
    {
        //reset state to idle
        ResetStateToIdleClientRpc();
        _fryingTimer.Value = 0f;
    }
    [ClientRpc]
    private void ResetStateToIdleClientRpc()
    {
        _state = State.Idle;
        //reset state to idle
        OnStoveStateChanged?.Invoke(this, new OnStoveStateChangedEventArgs { State = _state });
        FireOnProgressChangedEvent(0f);
        _outputCanBeBurned = false;
    }
    private void Awake()
    {
        InitializeFryingRecipeDict();
    }
    private void Start()
    {
        _state = State.Idle;
    }
    private void Update()
    {
        //execute update only on server
        if (!IsServer) return;

        if (HasKitchenObject() && _fryingRecipeDict.ContainsKey(GetKitchenObject().GetKitchenObjectSO()))
        {
            FryingRecipeScriptableObject fryingRecipe = _fryingRecipeDict[GetKitchenObject().GetKitchenObjectSO()];
            _outputCanBeBurned = fryingRecipe.OutputIsBurnedObject;
            switch (_state)
            {
                case (State.Idle):
                    //break out of the state machine if state is idle
                    break;
                case (State.Frying):
                    _fryingTimer.Value += Time.deltaTime;

                    if (_fryingTimer.Value >= fryingRecipe.FryingTimerMax)
                    {
                        CookingGameMultiplayer.Instance.DestroyKitchenObject(GetKitchenObject());
                        CookingGameMultiplayer.Instance.SpawnKitchenObject(fryingRecipe.Output, this);
                        _fryingTimer.Value = 0f;
                        if (fryingRecipe.OutputIsBurnedObject)
                        {
                            ObjectBurnedServerRpc();
                        }
                    }
                    break;
                case (State.Burned):
                    //TODO
                    break;
            }
        }
    }
    //Helper methods
    private void InitializeFryingRecipeDict()
    {
        if (_fryingRecipeDict == null)
        {
            //only initialize if has not been initialized yet
            _fryingRecipeDict = new Dictionary<KitchenObjectScriptableObject, FryingRecipeScriptableObject>();
            foreach (FryingRecipeScriptableObject frso in _fryingRecipeSOs)
            {
                _fryingRecipeDict[frso.Input] = frso;
            }
        }
    }
    private void FireOnProgressChangedEvent(float normalizedProgress)
    {
        OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
        {
            ProgressNormalized = normalizedProgress
        });
    }

    //Getters-Setters
    public bool OutputCanBeBurned()
    {
        return _outputCanBeBurned;
    }
}

public class OnStoveStateChangedEventArgs : EventArgs
{
    public StoveCounter.State State;
}
