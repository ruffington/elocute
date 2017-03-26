using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudienceHealth : MonoBehaviour {
    public int startingHealth = 100;                            // The amount of health the player starts the game with.
    public int currentHealth;                                   // The current health the player has.
    public AudioClip lowHealthClip;                             // The audio clip to play when the player has low health.
    //public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
    public AudioClip deathClip;                                 // The audio clip to play when the player dies.
    public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.

    Animator anim;                                              // Reference to the Animator component.
    AudioSource audienceAudio;                                    // Reference to the AudioSource component.
    bool hasLeft;                                               // Whether the player has left.
    bool damaged;                                               // True when the player gets damaged.

    public Renderer rend;

    //speech timing
    public float timeBetweenAttacks = 0.5f;     // The time in seconds between each attack.
    float timer;                                // Timer for counting up to the next attack.

    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
    }

    void Awake() //like the constructor
    {
        // Setting up the references.
        anim = GetComponent<Animator>();
        audienceAudio = GetComponent<AudioSource>();

        // Set the initial health of the player.
        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update () {
        //update timer for audience attack
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
        if (timer >= timeBetweenAttacks && currentHealth > 0)
        {
            // If the player has just been damaged...
            Debug.Log("volume: " + MicInput.MicLoudness);
            Debug.Log("current health: " + currentHealth);
            if (MicInput.MicLoudness < 10)
            {
                // ... set the colour of the damageImage to the flash colour.
                //damageImage.color = flashColour;
                TakeDamage(10);

            }
            // Otherwise...
            else
            {
                // ... transition the colour back to clear.
                //damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
                
            }

            // Reset the damaged flag.
            damaged = false;
        }
        
    }

    public void TakeDamage(int amount)
    {
        // Set the damaged flag so the screen will flash.
        damaged = true;
        

        // Reduce the current health by the damage amount.
        currentHealth -= amount;

        // Set the health bar's value to the current health.
        //healthSlider.value = currentHealth;

        // Play the hurt sound effect.
        //audienceAudio.Play();

        //reset the timer
        timer = 0f;

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (currentHealth <= 0 && !hasLeft)
        {
            // ... it should die.
            Death();
        }
    }

    void Death()
    {
        // Set the death flag so this function won't be called again.
        hasLeft = true;

        // Tell the animator that the player is dead.
        //anim.SetTrigger("Die");
        Debug.Log("death");

        // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        //audienceAudio.clip = deathClip;
        //audienceAudio.Play();

        // Change visiblility to hidden
        //anim.enabled = false;
        this.rend.enabled = false;
    }

}
