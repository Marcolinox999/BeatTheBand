using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        private enum States
        {
            Walking,
            Dead,
            OnMap,
            
        }

        private States _state;

        [Header("References")] [SerializeField]
        private WeaponManager _weaponManager;
        [Header("Values")]
        [SerializeField] private float speed;
        [SerializeField] private bool isDead = false;
        
        [Header("Input")]
        private float hInput;
        private float vInput;
        [SerializeField] private InputActionReference movementInput;
        [SerializeField] private InputActionReference attackInput;
        [SerializeField] private bool pveEnabled; //esta variable te la explico abajo donde se hace el check para el ataque

        [Header("OnMap Settings (me vas a matar Marco lo se")]
        [SerializeField] private MapPoint currentPoint;
        [SerializeField] private float moveSpeed = 10f;
        private bool _isOnMap = false;
        private Vector3 targetPosition;
        
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
        
        #region ON MAP SELECTION MOVEMENT
        
          private void OnMap()
        {
            if (_isOnMap || currentPoint == null) return;
            Vector2 moveInput = movementInput.action.ReadValue<Vector2>();

            if (moveInput.sqrMagnitude > 0.1f)
            {
                MapPoint nextPoint = null;
                if (Math.Abs(moveInput.y) > Math.Abs(moveInput.x))
                {
                    if (moveInput.y > 0) nextPoint = GetValidPoint(currentPoint.up);
                    else nextPoint = GetValidPoint(currentPoint.down);
                }
                else
                {
                    if (moveInput.x > 0) nextPoint = GetValidPoint(currentPoint.right);
                    else nextPoint = GetValidPoint(currentPoint.left);
                }

                if (nextPoint != null)
                {
                    currentPoint = nextPoint;
                    targetPosition = currentPoint.transform.position;
                    StartCoroutine(SmoothMoveToTarget());
                }
            }

            if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame ||
                Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (currentPoint.isLevel && !string.IsNullOrEmpty(currentPoint.sceneToLoad))
                {
                    SceneManager.LoadScene(currentPoint.sceneToLoad);
                }
            }
        }

        private MapPoint GetValidPoint(MapPoint[] points)
        {
            if (points !=null && points.Length > 0)
                return points[0];
            return null;
        }

        private IEnumerator SmoothMoveToTarget()
        {
            _isOnMap = true;
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }
            transform.position = targetPosition;
            _isOnMap = false;
        }

        #endregion
        
        
        private void Update()
        {
            switch (_state)
            {
                case States.Walking:
                    Movement();
                    break;
                case States.Dead:
                    break;
                case  States.OnMap:
                    OnMap();
                    break;
            }

            if (attackInput.action.WasPressedThisFrame() && pveEnabled) //MARCO HOLA lo de pve enabled es para que cuando esté en el mapa no pueda usar las armas
            {
                _weaponManager.Attack();
            }
        }
        
    }
    
}
