// VehicleCamera.cs
using UnityEngine;

public class VehicleCamera : MonoBehaviour
{
    [Tooltip("El objeto que la cámara debe seguir (la mototaxi).")]
    public Transform target;
    [Tooltip("Qué tan lejos estará la cámara del objetivo.")]
    public float distance = 8.0f;
    [Tooltip("Qué tan alta estará la cámara con respecto al objetivo.")]
    public float height = 3.0f;
    [Tooltip("Suavizado del movimiento de la cámara al seguir en altura.")]
    public float heightDamping = 2.0f;
    [Tooltip("Suavizado de la rotación de la cámara al seguir.")]
    public float rotationDamping = 3.0f;

    void LateUpdate()
    {
        // Es crucial que la cámara se actualice en LateUpdate,
        // después de que el target (vehículo) haya completado su movimiento en Update/FixedUpdate.
        if (!target)
            return;

        // Calcular la posición y rotación deseadas
        float wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;

        // Suavizar la rotación y la altura
        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Convertir el ángulo a una rotación
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Colocar la cámara detrás del objetivo
        transform.position = target.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        // Ajustar la altura final de la cámara
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        // Apuntar la cámara siempre hacia el objetivo
        transform.LookAt(target);
    }
}
