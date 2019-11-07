using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance = null;

    #region Serialized Fields
    [Header("Starting Values")]
    [SerializeField] private const int seedValue = 69;
    
    //Might eventually check for repetitive random pitches
    //[SerializeField] private float previousPitch;

    [Header("Components")]
    [SerializeField] private AudioSource audioSource = null;

    [Header("Audio Clips")]
    [SerializeField] private SoundClipToAudioClip audioClips = new SoundClipToAudioClip();
    #endregion

    // This doesn't need to be serialized, but we will leave for testing purposes, to easily see the pitch each time in the inspector
    [SerializeField] private float pitchValue = 1.0f;

    #region Initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } 
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        instance.audioSource = GetComponent<AudioSource>();  
        Random.InitState(seedValue);
    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        //test trigger for audio clip
        if (Input.GetKeyDown("return")){
            PlayShiftedClip(SoundClip.BALLOON_POP);
        }
    }
    #endregion

    #region Random Pitch Shifter
    private  float getRandomPitch(){
        float newPitch = Random.value;
        //scales 0-1 to .75-1.25
        newPitch = (((newPitch - 0.0f) * (1.25f - 0.75f)) / (1.0f - 0.0f)) + 0.75f;
        return newPitch;
    }
    #endregion

    #region Triggers

    // This will be where you shift actually apply the shift
    public static void PlayShiftedClip(SoundClip clip)
    {
        instance.audioSource.clip = instance.audioClips[clip];
        instance.pitchValue = instance.getRandomPitch();
        instance.audioSource.pitch = instance.pitchValue;
        instance.audioSource.Play();
        Debug.Log("Pitch Value: " + instance.pitchValue);
    }

    public static void PlayUnshiftedClip(SoundClip clip)
    {
        instance.audioSource.clip = instance.audioClips[clip];
        instance.audioSource.pitch = 1.0f;
        instance.audioSource.Play();
    }

    
    #endregion
}
