using UnityEngine;

public class ObjectAppearOnCollision : MonoBehaviour
{
    public GameObject objectToToggle;

    private void Start()
    {
        if (objectToToggle != null)
        {
            objectToToggle.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Player")){
            objectToToggle.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        if (collision.CompareTag("Player")){
            objectToToggle.SetActive(false);
        }
    }
}
