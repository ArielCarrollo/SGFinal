// MototaxiController.cs (Versión 3.0 - Control Total de Rendimiento)
using UnityEngine;

public class MototaxiController : MonoBehaviour
{
    [Header("Referencias a Componentes")]
    public WheelCollider frontWheel;
    public WheelCollider rearWheelLeft;
    public WheelCollider rearWheelRight;

    [Header("Referencias a los Modelos Visuales")]
    public Transform frontWheelTransform;
    public Transform rearWheelLeftTransform;
    public Transform rearWheelRightTransform;

    [Header("Parámetros de Conducción")]
    [Tooltip("Multiplicador máximo de fuerza del motor. La potencia real se define en la Curva de Torque.")]
    public float motorForce = 3000f; // Aumentamos el valor base, ya que ahora es un multiplicador
    [Tooltip("La fuerza máxima de frenado. ¡Debe ser un valor alto!")]
    public float brakeForce = 20000f; // Aumentado drásticamente

    [Header("Ajuste de Física")]
    [Tooltip("Baja el centro de masa para mayor estabilidad.")]
    public Vector3 centerOfMassOffset = new Vector3(0, -0.8f, 0);

    [Header("Estabilidad y Rendimiento Avanzado")]
    [Tooltip("Controla el ángulo de giro máximo según la velocidad (Km/h).")]
    public AnimationCurve steerCurve;
    [Tooltip("Controla la entrega de torque del motor según la velocidad (Km/h). Define la aceleración y velocidad máxima.")]
    public AnimationCurve motorTorqueCurve;

    // Variables internas
    private float horizontalInput;
    private float verticalInput;
    private bool isBraking;
    private Rigidbody rb;
    private float currentSpeed = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += centerOfMassOffset;
    }

    void Update()
    {
        GetInput();
        UpdateWheelVisuals();
    }

    void FixedUpdate()
    {
        currentSpeed = rb.linearVelocity.magnitude * 3.6f;
        HandleMotor();
        HandleSteering();
        HandleBraking();
    }

    void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBraking = Input.GetKey(KeyCode.Space);
    }

    void HandleMotor()
    {
        // Obtenemos el valor de la curva de torque (de 0 a 1) según la velocidad actual.
        float torqueCurveValue = motorTorqueCurve.Evaluate(currentSpeed);

        // Calculamos la fuerza final a aplicar.
        float finalTorque = verticalInput * motorForce * torqueCurveValue;

        rearWheelLeft.motorTorque = finalTorque;
        rearWheelRight.motorTorque = finalTorque;
    }

    void HandleSteering()
    {
        float dynamicMaxSteerAngle = steerCurve.Evaluate(currentSpeed);
        float currentSteerAngle = dynamicMaxSteerAngle * horizontalInput;
        frontWheel.steerAngle = currentSteerAngle;
    }

    void HandleBraking()
    {
        float currentBrakeForce = isBraking ? brakeForce : 0f;
        frontWheel.brakeTorque = currentBrakeForce;
        rearWheelLeft.brakeTorque = currentBrakeForce;
        rearWheelRight.brakeTorque = currentBrakeForce;
    }

    void UpdateWheelVisuals()
    {
        UpdateSingleWheel(frontWheel, frontWheelTransform);
        UpdateSingleWheel(rearWheelLeft, rearWheelLeftTransform);
        UpdateSingleWheel(rearWheelRight, rearWheelRightTransform);
    }

    void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
