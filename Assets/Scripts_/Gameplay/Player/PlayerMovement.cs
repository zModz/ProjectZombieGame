using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Scripts_.Gameplay.Player
{
    public class PlayerMovement : NetworkBehaviour
    {
        public PlayerInput playerInput;
        private CharacterController _char;

        [Header("Player Attributes")]
        public bool isInverted;

        // bool isDown;
        // bool wasDown;

        [Header("Character Attributes")]
        public int walkSpeed;
        private int _walkSpeed;
        public int runMulti;
        public int runningSpeed;
        public float jumpHeight;
        public Transform groundCheck;
        public LayerMask groundMask;
        private const float GroundDistance = 0.1f;

        // float fallMulti = 2.5f;
        // float lowMulti = 2f;
        private Rigidbody _rb;
        public bool isGrounded;
        public bool isMoving;
        public bool isRunning;

        public enum MovementEnum
        {
            IsStopped,
            IsMoving,
            IsRunning
        }

        [FormerlySerializedAs("MoveEnum")]
        public MovementEnum moveEnum;
        private Collider[] _childrenColliders;
        private Vector3 _velocity;

        //[Header("Camera Attributes")]
        //public GameObject cam;

        [Header("Head Bobbing")]
        public float walkingBobbingSpeed = 14f;
        public float bobbingAmount = 0.05f;

        //float defaultPosY = 0;
        //float timer = 0;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;

            _char = GetComponent<CharacterController>();
            _rb = GetComponent<Rigidbody>();
            playerInput = GetComponent<PlayerInput>();

            //defaultPosY = transform.localPosition.y;
        }

        // Start is called before the first frame update
        private void Start()
        {
            runningSpeed = walkSpeed * runMulti; // Calculates running speed by multiplying walkSpeed with a multiplier
            _walkSpeed = walkSpeed; // set a temp variable for walkSpeed

            _childrenColliders = GetComponentsInChildren<Collider>();
            foreach (var col in _childrenColliders)
            {
                // checking if it is our collider, then skip it,
                if (col != GetComponent<Collider>())
                {
                    // if it is not our collider then ignore collision between our collider and childs collider
                    Physics.IgnoreCollision(col, GetComponent<Collider>());
                }
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (!IsOwner)
                return;

            Movement();
        }

        private void Movement()
        {
            // Returns if player is not the local player
            if (!IsOwner)
                return;

            // Gravity
            _velocity.y += -9.81f * Time.deltaTime;
            _char.Move(_velocity * Time.deltaTime);

            // Checks if player is touching the ground
            isGrounded = Physics.CheckSphere(groundCheck.position, GroundDistance, groundMask);

            // Movement
            var input = playerInput.actions["Move"].ReadValue<Vector2>();
            var move = transform.right * input.x + transform.forward * input.y;
            _char.Move(move * (walkSpeed * Time.deltaTime));

            // Debugging players movement
            Debug.Log(move);

            // Jump
            if (playerInput.actions["Jump"].WasPressedThisFrame() && isGrounded)
            {
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * -9.81f);
            }

            // Sprint
            // TODO: Make sprint better
            if (move.z > 0)
            {
                if (playerInput.actions["Sprint"].IsPressed())
                {
                    walkSpeed = runningSpeed;
                    moveEnum = MovementEnum.IsRunning;
                }
                else
                {
                    walkSpeed = _walkSpeed;
                }
            }

            // Enum
            moveEnum = MovementEnum.IsStopped;
            if (move.z != 0 || move.x != 0)
            {
                moveEnum = MovementEnum.IsMoving;
            }
        }
    }
}
