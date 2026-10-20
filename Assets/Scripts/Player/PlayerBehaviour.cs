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
        [SerializeField] private Animator animator;
        private Rigidbody2D rb;
        [Header("Values")]
        [SerializeField] private float speed;
        [SerializeField] private bool isDead = false;
        
        [Header("Input")]
        private float hInput;
        private float vInput;
        [SerializeField] private InputActionReference movementInput;
        [SerializeField] private InputActionReference attackInput;
        [SerializeField] private bool pveEnabled = false;

        [Header("OnMap Settings")]
        [SerializeField] private MapPoint currentPoint;
        [SerializeField] private float moveSpeed = 10f;
        private bool _isOnMap = false;
        private Vector3 targetPosition;
        
        
        private void OnEnable()
        {
            rb = GetComponent<Rigidbody2D>();
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
            animator.SetFloat("Speed", moveInput.x != 0 || moveInput.y != 0?1:0);
            
            rb.AddForce(moveInput * moveSpeed);
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
                    if (moveInput.y > 0)
                    {
                        nextPoint = GetValidPoint(currentPoint.up);
                    }

                    else nextPoint = GetValidPoint(currentPoint.down); 
                }
                else
                {
                    if (moveInput.x > 0) {nextPoint = GetValidPoint(currentPoint.right);}
                    else nextPoint = GetValidPoint(currentPoint.left); 
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

            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                if (currentPoint.isLevel && !_isOnMap && !string.IsNullOrEmpty(currentPoint.sceneToLoad))
                {
                    SceneManager.LoadScene(currentPoint.sceneToLoad);
                }
            }
        }

        private MapPoint GetValidPoint(MapPoint[] points)
        {
            if (points != null && points.Length > 0)
            {
                return points[0];
            }
            
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

            if (attackInput.action.WasPressedThisFrame() && pveEnabled)
            {
                _weaponManager.Attack();
            }
            
        }

      
    }
    
}
