using UnityEngine;
using UnityEditor;
using Unity.Cinemachine; 
/*using System.Collections.Generic;
using System;
using UnityEngine.XR;
using NUnit.Framework.Constraints;
using static UnityEditor.Experimental.GraphView.GraphView;
using System.Threading;
public class LvlCreatorWindow : EditorWindow
{

    private List<GameObject> placedObjects = new List<GameObject>();
   
    private GameObject selectedObject;
    private Vector2 scrollPos;
    [SerializeField] private ObjectsPrefab prefabDatabase;

    [MenuItem("Tool/Level tool")]
    public static void ShowWindow()
    {
        GetWindow<LvlCreatorWindow>("Level Builder");
    }

      private void OnGUI()
      {
        //CAMBIAR CÁMARA
        GUILayout.Label("Cambiar cámara", EditorStyles.boldLabel);

        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
        if (GUILayout.Button("Isométrica", GUILayout.Height(30)))
        {
            SwitchToCamera("Isometric Camera");
        }
        if (GUILayout.Button("Cenital", GUILayout.Height(30)))
        {
            SwitchToCamera("OverHead Camera");
        }
        EditorGUILayout.EndHorizontal();


        //AGREGAR ELEMENTOS
        if (prefabDatabase == null)
        {
            EditorGUILayout.HelpBox("Asigná un Prefab Database", MessageType.Warning);
            prefabDatabase = (ObjectsPrefab)EditorGUILayout.ObjectField("Prefab Database", prefabDatabase, typeof(ObjectsPrefab), false);
            return;
        }

        GUILayout.Label("Figuras", EditorStyles.boldLabel);
        foreach (var figure in prefabDatabase._figuresPrefabs)
        {
            if (GUILayout.Button(figure.name))
            {
                SpawnPrefab(figure);
            }
        }

        GUILayout.Label("Objetos", EditorStyles.boldLabel);
        foreach (var figure in prefabDatabase._objectsPrefabs)
        {
            if (GUILayout.Button(figure.name))
            {
                SpawnPrefab(figure);
            }
        }


        //ARRAY DE ELEMENTOS Y ELIMINAR
        GUILayout.Label("Elementos colocados", EditorStyles.boldLabel);
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(100));
        GameObject objectToRemove = null;

        foreach (var obj in placedObjects)
        {
            if (obj != null)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(obj.name))
                {
                    Selection.activeGameObject = obj;
                    selectedObject = obj;
                }
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    objectToRemove = obj; // Marcar para eliminar después
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndScrollView();

        if (objectToRemove != null)
        {
            DestroyImmediate(objectToRemove);
            placedObjects.Remove(objectToRemove);
            if (selectedObject == objectToRemove)
                selectedObject = null;
        }


        //ROTACIÓN
        if (selectedObject != null)
        {
            GUILayout.Space(10);
            GUILayout.Label("Controles del objeto seleccionado", EditorStyles.boldLabel);

            EditorGUILayout.BeginVertical(GUILayout.Width(100));
            if (GUILayout.Button("UP", GUILayout.Height(30)))
            {
                selectedObject.transform.Rotate(-90, 0, 0); // Mirar hacia arriba
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("LEFT", GUILayout.Height(30)))
            {
                selectedObject.transform.Rotate(0, -90, 0); // Girar izquierda
            }
            if (GUILayout.Button("RIGHT", GUILayout.Height(30)))
            {
                selectedObject.transform.Rotate(0, 90, 0); // Girar derecha
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("DOWN", GUILayout.Height(30)))
            {
                selectedObject.transform.Rotate(90, 0, 0); // Mirar hacia abajo
            }
            EditorGUILayout.EndVertical();

            /*    Vector3 newScale = EditorGUILayout.Vector3Field("Escala", selectedObject.transform.localScale);
                if (newScale != selectedObject.transform.localScale)
                {
                    selectedObject.transform.localScale = newScale;
                }
        }

        //  HandleKeyboardInput();
    }
    

    private void SpawnPrefab(object figure)
    {

        GameObject prefab = figure as GameObject;
        if (prefab != null)
        {
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            Undo.RegisterCreatedObjectUndo(instance, "Placed prefab");
            instance.transform.position = Vector3.zero;
            placedObjects.Add(instance);
            Selection.activeGameObject = instance;
            selectedObject = instance;

            var playerTrans = instance.GetComponent<PlayerTransformation>();
            var playerMove = instance.GetComponent<PlayerMovement>();
            var teleport = instance.GetComponentsInChildren<Telport>();
            var ui = GameObject.FindFirstObjectByType<UIreference>();
            var figures = instance.GetComponent<CubeRotation>();
            var pressed = instance.GetComponentInChildren<OpenPortal>();
            if (ui != null)
            {
                if (playerTrans != null)
                {
                    playerTrans._textTrans = ui.transformationText;
                    playerTrans._cameraChange = ui._cameraChange;
                    playerTrans._cheatButton = ui._cheatTrans;
                    Debug.Log("Se cargaron los campos en playerTrans");
                }

                if (playerMove != null)
                {
                    playerMove._minimapCameraFollow = ui._minimapCameraFollow;
                    Debug.Log("Se cargaron los campos en playerMove");
                }

                if (teleport.Length > 0)
                {
                    foreach (var t in teleport)
                    {
                        t._playerGrab = ui._playerGrab;
                    }
                    Debug.Log("Se cargaron los campos en todos los scripts Telport");
                }

                if (figures != null)
                {
                    figures._cameraChange = ui._cameraChange;
                    Debug.Log("Se cargaron los campos en la figura");
                }

                if(pressed != null)
                {
                    pressed._portal = ui._portal;
                    Debug.Log("Se cargaron los campos en pressed");
                }
            }
        }
    }
   
    private void SwitchToCamera(string cameraName)
    {
        var vcam = GameObject.Find(cameraName)?.GetComponent<CinemachineCamera>();
        if (vcam == null)
        {
            Debug.LogWarning("No se encontró la cámara: " + cameraName);
            return;
        }

        // Actualiza prioridades
        foreach (var cam in GameObject.FindObjectsByType<CinemachineCamera>(sortMode: FindObjectsSortMode.None))
        {
            cam.Priority = (cam == vcam) ? 20 : 10;
        }

        SceneView sceneView = SceneView.lastActiveSceneView;
        
        if (sceneView != null)
        {
            // Asignamos la posición y rotación de la virtual camera a la SceneView
            sceneView.pivot = vcam.transform.position + vcam.transform.forward * 10;
            sceneView.rotation = vcam.transform.rotation;
            sceneView.Repaint();
        }
    }


}
*/