using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public AudioClip collectedClip;
    public float healing = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        goboController controller = other.GetComponent<goboController>();

        if (controller != null)
        {
            if (controller.currentHealth < controller.maxHealth)
            {
                controller.UpdateHealth(healing);
            }
        }

    }
}