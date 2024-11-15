using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    public static bool BossDefeated=false;
    public float retreatVal=0;
    public Animator anim;
    public bool  knockBackDone  =false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
      private void OnCollisionEnter2D(Collision2D col){
         if( col.gameObject.name=="Player"&&retreatVal==3&& knockBackDone ){
                 anim.SetBool("isFaint",true);
                 BossDefeated=true;
            }
            if(col.gameObject.name=="Player"&&retreatVal==3){
                  anim.SetBool("isRetreat",true);
                col.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(100,0,0));
                knockBackDone = true;
            }
          if(col.gameObject.name=="Player"&&retreatVal<3){
                retreatVal+= 1;
            }
          
      }
}
