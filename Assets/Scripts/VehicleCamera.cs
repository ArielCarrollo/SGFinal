// VehicleCamera.cs
using UnityEngine;

public class VehicleCamera : MonoBehaviour
{
    [Tooltip("El objeto que la c�mara debe seguir (la mototaxi).")]
    public Transform target;
    [Tooltip("Qu� tan lejos estar� la c�mara del objetivo.")]
    public float distance = 8.0f;
    [Tooltip("Qu� tan alta estar� la c�mara con respecto al objetivo.")]
    public float height = 3.0f;
    [Tooltip("Suavizado del movimiento de la c�mara al seguir en altura.")]
    public float heightDamping = 2.0f;
    [Tooltip("Suavizado de la rotaci�n de la c�mara al seguir.")]
    public float rotationDamping = 3.0f;

    void LateUpdate()
    {
        // Es crucial que la c�mara se actualice en LateUpdate,
        // despu�s de que el target (veh�culo) haya completado su movimiento en Update/FixedUpdate.
        if (!target)
            return;

        // Calcular la posici�n y rotaci�n deseadas
        float wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;

        // Suavizar la rotaci�n y la altura
        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Convertir el �ngulo a una rotaci�n
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Colocar la c�mara detr�s del objetivo
        transform.position = target.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        // Ajustar la altura final de la c�mara
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        // Apuntar la c�mara siempre hacia el objetivo
        transform.LookAt(target);
    }
}
