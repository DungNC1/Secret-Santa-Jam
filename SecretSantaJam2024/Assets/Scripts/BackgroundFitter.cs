using UnityEngine;
using UnityEngine.UI;

public class BackgroundFitter : MonoBehaviour
{
    private RectTransform rectTransform;
    private RawImage rawImage;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rawImage = GetComponent<RawImage>();
        FitToScreen();
    }

    private void FitToScreen()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float imageAspect = (float)rawImage.texture.width / (float)rawImage.texture.height;

        if (screenAspect >= imageAspect)
        {
            rectTransform.sizeDelta = new Vector2(Screen.height * imageAspect, Screen.height);
        }
        else
        {
            rectTransform.sizeDelta = new Vector2(Screen.width, Screen.width / imageAspect);
        }
    }
}
