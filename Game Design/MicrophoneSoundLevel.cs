using UnityEngine;
using UnityEngine.Audio;
using System;

public class MicrophoneSoundLevel : MonoBehaviour
{
    public float sensitivity = 100f; // Adjust this value to change the sensitivity
    public float threshold = 0.5f; // Adjust this value to change the threshold level
    public AudioMixerGroup mixerGroup; // Assign an audio mixer group to capture the microphone input
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private int currentIndex;
    private float timer;
    
    public FlowerScript flowerScript; // Reference to the FlowerScript

    private AudioSource audioSource;

    public event Action<float> SoundLevelReachedThreshold; // Event to trigger when the sound level exceeds the threshold

    void Start()
    {
        // Create an AudioSource and assign it to the microphone input
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixerGroup;
        audioSource.clip = Microphone.Start(null, true, 1, AudioSettings.outputSampleRate);
        audioSource.loop = true;
        while (!(Microphone.GetPosition(null) > 0)) { } // Wait until the microphone starts recording
        audioSource.Play();

        // Initialize the sprite renderer and current index
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentIndex = 0;

        // Check if there are sprites assigned
        if (sprites.Length == 0)
        {
            Debug.LogError("No sprites assigned to the MicrophoneSoundLevel script.");
            enabled = false; // Disable the script
        }
    }

    void Update()
    {
        // Read the sound level from the microphone input
        float[] samples = new float[256];
        audioSource.GetOutputData(samples, 0);

        // Calculate the RMS value of the samples
        float sum = 0f;
        for (int i = 0; i < samples.Length; i++)
        {
            sum += samples[i] * samples[i];
        }
        float rmsValue = Mathf.Sqrt(sum / samples.Length);

        // Check if the sound level exceeds the threshold
        if (rmsValue > threshold)
        {
            // Trigger the event
            SoundLevelReachedThreshold?.Invoke(rmsValue);
            flowerScript.StartAnimation(); // Call the StartAnimation() method of the FlowerScript
        }
        else
        {
            flowerScript.StopAnimation(); // Call the StopAnimation() method of the FlowerScript
        }
    }

    void AnimateSprite()
    {
        // Increment the timer
        timer += Time.deltaTime;

        // Check if it's time to change the sprite
        if (timer >= 0.1f)
        {
            // Increment the index and loop back to the beginning if necessary
            currentIndex = (currentIndex + 1) % sprites.Length;

            // Change the sprite
            spriteRenderer.sprite = sprites[currentIndex];

            // Reset the timer
            timer = 0f;
        }
    }
}
