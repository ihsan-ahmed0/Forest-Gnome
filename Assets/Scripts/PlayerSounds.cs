using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    //public static bool isGroundedSounds =false;
    [SerializeField] AudioSource audioSJump;
    [SerializeField] AudioSource audioSLand;
    [SerializeField] AudioSource audioSStep;
    [SerializeField] AudioClip audioSJump2;
    [SerializeField] AudioClip audioSLand2;
    [SerializeField] AudioClip audioSStep2;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Play jumping sound.
    public void JumpSound()
    {
        audioSJump.clip = audioSJump2;
        audioSJump.Play();
    }

    // Play Walking sound.
    public void WalkSound()
    {
        audioSStep.clip = audioSStep2;
        audioSStep.Play();
    }

    public void StopWalkSound()
    {
        audioSStep.clip = audioSStep2;
        audioSStep.Stop();
    }

    // Play landing sound.
    public void LandSound()
    {
        audioSLand.clip = audioSLand2;
        audioSLand.Play();
    }
}
