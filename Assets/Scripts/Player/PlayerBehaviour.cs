using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
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
        private bool pveEnabled = false;

        [Header("OnMap Settings (me vas a matar Marco lo se")]
        [SerializeField] private MapPoint currentPoint;
        [SerializeField] private float moveSpeed = 10f;
        private bool _isOnMap = false;
        private Vector3 targetPosition;
        
        private void OnEnable()
        {
            currentPoint.DeactivateLevelUI();
            movementInput.action.Enable();
            if (SceneManager.GetActiveScene().name == "Level Hub")
            {
                _isOnMap = false;
                _state = States.OnMap;
                pveEnabled = false; 
            }
            else
            {
                _state = States.Walking;
                pveEnabled = true;
                attackInput.action.Enable();
            }
            Debug.Log("state == " +  _state);
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
            Debug.Log("movementInput chose");

            if (moveInput.sqrMagnitude > 0.1f)
            {
                MapPoint nextPoint = null;
                if (Math.Abs(moveInput.y) > Math.Abs(moveInput.x))
                {
                    if (moveInput.y > 0)
                    {
                        nextPoint = GetValidPoint(currentPoint.up);
                        Debug.Log("moves up");
                    }

                    else nextPoint = GetValidPoint(currentPoint.down); Debug.Log("moves down");
                }
                else
                {
                    if (moveInput.x > 0) {nextPoint = GetValidPoint(currentPoint.right); Debug.Log("moves right");}
                    else nextPoint = GetValidPoint(currentPoint.left); Debug.Log("moves left");
                }

                if (nextPoint != null)
                {
                    if (currentPoint.isLevel)
                    {
                        currentPoint.DeactivateLevelUI();
                    }
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
            if (points != null && points.Length > 0)
            {
                Debug.Log("Punto válido encontrado: " + points[0].name);
                return points[0];
            }
    
            Debug.LogWarning("No hay ningún MapPoint asignado en esta dirección.");
            return null;
        }

        private IEnumerator SmoothMoveToTarget()
        {
            _isOnMap = true;
            currentPoint.DeactivateLevelUI();
            
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = targetPosition;
            _isOnMap = false;
            
            if (currentPoint.isLevel)
            {
                currentPoint.ActivateLevelUI();
            }
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

           /* if (attackInput.action.WasPressedThisFrame() && pveEnabled) //MARCO HOLA lo de pve enabled es para que cuando esté en el mapa no pueda usar las armas
            {
                _weaponManager.Attack();
            }
            */
        }
        
    }
    
}
