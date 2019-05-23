using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectUI : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        var position = transform.position + _camera.transform.rotation * Vector3.forward;
        var up = _camera.transform.rotation * Vector3.up;
        transform.LookAt(position, up);
    }
}
