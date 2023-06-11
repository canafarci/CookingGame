using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    private readonly static int _openCloseHash = Animator.StringToHash("OpenClose");
    private Animator _animator;
    private ContainerCounter _containerCounter;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _containerCounter = GetComponent<ContainerCounter>();
    }

    private void Start()
    {
        _containerCounter.OnPlayerGrabbedObject += PlayerGrabbedObjectHandler;
    }

    private void PlayerGrabbedObjectHandler(object sender, EventArgs e)
    {
        _animator.SetTrigger(_openCloseHash);
    }
}
