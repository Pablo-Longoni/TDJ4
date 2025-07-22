using UnityEngine;

public class JoystickDebugger : MonoBehaviour
{
    void Update()
    {
        // Probar los primeros 10 ejes de joystick directamente
        for (int i = 1; i <= 10; i++)
        {
            string axisName = "joystick " + 1 + " axis " + i; // Asumimos joystick 1
            try
            {
                float value = Input.GetAxisRaw(axisName);
                if (Mathf.Abs(value) > 0.1f)
                {
                    Debug.Log(axisName + ": " + value);
                }
            }
            catch
            {
                // Silenciar errores si el eje no está definido
            }
        }

        // Detectar botones presionados
        for (int i = 0; i < 20; i++)
        {
            if (Input.GetKeyDown("joystick 1 button " + i))
            {
                Debug.Log("Joystick button " + i + " pressed");
            }
        }
    }
}
