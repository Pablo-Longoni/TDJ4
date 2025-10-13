using UnityEngine;

public class WaterBoundsToMaterial : MonoBehaviour
{
    /* [Header("Asigná el Renderer del plano de agua")]
     public Renderer waterRenderer;

     [Header("Asigná el Material del ripple (el shader que hicimos)")]
     public Material rippleMaterial;

     [Header("Nombres de las propiedades en el shader")]
     public string waterCenterProperty = "_WaterCenter";
     public string waterSizeProperty = "_WaterSizeXZ";



     void Update()
     {
         if (waterRenderer == null || rippleMaterial == null)
             return;

         // Tomamos el bounding box del plano de agua
         Bounds b = waterRenderer.bounds;

         // El centro del plano en coordenadas de mundo
         Vector3 center = b.center;

         // Ancho (X) y largo (Z) del plano
         Vector2 sizeXZ = new Vector2(b.size.x, b.size.z);

         // Actualizamos las propiedades en el material del ripple
         rippleMaterial.SetVector(waterCenterProperty, center);
         rippleMaterial.SetVector(waterSizeProperty, new Vector4(sizeXZ.x, sizeXZ.y, 0, 0));
     }*/

        [Header("Renderer del plano del ripple (este objeto)")]
        public Renderer rippleRenderer;

        [Header("Material base del efecto ripple (el shader)")]
        public Material rippleMaterial;

        [Header("Nombres de las propiedades en el shader")]
        public string waterCenterProperty = "_WaterCenter";
        public string waterSizeProperty = "_WaterSizeXZ";

        private Material _instanceMaterial;
        private Renderer _waterRenderer;

        void Start()
        {
            // Crear una copia del material para este ripple
            _instanceMaterial = new Material(rippleMaterial);
            rippleRenderer.material = _instanceMaterial;

            // Buscar el plano de agua más cercano o asignarlo desde afuera
            _waterRenderer = FindClosestWaterRenderer();
        }

        void Update()
        {
            if (_waterRenderer == null || _instanceMaterial == null)
                return;

            Bounds b = _waterRenderer.bounds;
            Vector3 center = b.center;
            Vector2 sizeXZ = new Vector2(b.size.x, b.size.z);

            _instanceMaterial.SetVector(waterCenterProperty, center);
            _instanceMaterial.SetVector(waterSizeProperty, new Vector4(sizeXZ.x, sizeXZ.y, 0, 0));
        }

        private Renderer FindClosestWaterRenderer()
        {
            // Busca un Renderer con tag "Water" (así cada ripple sabe a qué agua pertenece)
            GameObject water = GameObject.FindGameObjectWithTag("Water");
            return water ? water.GetComponent<Renderer>() : null;
        }
 }

