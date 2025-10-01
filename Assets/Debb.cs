using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemDebugger : MonoBehaviour
{
    private GameObject lastSelectedObject;
    private float checkInterval = 0.1f; // Verificar cada 0.1 segundos
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= checkInterval)
        {
            timer = 0f;
            CheckSelection();
        }
    }

    void CheckSelection()
    {
        if (EventSystem.current == null)
        {
            Debug.LogWarning("[EventSystemDebug] No hay EventSystem activo!");
            return;
        }

        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

        // Si cambió la selección
        if (currentSelected != lastSelectedObject)
        {
            string oldName = lastSelectedObject != null ? lastSelectedObject.name : "NADA";
            string newName = currentSelected != null ? currentSelected.name : "NADA";

            // Log con colores para mejor visibilidad
            Debug.Log($"<color=yellow>[EventSystemDebug] Cambio de selección:</color>\n" +
                      $"<color=red>Anterior: {oldName}</color>\n" +
                      $"<color=green>Nuevo: {newName}</color>\n" +
                      $"<color=cyan>Canvas activo: {GetActiveCanvas(currentSelected)}</color>");

            // Si hay un objeto seleccionado, mostrar su jerarquía
            if (currentSelected != null)
            {
                Debug.Log($"<color=magenta>[EventSystemDebug] Jerarquía completa: {GetFullPath(currentSelected)}</color>");

                // Verificar si el objeto está activo
                if (!currentSelected.activeInHierarchy)
                {
                    Debug.LogError($"<color=red>[EventSystemDebug] ¡ALERTA! El objeto '{newName}' está INACTIVO pero fue seleccionado!</color>");
                }
            }

            lastSelectedObject = currentSelected;
        }
    }

    // Obtener el nombre del Canvas padre
    string GetActiveCanvas(GameObject obj)
    {
        if (obj == null) return "Ninguno";

        Canvas canvas = obj.GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            return canvas.gameObject.name;
        }
        return "Sin Canvas";
    }

    // Obtener el path completo del objeto en la jerarquía
    string GetFullPath(GameObject obj)
    {
        if (obj == null) return "";

        string path = obj.name;
        Transform current = obj.transform;

        while (current.parent != null)
        {
            current = current.parent;
            path = current.name + "/" + path;
        }

        return path;
    }

    // Log adicional cuando se presiona una tecla
    void OnGUI()
    {
        // Presiona 'D' para debug manual
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.D)
        {
            ForceDebugLog();
        }
    }

    void ForceDebugLog()
    {
        if (EventSystem.current == null)
        {
            Debug.LogError("[EventSystemDebug] No hay EventSystem!");
            return;
        }

        GameObject selected = EventSystem.current.currentSelectedGameObject;

        Debug.Log("================== DEBUG MANUAL ==================");
        Debug.Log($"Objeto seleccionado: {(selected != null ? selected.name : "NADA")}");

        if (selected != null)
        {
            Debug.Log($"Activo en jerarquía: {selected.activeInHierarchy}");
            Debug.Log($"Activo en self: {selected.activeSelf}");
            Debug.Log($"Canvas padre: {GetActiveCanvas(selected)}");
            Debug.Log($"Path completo: {GetFullPath(selected)}");

            // Verificar si es un botón
            UnityEngine.UI.Button btn = selected.GetComponent<UnityEngine.UI.Button>();
            if (btn != null)
            {
                Debug.Log($"Es un botón - Interactable: {btn.interactable}");
                Debug.Log($"Navigation: {btn.navigation.mode}");
            }
        }
        Debug.Log("==================================================");
    }
}