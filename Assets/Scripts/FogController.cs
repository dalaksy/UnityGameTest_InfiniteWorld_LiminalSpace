using UnityEngine;

public class FogController : MonoBehaviour
{
    void Start()
    {
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogStartDistance = 10f;
        RenderSettings.fogEndDistance = 150f;
        RenderSettings.fogColor = new Color(0.05f, 0.05f, 0.2f);

        RenderSettings.ambientLight = new Color(0.05f, 0.05f, 0.1f);

        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Camera.main.backgroundColor = new Color(0.05f, 0.05f, 0.2f);
    }
}