using UnityEngine;
using UnityEngine.Audio;

public class MicrophoneInput : MonoBehaviour
{
    public string microphoneName = null; // Name of the microphone device (null for default)
    public bool loopMicrophone = true; // Set whether to loop the microphone input
    public AudioMixerGroup mixerGroup; // Assign an audio mixer group for the microphone input

    private AudioSource audioSource;

    void Start()
    {
        // Create an AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixerGroup;

        // Get the microphone device name
        string deviceName = microphoneName != null ? microphoneName : Microphone.devices[0];

        // Start recording from the microphone
        audioSource.clip = Microphone.Start(deviceName, loopMicrophone, 1, AudioSettings.outputSampleRate);
        audioSource.loop = loopMicrophone;

        // Wait until the microphone starts recording
        while (!(Microphone.GetPosition(deviceName) > 0))
        {
        }

        // Play the audio from the microphone
        audioSource.Play();
    }
}