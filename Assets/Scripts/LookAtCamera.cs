using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum Mode { LookAt, LookAtInverted, CameraForward, CameraForwardInverted }
    [SerializeField] private Mode _mode;
    void LateUpdate()
    {
        switch (_mode)
        {
            case (Mode.LookAt):
                transform.LookAt(Camera.main.transform.position);
                break;
            case (Mode.LookAtInverted):
                transform.LookAt(transform.position - Camera.main.transform.position);
                break;
            case (Mode.CameraForward):
                transform.forward = Camera.main.transform.forward;
                break;
            case (Mode.CameraForwardInverted):
                transform.forward = -Camera.main.transform.forward;
                break;
            default:
                break;
        }
    }
}
