using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotate : MonoBehaviour
{
    public float spinSpeed = 90.0f;

    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // Rotate the image by the spin speed per second
        rect.Rotate(Vector3.forward, spinSpeed * Time.deltaTime);
    }
}
