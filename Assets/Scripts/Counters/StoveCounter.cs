using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public enum State { Idle, Frying, Burned }
    [SerializeField] private FryingRecipeScriptableObject[] _fryingRecipeSOs;
    private State _state;
    float _fryingTimer = 0f;
    private bool _outputCanBeBurned = false;
    private static Dictionary<KitchenObjectScriptableObject, FryingRecipeScriptableObject> _fryingRecipeDict;
    public event EventHandler<OnStoveStateChangedEventArgs> OnStoveStateChanged;
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;

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
                    _fryingTimer += Time.deltaTime;
                    //fire event
                    FireOnProgressChangedEvent(_fryingTimer / fryingRecipe.FryingTimerMax);

                    if (_fryingTimer >= fryingRecipe.FryingTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        CookingGameMultiplayer.Instance.SpawnKitchenObject(fryingRecipe.Output, this);
                        _fryingTimer = 0f;
                        if (fryingRecipe.OutputIsBurnedObject)
                        {
                            _state = State.Burned;
                            OnStoveStateChanged?.Invoke(this, new OnStoveStateChangedEventArgs { State = _state });
                            FireOnProgressChangedEvent(0f);
                        }
                    }
                    break;
                case (State.Burned):
                    //TODO
                    break;
            }
        }
    }
    public override void Interact(Player player)
    {
        if (!HasKitchenObject()) //table is empty
        {
            if (player.HasKitchenObject())
            {
                //player has a KO and table is empty, transfer player item to the table
                player.GetKitchenObject().SetKitchenObjectParent(this);
                //if KO can be fried, change state to frying and reset timer
                if (_fryingRecipeDict.ContainsKey(GetKitchenObject().GetKitchenObjectSO()))
                {
                    _state = State.Frying;
                    _fryingTimer = 0f;
                    OnStoveStateChanged?.Invoke(this, new OnStoveStateChangedEventArgs { State = _state });
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
                        GetKitchenObject().DestroySelf();
                        ResetStateToIdle();
                    }
                }
            }
            else
            {
                //player hasn't got anything
                GetKitchenObject().SetKitchenObjectParent(player);
                ResetStateToIdle();
            }
        }
    }



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
    private void ResetStateToIdle()
    {
        //reset state to idle
        _state = State.Idle;
        _fryingTimer = 0f;
        OnStoveStateChanged?.Invoke(this, new OnStoveStateChangedEventArgs { State = _state });
        FireOnProgressChangedEvent(0f);
        _outputCanBeBurned = false;
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
