using UnityEngine;
using UnityEngine.InputSystem;
using Manager;

namespace Player
{
    public class PlayerMovement : Player
    {
        [Header("Script")]
        [SerializeField]
        private PlayerController _playerController;
        [SerializeField]
        internal FixedJoystick Joystick;

        [Header("InputActions")]
        [SerializeField]
        private PlayerInput _playerInput;

        [Header("Transform")]
        [SerializeField]
        private Transform _camera;
        [SerializeField]
        private Transform _groundCheck;

        [Header("Float Value")]
        [SerializeField]
        private float speed = 6f;
        private float gravity = -9.81f;
        private float groundDistance = 0.3f;
        [SerializeField]
        private float turnSmoothTime = 0.1f;
        private float turnSmoothVelocity;

        [Space]
        [Header("Other")]
        [SerializeField]
        private LayerMask groundMask;
        [SerializeField]
        private bool isGrounded = true;

        [Header("Vector3")]
        private Vector3 velocity;
        internal Vector3 Direction;

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            if (!_playerController.IsDeath)
            {
                Move();
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            speed = GameManager.Instance.GameSceneData.PlayerSpeed;
        }

        /// <summary>
        /// 角色移動
        /// </summary>
        private void Move()
        {
            isGrounded = Physics.CheckSphere(_groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            velocity.y += gravity * Time.deltaTime;
            CharacterController.Move(velocity * Time.deltaTime);

            Vector2 input = _playerInput.actions["Move"].ReadValue<Vector2>();
            float horizontal = input.x;
            float vertical = input.y;

            Direction = new Vector3(horizontal, 0f, vertical).normalized;
            _playerController.Animator.SetFloat("Speed", Direction.magnitude);

            if (Direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                CharacterController.Move(moveDir.normalized * speed * Time.deltaTime);
            }
        }
    }
}