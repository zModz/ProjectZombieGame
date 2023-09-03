using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Scripts_.Gameplay.Player
{
    public class CameraMovement : NetworkBehaviour
    {
        public Transform player;
        public PlayerInput playerInput;
        public bool isInverted;
        [Header("Camera Attributes")]
        public GameObject cam;
        private Camera _camera;
        private CameraMovement _cameraMovement;
        // public Camera weaponCam;
        [FormerlySerializedAs("cam_sens_x")] [Range(0, 100)]
        public int camSensX = 1;
        [FormerlySerializedAs("cam_sens_y")] [Range(0, 100)]
        public int camSensY = 1;
        public float v;
        public float h;

        private void Awake()
        {
            _camera = cam.GetComponent<Camera>();
            _cameraMovement = cam.GetComponent<CameraMovement>();
        }

        // Start is called before the first frame update
        private void Start()
        {
            cam = gameObject; // this.gameObject
        }

        // Update is called once per frame
        private void Update()
        {
            if (!IsOwner) return;

            if (!_camera.enabled && !_cameraMovement.enabled)
            {
                _camera.enabled = true;
                _cameraMovement.enabled = true;
            }

            CamMove();
        }
    
        private void CamMove()
        {
            if (!IsOwner) return;
        
            if (Time.deltaTime == 0) { return; }

            // cam_sens_x *= 5;
            // cam_sens_y *= 5;
        
            var camInput = playerInput.actions["Look"].ReadValue<Vector2>();

            // Cam Movement // Keyboard
            if (!isInverted)
            {
                h = camInput.x * camSensX * Time.deltaTime;
                v += camInput.y * -camSensY * Time.deltaTime;

                // Rotate player with mouse input
                player.transform.Rotate(Vector3.up * h);

                // Rotate camera on the Y axis
                var xRotation = Mathf.Clamp(v, -90, 90);
                cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            }
            else
            {
                var hor = camInput.x * -camSensX * Time.deltaTime;
                v += camInput.y * camSensY * Time.deltaTime;

                // Rotate player with mouse input
                player.transform.Rotate(Vector3.up * hor);

                // Rotate camera on the Y axis
                var xRotation = Mathf.Clamp(-v, -90, 90);
                cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            }
        }
    }
}
