using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeImageInCanvas : MonoBehaviour
{
    public float fadeDuration = 2f;
    public Image imageComponent;

    private void Start()
    {
        imageComponent = GetComponent<Image>();

        StartFadeToBlack();
    }

    public void StartFadeToBlack(){
        if (imageComponent != null){
            StartCoroutine(FadeToColor(Color.black, fadeDuration));
        }
    }

    private IEnumerator FadeToColor(Color targetColor, float duration){
        Color startColor = imageComponent.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration){
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            imageComponent.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        imageComponent.color = targetColor;
    }
}
