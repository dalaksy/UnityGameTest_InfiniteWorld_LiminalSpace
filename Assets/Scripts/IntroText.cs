using UnityEngine;
using TMPro;
using System.Collections;

public class IntroText : MonoBehaviour
{
    private TextMeshProUGUI textElement;
    public float displayTime = 3f; // Сколько секунд текст висит
    public float fadeDuration = 2f; // Сколько секунд исчезает

    void Start()
    {
        textElement = GetComponent<TextMeshProUGUI>();
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        // Просто ждем и гасим текст
        yield return new WaitForSeconds(displayTime);

        float currentTime = 0;
        Color startColor = textElement.color;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, currentTime / fadeDuration);
            textElement.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}