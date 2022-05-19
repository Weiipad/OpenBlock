using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class VisionController : MonoBehaviour
{
    [SerializeField]
    private Camera controlCamera;
    [SerializeField]
    private float sensitivity;

    private Vector3 yawPitch;
    private Vector3 movementDelta;

    public System.Func<Vector2> GetMovementDelta;

    void Update()
    {
        movementDelta = GetMovementDelta();
        yawPitch += sensitivity * new Vector3(-movementDelta.y, movementDelta.x);

        yawPitch.x = Mathf.Clamp(yawPitch.x, -89, 89);


        controlCamera.transform.localRotation = Quaternion.Euler(yawPitch);
    }
}
