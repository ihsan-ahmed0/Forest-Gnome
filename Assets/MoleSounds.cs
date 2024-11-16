using UnityEngine;

public class MoleSounds : MonoBehaviour
{
      public AudioSource audioHit;
        public AudioClip audioHitClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
      private void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag=="Player"&&PlayerSounds.isGroundedSounds==false){
            audioHit.clip =audioHitClip;
                    audioHit.Play();
           
        }
       
    }
    
}
