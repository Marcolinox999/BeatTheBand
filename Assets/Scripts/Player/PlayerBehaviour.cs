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
        [SerializeField] private float speed = 5f;
        [SerializeField] private bool isDead = false;

        [Header("Input")]
        private Vector2 _moveInput;
        [SerializeField] private InputActionReference movementInput;
        [SerializeField] private InputActionReference attackInput;
        [SerializeField] private bool pveEnabled = false;

        [Header("OnMap Settings")]
        [SerializeField] private MapPoint currentPoint;
        [SerializeField] private float moveSpeed = 10f;
        private bool _isOnMap = false;
        private Vector3 targetPosition;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            if (currentPoint != null)
            {
                currentPoint.DeactivateLevelUI();
            }
            else
                Debug.Log("No point");
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
            Debug.Log("state == " + _state);
        }

        private void OnDisable()
        {
            movementInput.action.Disable();
            attackInput.action.Disable();
        }

        // Reads input every frame; the physics are applied in FixedUpdate
        private void Movement()
        {
            _moveInput = Vector2.ClampMagnitude(movementInput.action.ReadValue<Vector2>(), 1f);
            animator.SetFloat("Speed", _moveInput.sqrMagnitude > 0f ? 1 : 0);

            if (isDead) _state = States.Dead;
        }

        private void FixedUpdate()
        {
            if (rb.bodyType == RigidbodyType2D.Static) return;
            if (_state == States.Walking)
            {
                rb.linearVelocity = _moveInput * speed;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
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
                    if (moveInput.x > 0) { nextPoint = GetValidPoint(currentPoint.right); }
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

            // Interaction point requested by Isaac
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                if (currentPoint.isLevel && !_isOnMap && !string.IsNullOrEmpty(currentPoint.sceneToLoad) && currentPoint.isFinal)
                {
                    if (GameManager.Instance.beatenLevels[1] && GameManager.Instance.beatenLevels[2] &&
                        GameManager.Instance.beatenLevels[0])
                    {
                        StartCoroutine(LoadLevel(currentPoint.sceneToLoad, 2f));
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    if (currentPoint.isLevel && !_isOnMap && !string.IsNullOrEmpty(currentPoint.sceneToLoad))
                    {
                        StartCoroutine(LoadLevel(currentPoint.sceneToLoad, 2f));
                    }
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

        private IEnumerator LoadLevel(string sceneName, float delayTime)
        {
            _isOnMap = true;

            // TODO: add visual effects before loading the level

            yield return new WaitForSeconds(delayTime);

            GameManager.Instance.LoadScene(sceneName);
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
                case States.OnMap:
                    OnMap();
                    break;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.PauseGameIResumeGame();
            }

            if (attackInput.action.WasPressedThisFrame() && pveEnabled && GameManager.Instance.CurrentState == GameManager.GameState.Playing)
            {
                _weaponManager.Attack();
            }
        }
    }
}