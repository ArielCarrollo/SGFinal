// AntiRollBar.cs
using UnityEngine;

public class AntiRollBar : MonoBehaviour
{
    [Tooltip("El WheelCollider de la rueda izquierda a conectar.")]
    public WheelCollider wheelL;
    [Tooltip("El WheelCollider de la rueda derecha a conectar.")]
    public WheelCollider wheelR;
    [Tooltip("La rigidez de la barra estabilizadora. Un valor alto hace que el chasis se incline menos.")]
    public float antiRollForce = 5000f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Esta estructura se usa para obtener informaci�n de la suspensi�n de cada rueda.
        WheelHit hit;
        float travelL = 1.0f;
        float travelR = 1.0f;

        // Comprobamos si la rueda izquierda est� tocando el suelo.
        bool groundedL = wheelL.GetGroundHit(out hit);
        if (groundedL)
        {
            // Si toca, calculamos qu� tan comprimida est� la suspensi�n (0 = totalmente comprimida, 1 = totalmente extendida).
            travelL = (-wheelL.transform.InverseTransformPoint(hit.point).y - wheelL.radius) / wheelL.suspensionDistance;
        }

        // Hacemos lo mismo para la rueda derecha.
        bool groundedR = wheelR.GetGroundHit(out hit);
        if (groundedR)
        {
            travelR = (-wheelR.transform.InverseTransformPoint(hit.point).y - wheelR.radius) / wheelR.suspensionDistance;
        }

        // Calculamos la diferencia de compresi�n entre las dos suspensiones.
        float antiRollGrounded = (travelL - travelR) * antiRollForce;

        // Si una rueda est� en el suelo, aplicamos la fuerza para contrarrestar el balanceo.
        if (groundedL)
            rb.AddForceAtPosition(wheelL.transform.up * -antiRollGrounded, wheelL.transform.position);

        if (groundedR)
            rb.AddForceAtPosition(wheelR.transform.up * antiRollGrounded, wheelR.transform.position);
    }
}
