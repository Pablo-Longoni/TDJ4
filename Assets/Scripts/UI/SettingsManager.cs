using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
public class SettingsManager : MonoBehaviour
{
    [Header("UI Sliders")]
    public Slider zoomSpeedSlider;
    public Slider rotationSpeedSlider;

    [Header("Valores por defecto")]
    public float defaultZoomSpeed = 5f;
    public float defaultRotationSpeed = 1000f;

    private void Start()
    {
        // Cargar valores guardados (o usar default si no existen)
        float zoomSpeed = PlayerPrefs.GetFloat("ZoomSpeed", defaultZoomSpeed);
        float rotationSpeed = PlayerPrefs.GetFloat("RotationSpeed", defaultRotationSpeed);

        // Setear sliders
        if (zoomSpeedSlider != null) zoomSpeedSlider.value = zoomSpeed;
        if (rotationSpeedSlider != null) rotationSpeedSlider.value = rotationSpeed;

        // Aplicar de inmediato a las cámaras
        ApplyZoomSpeed(zoomSpeed);
        ApplyRotationSpeed(rotationSpeed);

        // Suscribir sliders
        if (zoomSpeedSlider != null)
            zoomSpeedSlider.onValueChanged.AddListener(ApplyZoomSpeed);

        if (rotationSpeedSlider != null)
            rotationSpeedSlider.onValueChanged.AddListener(ApplyRotationSpeed);
        Debug.Log("Zoom: " + zoomSpeed + " Rotation: " + rotationSpeed);
    }


    public void ApplyZoomSpeed(float value)
    {
        PlayerPrefs.SetFloat("ZoomSpeed", value);
        PlayerPrefs.Save();

        // Pasar el valor al script de la cámara
        CameraZoom  [] zoom = FindObjectsByType<CameraZoom>(FindObjectsSortMode.None);

    //    if (zoom != null) zoom.SetZoomSpeed(value);
        foreach (CameraZoom i in zoom)
        {
            if (zoom != null) i.SetZoomSpeed(value);
        }
    }

    public void ApplyRotationSpeed(float value)
    {
        PlayerPrefs.SetFloat("RotationSpeed", value);
        PlayerPrefs.Save();

        // Pasar el valor al script de la cámara
        CameraRotation rotation = FindFirstObjectByType<CameraRotation>();
        if (rotation != null) rotation.SetRotationSpeed(value);
    }

    public void SetDefaultValues()
    {
        PlayerPrefs.SetFloat("ZoomSpeed", 160);
        PlayerPrefs.SetFloat("RotationSpeed", 360);
        PlayerPrefs.Save();

        float zoom = PlayerPrefs.GetFloat("ZoomSpeed");
        float rotation = PlayerPrefs.GetFloat("RotationSpeed");
        Debug.Log("Valores settings predeterminados" + zoom + ", " + rotation);

        if (zoomSpeedSlider != null) zoomSpeedSlider.value = defaultZoomSpeed;
        if (rotationSpeedSlider != null) rotationSpeedSlider.value = defaultRotationSpeed;

        // Aplicar a las cámaras inmediatamente
        ApplyZoomSpeed(defaultZoomSpeed);
        ApplyRotationSpeed(defaultRotationSpeed);
    }
}
