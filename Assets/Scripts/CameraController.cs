using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public int height;

    // Update is called once per frame
    private void Update()
    {
        if (target != null)
        {
            gameObject.transform.position = target.transform.position + (Vector3.up * height);
        }
    }
}
