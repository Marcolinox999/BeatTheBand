using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        private enum States
        {
            Walking,
            Dead,
            
        }

        private States _state;
        
        [Header("Values")]
        [SerializeField] private float speed;
        [SerializeField] private bool isDead = false;
        
        [Header("Input")]
        private float hInput;
        private float vInput;
        [SerializeField] private InputActionReference movementInput;
        [SerializeField] private InputActionReference attackInput;
        [SerializeField] private bool pveEnabled; //esta variable te la explico abajo donde se hace el check para el ataque
        
        private void OnEnable()
        {
            movementInput.action.Enable();
            attackInput.action.Enable();
        }

        private void OnDisable()
        {
            movementInput.action.Disable();
            attackInput.action.Disable();

        }

        private void Movement()
        {
            Vector2 moveInput =  movementInput.action.ReadValue<Vector2>();
            moveInput = Vector2.ClampMagnitude(moveInput, 1);
            hInput = moveInput.x;
            vInput = moveInput.y;
            transform.Translate(hInput * speed, vInput * speed, 0);
            if (isDead) _state =  States.Dead;
        }

        private void Update()
        {
            switch (_state)
            {
                case States.Walking:
                    Movement();
                    break;
                case States.Dead:
                    break;
            }

            if (attackInput.action.WasPressedThisFrame() && pveEnabled == true) //MARCO HOLA lo de pve enabled es para que cuando esté en el mapa no pueda usar las armas
            {
                Attack();
            }
        }

        private void Attack()
        {
            switch (BeatManager.instance.PrecisionCheck())
            {
                case BeatManager.Score.Missed:
                    break;
                case BeatManager.Score.Ok:
                    Debug.Log("Ok");
                    break;
                case BeatManager.Score.Perfect:
                    Debug.Log("Perfect");
                    break;
                
            }
            
        }
    }
    
}
