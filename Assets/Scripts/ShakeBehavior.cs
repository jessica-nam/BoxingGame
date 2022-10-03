using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeBehavior : MonoBehaviour
{
    public static ShakeBehavior instance;
    // Transform of the GameObject you want to shake
    private Transform transform;

    // Desired duration of the shake effect
    private float shakeDuration = 0f;

    // A measure of magnitude for the shake. Tweak based on your preference
    private float shakeMagnitude = 0.05f;

    // A measure of how quickly the shake effect should evaporate
    private float dampingSpeed = 0.3f;

    Vector3 initialPosition;

    private void Awake()
    {
        instance = this;
        if (transform == null)
        {
            transform = GetComponent(typeof(Transform)) as Transform;
        }
        
    }

    

    void OnEnable()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = initialPosition;
        }
    }
    public void TriggerShake()
    {
        shakeDuration = 0.5f;
    }
}
