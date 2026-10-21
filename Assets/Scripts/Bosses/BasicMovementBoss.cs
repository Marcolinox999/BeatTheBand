using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BasicMovementBoss : MonoBehaviour
{
    private enum MovType
    {
        PtP,
        Jmp,
        Sty
    }
    private MovType currentType;
    [Header("Boss Point to Point")]
    [SerializeField] Transform[] keys;
    int currentKey = 0;
    [Header("Boss Jumps")]
    [SerializeField] private float jumpRadius = 4f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpDuration = 0.5f;
    [SerializeField] private float jumpInterval = 2f;
    [SerializeField] private float checkRadius = 0.3f;
    [SerializeField] private LayerMask obstacleLayers;
    private bool isJumping;
    private float nextJumpTime;
    [Header("Boss Speed")]
    public float bossSpeed;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            currentType = MovType.PtP;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            currentType = MovType.Jmp;
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            currentType = MovType.Sty;
        }
            
        switch (currentType)
        {
            case MovType.PtP:
                PointToPoint();
                break;
            case MovType.Jmp:
                Jump();
                break;
            case MovType.Sty:
                Stay();
                break;
            
        }
    }

    private void Stay()
    {
        throw new NotImplementedException();
    }

    private void Jump()
    {
        if (isJumping || Time.time < nextJumpTime)
            return;

        if (TryFindJumpPoint(out Vector2 destination))
        {
            StartCoroutine(JumpTo(destination));
            nextJumpTime = Time.time + jumpInterval;
        }
        else
        {
            nextJumpTime = Time.time + 0.5f;
        }
    }
    private bool TryFindJumpPoint(out Vector2 destination)
    {
        Vector2 origin = transform.position;
        for (int i = 0; i < 20; i++)
        {
            Vector2 direction = Random.insideUnitCircle.normalized;
            if (direction == Vector2.zero) continue;
            float distance = Random.Range(1f, jumpRadius);
            Vector2 candidate = origin + direction * distance;
            Collider2D obstacle = Physics2D.OverlapCircle(
                candidate,
                checkRadius,
                obstacleLayers
            );
            if (obstacle != null) continue;
            Vector2 path = candidate - origin;
            RaycastHit2D hit = Physics2D.CircleCast(origin, checkRadius, path.normalized,path.magnitude,obstacleLayers);
            if (hit.collider != null) continue;
            destination = candidate; 
            return true;
        }
        destination = origin;
        return false;
    }

    private IEnumerator JumpTo(Vector2 destination)
    { 
        isJumping = true;
        Vector2 start = transform.position;
        float elapsed = 0f;
        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / jumpDuration);
            Vector2 position = Vector2.Lerp(start, destination, t);
            position.y += Mathf.Sin(t * Mathf.PI) * jumpHeight;
            transform.position = position;
            yield return null;
        }
        transform.position = destination;
        isJumping = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, jumpRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }

    private void PointToPoint()
    {
        if (Vector3.Distance(transform.position, keys[currentKey].position) < 0.01f)
        { currentKey++;
            if (currentKey >= keys.Length)
                currentKey = 0;
        }
        transform.position = Vector3.MoveTowards(transform.position, keys[currentKey].position,bossSpeed *Time.deltaTime
        );
    }
}
