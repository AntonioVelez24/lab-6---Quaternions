    using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mathematics.Week6
{
    public class PlaneController : MonoBehaviour
    {
        [Header("Plane")]
        [SerializeField] private float health;
        [SerializeField] private float currentHealth;
        [SerializeField] private float speed = 8f;
        private float xDirection;
        private float yDirection;
        private Rigidbody _compRigidbody;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private GameObject explosionPrefab;

        [Header("Controls Properties")]
        [SerializeField] private float pitchPlane;
        [SerializeField] private float pitchGain = 1f;
        [SerializeField] private MinMax pitchTreshHold;
        [SerializeField] private float rollPlane;
        [SerializeField] private float rollhGain = 1f;
        [SerializeField] private MinMax rollTreshHold;

        [Header("Rotation Data")]
        [SerializeField] private Quaternion qx = Quaternion.identity; //<0,,0,0,1>
        [SerializeField] private Quaternion qy = Quaternion.identity; //<0,,0,0,1>
        [SerializeField] private Quaternion qz = Quaternion.identity; //<0,,0,0,1>

        [SerializeField] private Quaternion r = Quaternion.identity; //<0,,0,0,1>

        private float anguloSen;
        private float anguloCos;

        protected float _pitchDirection = 0f;
        protected float _rollDirection = 0f;

        private void Update()
        {
            if (currentHealth <= 0) {

                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(explosion, 0.5f);
                _audioSource.Play();
                Destroy(gameObject);                
            }
        }
        private void FixedUpdate()
        {
            pitchPlane += _pitchDirection * pitchGain * -1;

            pitchPlane = Mathf.Clamp(pitchPlane, pitchTreshHold.MinValue, pitchTreshHold.MaxValue);

            rollPlane += _rollDirection * rollhGain * -1;

            rollPlane = Mathf.Clamp(rollPlane, rollTreshHold.MinValue, rollTreshHold.MaxValue);

            //rotacion z -> x -> y
            anguloSen = Mathf.Sin(Mathf.Deg2Rad * rollPlane * 0.5f);
            anguloCos = Mathf.Cos(Mathf.Deg2Rad * rollPlane * 0.5f);
            qz.Set(0, 0, anguloSen, anguloCos);

            anguloSen = Mathf.Sin(Mathf.Deg2Rad * pitchPlane * 0.5f);
            anguloCos = Mathf.Cos(Mathf.Deg2Rad * pitchPlane * 0.5f);
            qx.Set(anguloSen, 0, 0, anguloCos);

            /*anguloSen = Mathf.Sin(Mathf.Deg2Rad * rollPlane * 0.5f);
            anguloCos = Mathf.Cos(Mathf.Deg2Rad * rollPlane * 0.5f);
            qy.Set(0, anguloSen, 0, anguloCos);*/

            //multiplicación y -> x -> z
            r = qy * qx * qz;

            transform.rotation = r;

            UpdatePosition();

            _compRigidbody.velocity = new Vector3(speed * xDirection, speed * yDirection, _compRigidbody.velocity.z);

        }

        private void Awake()
        {
            currentHealth = health;
            _compRigidbody = GetComponent<Rigidbody>();
        }
        //Pitch -> X Axis
        public void RotatePitch(InputAction.CallbackContext context)
        {
            _pitchDirection = context.ReadValue<float>();
        }

        //Roll -> Z Axis
        public void RotateRoll(InputAction.CallbackContext context)
        {
            _rollDirection = context.ReadValue<float>();
        }
        public void ReadMovementX(InputAction.CallbackContext context)
        {
            yDirection = context.ReadValue<float>();
        }
        public void ReadMovementZ(InputAction.CallbackContext context)
        {
            xDirection = context.ReadValue<float>();
        }
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.CompareTag("Asteroid"))
            {
                currentHealth = currentHealth - 1;
                Destroy(collision.gameObject);
            }
        }
        private float _verticalDirection = 0f;
        private float _horizontalDirection = 0f;
        [SerializeField] private float velocitySpeed = 5f;

        private Rigidbody _myRB;

        private void Start()
        {
            _myRB = GetComponent<Rigidbody>();
        }

        public void TranslateVertical(InputAction.CallbackContext context)
        {
            _verticalDirection = context.ReadValue<float>();
        }

        public void TranslateHorizontal(InputAction.CallbackContext context)
        {
            _horizontalDirection = context.ReadValue<float>();
        }

        private void UpdatePosition()
        {
            _myRB.velocity = new Vector3(-_horizontalDirection * velocitySpeed, -_verticalDirection * velocitySpeed, 0f);
        }
    }

    [System.Serializable]
    public struct MinMax
    {
        public float MinValue;
        public float MaxValue;
    }
}
