using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [Header("Camera Offets")]
    [SerializeField] float horizontalCameraOffset;
    [SerializeField] float verticalCameraOffset;

    [SerializeField] GameObject healthText;
    private TMP_Text text;

    private GameObject camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        text = healthText.GetComponent<TMP_Text>();
        text.text = "Health: 100";
    }

    // Update is called once per frame
    void Update()
    {
        FollowCamera();
    }

    // Follow the position of the camera.
    private void FollowCamera()
    {
        Vector3 cameraPosition = camera.transform.position;
        transform.position = new Vector3(cameraPosition.x + horizontalCameraOffset, cameraPosition.y + verticalCameraOffset, -5);
    }

    // Change the health text whenever the funciton is called.
    public void ChangeHealthText(int newHealth)
    {
        if (newHealth < 0)
        {
            newHealth = 0;
        }
        text.text = "Health: " + newHealth.ToString();
    }
}
