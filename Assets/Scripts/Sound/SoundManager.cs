using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioReferenceScriptableObject _audioReferencesSO;
    private float _volumeMultiplier = 1f;
    private void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        else
            Instance = this;
    }
    private void Start()
    {
        DeliveryManager.Instance.OnPlateDelivered += PlateDeliveredHandler;
        CuttingCounter.OnAnyCut += AnyCutHandler;
        PlayerKitchenObjectParent.OnAnyPlayerPickedUpObject += PickedUpObjectHandler;
        BaseCounter.OnAnyObjectPlaced += AnyObjectPlacedHandler;
        TrashCounter.OnAnyObjectTrashed += AnyObjectTrashedHandler;
    }

    private void AnyObjectTrashedHandler(object sender, EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(_audioReferencesSO.Trash, trashCounter.transform.position, 10f);
    }

    private void AnyObjectPlacedHandler(object sender, EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(_audioReferencesSO.ObjectDrop, baseCounter.transform.position);
    }

    private void PickedUpObjectHandler(object sender, EventArgs e)
    {
        PlayerKitchenObjectParent player = sender as PlayerKitchenObjectParent;
        PlaySound(_audioReferencesSO.ObjectPickup, player.transform.position);
    }

    private void AnyCutHandler(object sender, EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(_audioReferencesSO.Chop, cuttingCounter.transform.position);
    }

    private void PlateDeliveredHandler(object sender, OnPlateDeliveredEventArgs eventArgs)
    {
        if (eventArgs.Successful)
        {
            PlaySound(_audioReferencesSO.DeliverySuccess, DeliveryCounter.Instance.transform.position);
        }
        else
        {
            PlaySound(_audioReferencesSO.DeliveryFail, DeliveryCounter.Instance.transform.position);
        }
    }
    private void PlaySound(AudioClip[] audioClips, Vector3 position, float volume = 1f)
    {
        AudioClip clip = audioClips[UnityEngine.Random.Range(0, audioClips.Length)];
        PlaySound(clip, position, volume * _volumeMultiplier);
    }
    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume * _volumeMultiplier);
    }
    public void PlayFootstepSound(Vector3 position)
    {
        PlaySound(_audioReferencesSO.Footstep, position);
    }
    //Getters-Setters
    public void SetMasterVolume(float volume)
    {
        _volumeMultiplier = volume;
    }
    //cleanup
    private void OnDestroy()
    {
        DeliveryManager.Instance.OnPlateDelivered -= PlateDeliveredHandler;
        CuttingCounter.OnAnyCut -= AnyCutHandler;
        PlayerKitchenObjectParent.OnAnyPlayerPickedUpObject -= PickedUpObjectHandler;
        BaseCounter.OnAnyObjectPlaced -= AnyObjectPlacedHandler;
        TrashCounter.OnAnyObjectTrashed -= AnyObjectTrashedHandler;
    }
}
