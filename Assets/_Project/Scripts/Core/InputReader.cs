using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Haxolot.Core
{
    /// <summary>
    /// Passerelle écoutant le New Input System et diffusant des événements C#.
    /// Fonctionne avec PlayerInput ou directement avec les périphériques (Clavier / Souris / Manette).
    /// </summary>
    public class InputReader : MonoBehaviour
    {
        [Header("Référence directe optionnelle")]
        [SerializeField] private InputActionAsset inputActions;

        public event Action<Vector2> OnMoveEvent;
        public event Action OnDashEvent;
        public event Action OnAttackEvent;
        public event Action OnParryEvent;
        public event Action OnInteractEvent;

        private InputAction moveAction;
        private InputAction dashAction;
        private InputAction attackAction;
        private InputAction parryAction;
        private InputAction interactAction;

        private Vector2 currentMoveInput;
        public Vector2 MoveInput => currentMoveInput;

        private void Awake()
        {
            PlayerInput playerInput = GetComponent<PlayerInput>();
            InputActionAsset asset = inputActions != null ? inputActions : (playerInput != null ? playerInput.actions : null);

            if (asset != null)
            {
                var playerMap = asset.FindActionMap("Player");
                if (playerMap != null)
                {
                    moveAction = playerMap.FindAction("Move");
                    dashAction = playerMap.FindAction("Sprint") ?? playerMap.FindAction("Jump") ?? playerMap.FindAction("Dash");
                    attackAction = playerMap.FindAction("Attack");
                    parryAction = playerMap.FindAction("Parry") ?? playerMap.FindAction("Crouch");
                    interactAction = playerMap.FindAction("Interact");
                }
            }
        }

        private void Update()
        {
            // Raccourci direct de confort : Clic droit souris = Parade immédiate
            if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
            {
                OnParryEvent?.Invoke();
            }
        }

        private void OnEnable()
        {
            if (moveAction != null)
            {
                moveAction.performed += OnMovePerformed;
                moveAction.canceled += OnMoveCanceled;
                moveAction.Enable();
            }

            if (dashAction != null)
            {
                dashAction.performed += ctx => OnDashEvent?.Invoke();
                dashAction.Enable();
            }

            if (attackAction != null)
            {
                attackAction.performed += ctx => OnAttackEvent?.Invoke();
                attackAction.Enable();
            }

            if (parryAction != null)
            {
                parryAction.performed += ctx => OnParryEvent?.Invoke();
                parryAction.Enable();
            }

            if (interactAction != null)
            {
                interactAction.performed += ctx => OnInteractEvent?.Invoke();
                interactAction.Enable();
            }
        }

        private void OnDisable()
        {
            if (moveAction != null)
            {
                moveAction.performed -= OnMovePerformed;
                moveAction.canceled -= OnMoveCanceled;
                moveAction.Disable();
            }

            dashAction?.Disable();
            attackAction?.Disable();
            parryAction?.Disable();
            interactAction?.Disable();
        }

        private void OnMovePerformed(InputAction.CallbackContext ctx)
        {
            currentMoveInput = ctx.ReadValue<Vector2>();
            OnMoveEvent?.Invoke(currentMoveInput);
        }

        private void OnMoveCanceled(InputAction.CallbackContext ctx)
        {
            currentMoveInput = Vector2.zero;
            OnMoveEvent?.Invoke(Vector2.zero);
        }

        // Support du mode "Send Messages" du composant PlayerInput de Unity
        public void OnMove(InputValue value)
        {
            currentMoveInput = value.Get<Vector2>();
            OnMoveEvent?.Invoke(currentMoveInput);
        }

        public void OnSprint(InputValue value) => OnDashEvent?.Invoke();
        public void OnJump(InputValue value) => OnDashEvent?.Invoke();
        public void OnDash(InputValue value) => OnDashEvent?.Invoke();
        public void OnAttack(InputValue value) => OnAttackEvent?.Invoke();
        public void OnParry(InputValue value) => OnParryEvent?.Invoke();
        public void OnCrouch(InputValue value) => OnParryEvent?.Invoke();
        public void OnInteract(InputValue value) => OnInteractEvent?.Invoke();
    }
}
