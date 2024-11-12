using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using SpeechGenerationSystem;
using UnityText2Speech;

 
// using SpeechLib; // Old system namespace for SpVoice

public class ConversationManager : MonoBehaviour
{
    [Header("Config")]
    public float triggerDelay = 1f;
    public float animationTriggerChance = 0.5f; // Probability of triggering an animation (50%)

    [Header("Data")]
    public List<AudioClip> audioClips;
    public AudioClip finalClip;
    public Animator animator; // Animator for triggering animations

    [Header("Objects")]
    public AudioSource audioSource; // AudioSource for playing speech
    public ResponsePanel responsePanel;
    public Transform passport;

    // Old SpVoice text-to-speech system
    // SpVoice Attendant_voice; // Removed the old SpVoice object

    // New Speech System Fields
    public USgs speechGenerator; // Speech Generation System (replaces SpVoice)
    public Text inputText; // Text input to be converted to speech

    [Header("Controllers")]
    public List<XRController> controllers;

    bool triggerIsDown = false;
    int voiceLineTracker = -1;
    public bool beganConvo = false;

    private string[] conversationTriggers = { "Conversation1", "Conversation2", "Conversation3", "Conversation4" };



    void Start()
    {
        // Old SpVoice initialization (Removed)
        // Attendant_voice = new SpVoice();
        //inputText= new Text();
        
        // Initialize the Speech Generation System
        speechGenerator = GetComponent<USgs>(); // Get the USgs component for speech generation
        speechGenerator.audioPlayer = audioSource; // Assign the AudioSource for playing speech

        passport.localScale = Vector3.zero; // Set the initial scale of passport (for future interaction)
    }

    void Update()
    {
         //For means of testing
        if (Input.GetKeyDown(KeyCode.S))
        {
            
            // Define an array of three different sentences in Spanish
            string[] sentences = {
                "El día está soleado y hermoso.",  // Positive sentence
                "No me gusta cómo está lloviendo hoy.",  // Negative sentence
                "Hoy es un día como cualquier otro.",
                "Esta es una oración de ejemplo que utiliza el nuevo sistema de generación de voz."  // Neutral sentence
            };

            // Choose a random sentence from the array
            int randomIndex = Random.Range(0, sentences.Length);
            string selectedSentence = sentences[randomIndex];

            // Speak the selected sentence
            SpeakText(selectedSentence);

            // Classify the sentence as positive, negative, or neutral
            string classification = "";
            if (randomIndex == 0) {
                classification = "positive";
            } else if (randomIndex == 1) {
                classification = "negative";
            } else {
                classification = "neutral";
            }

            // Log the classification in the console
            Debug.Log("The selected sentence is: " + classification);
        
                SpeakText("Esta es una oración de ejemplo que utiliza el nuevo sistema de generación de voz.");
        }
    }

    // Method for triggering speech using the new Speech Generation System
    void SpeakText(string textToSpeak)
    {
        // Old system using SpVoice
        // Attendant_voice.Speak(textToSpeak, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
        // Randomly decide if an animation should be triggered

        if (Random.value < animationTriggerChance)
        {
            TriggerRandomAnimation();
        }

        speechGenerator.ReceiveTextToSpeech(textToSpeak); // Convert and play speech using the Speech Generation System
    }

    // Method to randomly trigger one of the four conversation animations
    void TriggerRandomAnimation()
    {
        // Select a random index for the animation triggers
        int randomIndex = Random.Range(0, conversationTriggers.Length);

        // Trigger the selected animation
        animator.SetTrigger(conversationTriggers[randomIndex]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !beganConvo)
        {
            beganConvo = true;
            BeginConversation();
        }
    }

    // Trigger the start of the conversation
    void BeginConversation()
    {
        passport.transform.localScale = Vector3.one; // Show passport object for interaction
        // Additional logic for starting the conversation...
    }
}
