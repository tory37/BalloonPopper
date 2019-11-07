using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance = null;

    #region Serialized Fields
    [Header("Starting Values")]
    [SerializeField] private const int seedValue = 69;
    [SerializeField] private float pitchValue = 1.0f;
    
    //Might eventually check for repetitive random pitches
    //[SerializeField] private float previousPitch;

    [SerializeField] private AudioSource audioSource;
    [Header("Balloon Pop (not really)")]
    [SerializeField] private AudioClip audioClip;
    #endregion

     #region Initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } 
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();  
        audioSource.clip = audioClip;
        Random.InitState(seedValue);
    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        audioSource.pitch = pitchValue; 

        //test trigger for audio clip
        if (Input.GetKeyDown("return")){
            audioSource.Play();
            pitchValue = RandomPitch();
            Debug.Log(pitchValue);
        }
    }
    #endregion

    #region Random Pitch Shifter
    private  float RandomPitch(){
        float newPitch = Random.value;
        //scales 0-1 to .75-1.25
        newPitch = (((newPitch - 0.0f) * (1.25f - 0.75f)) / (1.0f - 0.0f)) + 0.75f;
        return newPitch;
    }
    #endregion
}
