using UnityEngine;
using UnityEngine.InputSystem;
using System.Runtime.InteropServices;
using System.Text;

public class GamepadVibrationDebugger : MonoBehaviour
{
    [Header("Test Settings")]
    [Range(0f, 1f)]
    public float testIntensity = 0.8f;
    public float testDuration = 2f;

    [Header("Controls")]
    public KeyCode testInputSystemKey = KeyCode.Alpha1;
    public KeyCode testXInputKey = KeyCode.Alpha2;
    public KeyCode testBothMotorsKey = KeyCode.Alpha3;
    public KeyCode testLeftMotorKey = KeyCode.Alpha4;
    public KeyCode testRightMotorKey = KeyCode.Alpha5;
    public KeyCode stopKey = KeyCode.Space;

    private bool isTesting = false;

    // XInput para Windows
    [DllImport("XInput1_4.dll")]
    private static extern uint XInputSetState(uint dwUserIndex, ref XInputVibration pVibration);

    [DllImport("XInput1_4.dll")]
    private static extern uint XInputGetState(uint dwUserIndex, ref XInputState pState);

    [StructLayout(LayoutKind.Sequential)]
    private struct XInputVibration
    {
        public ushort wLeftMotorSpeed;
        public ushort wRightMotorSpeed;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct XInputState
    {
        public uint dwPacketNumber;
        public XInputGamepad Gamepad;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct XInputGamepad
    {
        public ushort wButtons;
        public byte bLeftTrigger;
        public byte bRightTrigger;
        public short sThumbLX;
        public short sThumbLY;
        public short sThumbRX;
        public short sThumbRY;
    }

    void Start()
    {
        PrintGamepadInfo();
    }

    void Update()
    {
        if (isTesting) return;

        if (Input.GetKeyDown(testInputSystemKey))
            StartCoroutine(TestInputSystemVibration());

        if (Input.GetKeyDown(testXInputKey))
            StartCoroutine(TestXInputVibration());

        if (Input.GetKeyDown(testBothMotorsKey))
            StartCoroutine(TestBothMotorsSeparately());

        if (Input.GetKeyDown(testLeftMotorKey))
            StartCoroutine(TestLeftMotorOnly());

        if (Input.GetKeyDown(testRightMotorKey))
            StartCoroutine(TestRightMotorOnly());

        if (Input.GetKeyDown(stopKey))
            StopAllVibration();
    }

    // ✅ Método corregido y compatible con todas las versiones del Input System
    void PrintGamepadInfo()
    {
        StringBuilder info = new StringBuilder();
        info.AppendLine("=== GAMEPAD DIAGNOSTICS ===");
        info.AppendLine();

        var gamepad = Gamepad.current;
        if (gamepad != null)
        {
            info.AppendLine($"✓ Gamepad detectado por Input System");
            info.AppendLine($"  Nombre: {gamepad.name}");
            info.AppendLine($"  Display Name: {gamepad.displayName}");
            info.AppendLine($"  Device Class: {gamepad.description.deviceClass}");
            info.AppendLine($"  Manufacturer: {gamepad.description.manufacturer}");
            info.AppendLine($"  Product: {gamepad.description.product}");

            // Detección háptica segura (solo informativa)
            bool soportaHapticos = gamepad != null &&
                                   gamepad.GetType().GetMethod("SetMotorSpeeds") != null;
            info.AppendLine($"  Soporta motores hápticos: {soportaHapticos}");
            info.AppendLine();
        }
        else
        {
            info.AppendLine("✗ No hay gamepad detectado por Input System");
            info.AppendLine();
        }

        XInputState state = new XInputState();
        uint xInputResult = XInputGetState(0, ref state);

        if (xInputResult == 0)
        {
            info.AppendLine($"✓ XInput detectó un control en slot 0");
            info.AppendLine($"  Packet Number: {state.dwPacketNumber}");
        }
        else
        {
            info.AppendLine($"✗ XInput no detectó control (Error: {xInputResult})");
        }

        info.AppendLine();
        info.AppendLine("=== CONTROLES DE PRUEBA ===");
        info.AppendLine($"[{testInputSystemKey}] - Test Input System API");
        info.AppendLine($"[{testXInputKey}] - Test XInput API (Windows)");
        info.AppendLine($"[{testBothMotorsKey}] - Test ambos motores por separado");
        info.AppendLine($"[{testLeftMotorKey}] - Test motor izquierdo solo");
        info.AppendLine($"[{testRightMotorKey}] - Test motor derecho solo");
        info.AppendLine($"[{stopKey}] - Detener vibración");
        info.AppendLine();
        info.AppendLine($"Intensidad de prueba: {testIntensity:F2}");
        info.AppendLine($"Duración de prueba: {testDuration}s");

        Debug.Log(info.ToString());
    }

    System.Collections.IEnumerator TestInputSystemVibration()
    {
        isTesting = true;
        Debug.Log(">>> TEST 1: Input System API <<<");

        var gamepad = Gamepad.current;
        if (gamepad == null)
        {
            Debug.LogError("No hay gamepad conectado!");
            isTesting = false;
            yield break;
        }

        gamepad.ResetHaptics();
        yield return new WaitForSecondsRealtime(0.1f);

        gamepad.ResumeHaptics();
        yield return new WaitForSecondsRealtime(0.1f);

        Debug.Log($"Activando motores a {testIntensity:F2}...");
        gamepad.SetMotorSpeeds(testIntensity, testIntensity);

        float elapsed = 0f;
        while (elapsed < testDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            Debug.Log($"Vibrando... {elapsed:F2}s / {testDuration}s");
            yield return new WaitForSecondsRealtime(0.5f);
        }

        gamepad.SetMotorSpeeds(0f, 0f);
        gamepad.PauseHaptics();
        Debug.Log(">>> TEST 1 COMPLETADO <<<");

        isTesting = false;
    }

    System.Collections.IEnumerator TestXInputVibration()
    {
        isTesting = true;
        Debug.Log(">>> TEST 2: XInput API (Windows) <<<");

        ushort motorSpeed = (ushort)(testIntensity * 65535);

        XInputVibration vibration = new XInputVibration
        {
            wLeftMotorSpeed = motorSpeed,
            wRightMotorSpeed = motorSpeed
        };

        uint result = XInputSetState(0, ref vibration);

        if (result != 0)
        {
            Debug.LogError($"XInput falló con código: {result}");
            isTesting = false;
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < testDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return new WaitForSecondsRealtime(0.5f);
        }

        vibration.wLeftMotorSpeed = 0;
        vibration.wRightMotorSpeed = 0;
        XInputSetState(0, ref vibration);

        Debug.Log(">>> TEST 2 COMPLETADO <<<");
        isTesting = false;
    }

    System.Collections.IEnumerator TestBothMotorsSeparately()
    {
        isTesting = true;
        Debug.Log(">>> TEST 3: Ambos motores por separado <<<");

        var gamepad = Gamepad.current;
        if (gamepad == null)
        {
            Debug.LogError("No hay gamepad conectado!");
            isTesting = false;
            yield break;
        }

        gamepad.ResetHaptics();
        gamepad.ResumeHaptics();

        Debug.Log("Activando MOTOR BAJO (low frequency)...");
        gamepad.SetMotorSpeeds(testIntensity, 0f);
        yield return new WaitForSecondsRealtime(testDuration / 2);

        Debug.Log("Activando MOTOR ALTO (high frequency)...");
        gamepad.SetMotorSpeeds(0f, testIntensity);
        yield return new WaitForSecondsRealtime(testDuration / 2);

        gamepad.SetMotorSpeeds(0f, 0f);
        gamepad.PauseHaptics();

        Debug.Log(">>> TEST 3 COMPLETADO <<<");
        isTesting = false;
    }

    System.Collections.IEnumerator TestLeftMotorOnly()
    {
        isTesting = true;
        Debug.Log(">>> TEST 4: Solo motor IZQUIERDO <<<");

        var gamepad = Gamepad.current;
        if (gamepad == null)
        {
            Debug.LogError("No hay gamepad conectado!");
            isTesting = false;
            yield break;
        }

        gamepad.ResetHaptics();
        gamepad.ResumeHaptics();
        gamepad.SetMotorSpeeds(testIntensity, 0f);

        yield return new WaitForSecondsRealtime(testDuration);

        gamepad.SetMotorSpeeds(0f, 0f);
        gamepad.PauseHaptics();

        Debug.Log(">>> TEST 4 COMPLETADO <<<");
        isTesting = false;
    }

    System.Collections.IEnumerator TestRightMotorOnly()
    {
        isTesting = true;
        Debug.Log(">>> TEST 5: Solo motor DERECHO <<<");

        var gamepad = Gamepad.current;
        if (gamepad == null)
        {
            Debug.LogError("No hay gamepad conectado!");
            isTesting = false;
            yield break;
        }

        gamepad.ResetHaptics();
        gamepad.ResumeHaptics();
        gamepad.SetMotorSpeeds(0f, testIntensity);

        yield return new WaitForSecondsRealtime(testDuration);

        gamepad.SetMotorSpeeds(0f, 0f);
        gamepad.PauseHaptics();

        Debug.Log(">>> TEST 5 COMPLETADO <<<");
        isTesting = false;
    }

    void StopAllVibration()
    {
        Debug.Log("=== DETENIENDO TODA VIBRACIÓN ===");

        var gamepad = Gamepad.current;
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(0f, 0f);
            gamepad.PauseHaptics();
            Debug.Log("Input System detenido");
        }

        XInputVibration vibration = new XInputVibration
        {
            wLeftMotorSpeed = 0,
            wRightMotorSpeed = 0
        };
        XInputSetState(0, ref vibration);
        Debug.Log("XInput detenido");

        StopAllCoroutines();
        isTesting = false;
    }

    void OnDestroy() => StopAllVibration();
    void OnApplicationQuit() => StopAllVibration();
}
