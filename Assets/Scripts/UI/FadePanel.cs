using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadePanel : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        if (fadeImage == null)
        {
            fadeImage = GetComponent<Image>();
        }
        FadeIn();
    }
    
    public void FadeIn()
    {
        StartCoroutine(FadeCoroutine(1,0, false));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCoroutine(0, 1, true));
    }
    
    // AI Generation
    private IEnumerator FadeCoroutine(float startAlpha, float endAlpha, bool flag)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        color.a = startAlpha;
        fadeImage.color = color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadeImage.raycastTarget = flag;
        fadeImage.color = color;
    }
}
