using UnityEngine;

public class MoleSounds : MonoBehaviour
{
    [SerializeField] AudioSource audioHit;
    [SerializeField] AudioClip hitSound;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitSound()
    {
        audioHit.clip = hitSound;
        audioHit.Play();
    }
}
