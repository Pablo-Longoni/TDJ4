using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class SceneFlowDebugger : MonoBehaviour
{
    [Header("Claves de PlayerPrefs a inspeccionar (pon las de tu juego)")]
    [SerializeField] private string[] playerPrefsKeys = { "nivelActual", "ultimoNivel", "nivelDesbloqueado", "level" };

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Debug.Log($"<color=yellow>[SceneFlow] ===== INICIO =====</color>\n" +
                  $"Platform: {Application.platform}\n" +
                  $"Escena activa: {SceneManager.GetActiveScene().name} (index {SceneManager.GetActiveScene().buildIndex})\n" +
                  $"Resolución: {Screen.width}x{Screen.height} | DPI: {Screen.dpi}\n" +
                  $"Touch soportado: {Input.touchSupported} | Mouse simula touch: {Input.simulateMouseWithTouches}\n" +
                  $"Escenas en Build: {SceneManager.sceneCountInBuildSettings}");

#if UNITY_ANDROID
        Debug.Log("[SceneFlow] Compilando con define UNITY_ANDROID (busca #if UNITY_ANDROID en tu código)");
#endif
#if UNITY_EDITOR
        Debug.Log("[SceneFlow] UNITY_EDITOR activo");
#endif

        DumpPlayerPrefs();

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += s => Debug.Log($"<color=orange>[SceneFlow] DESCARGADA: {s.name} | frame {Time.frameCount} | t={Time.realtimeSinceStartup:F2}</color>");
        SceneManager.activeSceneChanged += (a, b) =>
            Debug.Log($"<color=red>[SceneFlow] ACTIVE SCENE CAMBIÓ: {(a.IsValid() ? a.name : "-")} -> {b.name} | frame {Time.frameCount}</color>");

        HookButtons();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"<color=green>[SceneFlow] CARGADA: {scene.name} (index {scene.buildIndex}) modo {mode} | frame {Time.frameCount} | t={Time.realtimeSinceStartup:F2}</color>");
        DumpPlayerPrefs();
        HookButtons();
    }

    void DumpPlayerPrefs()
    {
        foreach (var key in playerPrefsKeys)
        {
            if (PlayerPrefs.HasKey(key))
                Debug.Log($"[SceneFlow] PlayerPrefs '{key}' = int:{PlayerPrefs.GetInt(key, -999)} / string:'{PlayerPrefs.GetString(key, "")}'");
        }
    }

    // Engancha un log a TODOS los botones (incluso inactivos) y muestra sus listeners del Inspector
    void HookButtons()
    {
        foreach (var btn in Resources.FindObjectsOfTypeAll<Button>())
        {
            if (!btn.gameObject.scene.IsValid()) continue; // ignora prefabs/assets

            var b = btn;
            b.onClick.RemoveListener(null);
            b.onClick.AddListener(() =>
                Debug.Log($"<color=cyan>[SceneFlow] CLICK en botón '{GetPath(b.gameObject)}' | frame {Time.frameCount}\n{new StackTrace(true)}</color>"));

            int count = b.onClick.GetPersistentEventCount();
            for (int i = 0; i < count; i++)
            {
                Debug.Log($"[SceneFlow] Botón '{b.name}' -> {b.onClick.GetPersistentTarget(i)}.{b.onClick.GetPersistentMethodName(i)}");
            }
        }
    }

    string GetPath(GameObject obj)
    {
        string path = obj.name;
        var t = obj.transform;
        while (t.parent != null) { t = t.parent; path = t.name + "/" + path; }
        return path;
    }
}