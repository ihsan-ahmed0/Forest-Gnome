using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
   public static bool isGroundedSounds =false;
    public AudioSource audioSJump;
      public AudioSource audioSLand;
        public AudioSource audioSStep;
        public AudioClip audioSJump2;
      public AudioClip audioSLand2;
        public AudioClip audioSStep2;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space")&&isGroundedSounds){
          
            audioSJump.clip=audioSJump2;
            audioSJump.Play();
             
        }
       
       
    }
     private void OnCollisionStay2D(Collision2D col){
         if(col.gameObject.tag=="Ground"){
            if((Input.GetKeyDown("left")||Input.GetKey("right")||Input.GetKey("a")||Input.GetKey("d"))){
                audioSStep.clip =audioSStep2;
                    audioSStep.Play();
                   
                }
         }
     }
    private void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag=="Ground"){
              audioSLand.clip =audioSLand2;
                    audioSLand.Play();
           
        }
        isGroundedSounds=true;
    }
    private void OnCollisionExit2D(Collision2D col){
        if(col.gameObject.tag=="Ground"){
           
            isGroundedSounds=false;
        }
    }
}
