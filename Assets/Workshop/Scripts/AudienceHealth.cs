﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudienceHealth : MonoBehaviour {
    public int startingHealth = 100;                            // The amount of health the player starts the game with.
    public int currentHealth;                                   // The current health the player has.
    public int previousHealth;
    public AudioClip lowHealthClip;                             // The audio clip to play when the player has low health.
    //public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
    public AudioClip noHealthClip;                                 // The audio clip to play when the player dies.
    public float flashSpeed = 1f;                               // The speed the damageImage will fade at.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.

	public float volumeThreshold;									// The minimum volume user must speak at for object to hear.

    Animator anim;                                              // Reference to the Animator component.
    AudioSource audienceAudio;                                    // Reference to the AudioSource component.
    bool hasLeft;                                               // Whether the player has left.
    bool damaged;                                               // True when the player gets damaged.

    public Renderer rend;

    //speech timing
    public float timeBetweenAttacks = 3f;     // The time in seconds between each attack.
    public float timer;                                // Timer for counting up to the next attack.

    public float duration = 1.0F;


    // Use this for initialization
    void Start () {
		this.volumeThreshold = pythagoranTheorem (this.transform.position.x, 
			this.transform.position.y, 
			this.transform.position.z);
		// Debug.Log (volumeThreshold);
	}

    void Awake() //like the constructor
    {
        // Setting up the references.
        anim = GetComponent<Animator>();
        audienceAudio = GetComponent<AudioSource>();

        // Set the initial health of the player.
        currentHealth = startingHealth;
        previousHealth = currentHealth;
    }

    // Update is called once per frame
    void Update () {
        //update timer for audience attack
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;


		float loudness = MicInput.MicLoudness * 100;

		// Debug.Log("volume: " + loudness);

		// If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
        if (timer >= timeBetweenAttacks && currentHealth > 0)
        {
            // If the player has just been damaged...
            //Debug.Log("volume: " + MicInput.MicLoudness);

			//Debug.Log("current health: " + currentHealth);
			if (loudness < volumeThreshold)
            {
                // ... set the colour of the damageImage to the flash colour.
                //damageImage.color = flashColour;
                StartCoroutine(damageColor(true));
                TakeDamage(10);
            }
            else
            {
                TakeDamage(-10);
                StartCoroutine(damageColor(false));
            }
            // Otherwise... do nothing
            if (previousHealth > currentHealth)
            {
                if (currentHealth == 20)
                {
                    audienceAudio.clip = lowHealthClip;
                    audienceAudio.Play();
                }
            }
            // Reset the damaged flag.
            damaged = false;
        }
        else if (timer >= timeBetweenAttacks && currentHealth <= 0)
        {
            // Debug.Log("current health: " + currentHealth);
            // Debug.Log("loudness: " + loudness);
            if (loudness >= 25)
            {
                TakeDamage(-10);
            }
            else
            {
                TakeDamage(10);
            }
            damaged = false;
        }
        
    }

    public void setVisibility(bool visible)
    {
        // Gets GameComponent children and loops through all elements and sets those with Renderer to visible
        foreach (Transform child in transform)
        {
            if(child.GetComponent<Renderer>() != null)
                child.GetComponent<Renderer>().enabled = visible;
        }
    }

    IEnumerator damageColor(bool hit)
    {
        // Only flash red for first color component
        bool first = false;
        if (hit)
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<Renderer>() != null)
                {
                    Color OriginalColor = child.GetComponent<Renderer>().material.color;

                    for (int i = 0; i < 3; i++)
                    {
                        child.GetComponent<Renderer>().material.color = flashColour;
                        yield return new WaitForSeconds(.1f);
                        child.GetComponent<Renderer>().material.color = OriginalColor;
                        yield return new WaitForSeconds(.1f);
                    }
                    first = true;

                }
                if (first)
                    break;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        // Set the damaged flag so the screen will flash.
        damaged = true;

        previousHealth = currentHealth;

        // Reduce the current health by the damage amount.
        currentHealth -= amount;

        // Set the health bar's value to the current health.
        //healthSlider.value = currentHealth;

        // Play the hurt sound effect.
        //audienceAudio.Play();

        //reset the timer
        timer = 0f;

        // If the audience member has lost all it's health and the death flag hasn't been set yet...
        if (currentHealth <= 0 && !hasLeft)
        {
			// ... it should die. (figurtively lol)
            Death();
        }
        else if(currentHealth > 0 && hasLeft)
        {
            Alive();
        }
    }

    void Alive()
    {
        // Set the death flag to false
        hasLeft = false;

        Debug.Log("alive");

        // Change visibility to not hidden
        setVisibility(true);
    }

    void Death()
    {
        // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        audienceAudio.clip = noHealthClip;
        audienceAudio.Play();

        // Set the death flag so this function won't be called again.
        hasLeft = true;
        // Tell the animator that the player is dead.
        //anim.SetTrigger("Die");
        Debug.Log("death");

        // Change visiblility to hidden
        setVisibility(false);
    }

	float pythagoranTheorem(float x, float y, float z) {
		return Mathf.Sqrt(Mathf.Pow(x,2) + Mathf.Pow(y,2) + Mathf.Pow(z,2));
	}

}
