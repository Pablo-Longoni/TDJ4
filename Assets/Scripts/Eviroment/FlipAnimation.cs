using System.Collections;
using UnityEngine;

public class FlipAnimation : MonoBehaviour
{
    [SerializeField] private Transform _flip;


    [SerializeField] private float speed = 2f;
    [SerializeField] private float angle = 15f; // Ángulo máximo de rotación

    private float _time;

    private void Update()
    {
        if (_flip == null) return;

        _time += Time.deltaTime * speed;

        // Calcula el ángulo oscilante entre -angle y +angle
        float rotationX = Mathf.Sin(_time) * angle;

        // Aplica la rotación solo en el eje X (puedes cambiarlo si querés que sea Y o Z)
        _flip.localRotation = Quaternion.Euler(rotationX, 0f, 0f); 
        _flip.localRotation = Quaternion.Euler(rotationX, Time.time * 50f, 0f);
    }
}
