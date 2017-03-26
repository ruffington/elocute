using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    AudienceHealth audHealth;
    Vector3 randomDirection;
    // Use this for initialization
    void Start () {
        audHealth = this.GetComponent<AudienceHealth>();
        randomDirection = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(0, 1.0f));

    }

    // Update is called once per frame
    void Update () {
        if (audHealth.currentHealth <= 50 && audHealth.currentHealth > 0 && (MicInput.MicLoudness * 100) < audHealth.volumeThreshold)
        {
            Debug.Log("current health: " + audHealth.currentHealth);
            transform.Translate(randomDirection * Time.deltaTime, Camera.main.transform);
        }
        else if (audHealth.currentHealth <= 50 && audHealth.currentHealth > 0 && (MicInput.MicLoudness * 100) >= audHealth.volumeThreshold)
        {
            if (MicInput.MicLoudness * 100 >= 25)
            {
                transform.Translate(-randomDirection * Time.deltaTime, Camera.main.transform);
            }
        }
    }
}
