using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] int SensitivityVertical = 3;
    [SerializeField] int SensitivityHorizontal = 3;

    [SerializeField] int LockVerticalMin;
    [SerializeField] int LockVerticalMax;

    [SerializeField] bool InvertY = false;

    float XRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        // get input
        float mouseX = Input.GetAxis("Mouse X") * SensitivityHorizontal * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * SensitivityVertical * Time.deltaTime;
        
        // rotate (with inversion?)
        XRotation += InvertY ? mouseY : -mouseY;

        // clamp XRotation
        XRotation = Mathf.Clamp(XRotation, LockVerticalMin, LockVerticalMax);

        // rotate on x axis
        transform.localRotation = Quaternion.Euler(XRotation, 0, 0);

        // rotate the transform
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
