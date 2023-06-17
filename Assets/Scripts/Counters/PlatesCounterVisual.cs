using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private Transform _counterTopPoint;
    [SerializeField] private Transform _platesVisualPrefab;
    private PlatesCounter _platesCounter;
    private List<Transform> _plateVisuals = new List<Transform>();
    private const float PLATE_Y_OFFSET = 0.1f;
    private void Awake()
    {
        _platesCounter = GetComponent<PlatesCounter>();
    }
    private void Start()
    {
        _platesCounter.OnPlateSpawned += PlatesSpawnedHandler;
    }
    private void PlatesSpawnedHandler(object sender, OnPlateSpawnedEventArgs eventArgs)
    {
        if (eventArgs.Change == OnPlateSpawnedEventArgs.CountChangeType.Increase)
        {
            Transform plateVisualTransform = Instantiate<Transform>(_platesVisualPrefab, _counterTopPoint);
            plateVisualTransform.localPosition = new Vector3(0f, PLATE_Y_OFFSET * _plateVisuals.Count, 0f);
            _plateVisuals.Add(plateVisualTransform);
        }
        else
        {
            Transform plateTransform = _plateVisuals[_plateVisuals.Count - 1];
            _plateVisuals.Remove(plateTransform);
            Destroy(plateTransform.gameObject);
        }
    }
}

