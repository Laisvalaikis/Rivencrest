using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageFadeController : MonoBehaviour
{
    public float fadeInDuration = 0.2f;
    public float fadeOutDuration = 0.2f;

    private Image targetImage;
    private Coroutine fadeCoroutine;
    private float currentAlpha;

    private void Start()
    {
    }

    private void OnEnable()
    {
        targetImage = GetComponent<Image>();
        currentAlpha = targetImage.color.a;
        FadeInStart();
    }

    private void OnDisable()
    {
        Color fixedColor = targetImage.color;
        fixedColor.a = 1;
        targetImage.color = fixedColor;
        Debug.Log("Disabled");
    }

    public void FadeInStart()
    {
        targetImage.CrossFadeAlpha(0f, 0, true);
    }

    public void FadeIn()
    {
        if (targetImage == null) return;
        targetImage.CrossFadeAlpha(0.5f, fadeInDuration, true);
    }

    public void FadeOut()
    {
        if (targetImage == null) return;
        targetImage.CrossFadeAlpha(0f, fadeOutDuration, true);
    }

    private IEnumerator FadeImage(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color currentColor = targetImage.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            currentAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            currentColor.a = currentAlpha;
            targetImage.color = currentColor;
            yield return null;
        }

        currentColor.a = endAlpha;
        targetImage.color = currentColor;
        currentAlpha = endAlpha;
    }
}
