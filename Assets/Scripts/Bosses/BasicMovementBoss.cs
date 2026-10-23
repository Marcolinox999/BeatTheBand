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
    private enum PtPStyle
    {
        Normal,
        Accelerate,
        Decelerate,
        Arc,
        Sinusoidal
    }
    private MovType currentType;
    [Header("Boss Point to Point")]
    [SerializeField] Transform[] keys;
    int currentKey = 0;
    [Header("Point To Point Style")]
    [SerializeField] private PtPStyle ptPStyle = PtPStyle.Normal;

    [SerializeField] private float arcHeight = 3f;
    [SerializeField] private float sineAmplitude = 1f;
    [SerializeField] private float sineCycles = 2f;
    private Vector3 ptpStart;
    private Vector3 ptpTarget;
    private float ptpElapsed;
    private float ptpDuration;
    private bool ptpInitialized;
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

    public void MovemeventSwitch(int movType, int style = 0)
    {
        switch (movType)
        {
            case 0:
                currentType = MovType.PtP;
                break;
            case 1:
                currentType = MovType.Jmp;
                break;
            case 2:
                currentType = MovType.Sty;
                break;
        }

        switch (style)
        {
            case 0:
                ptPStyle = PtPStyle.Normal;
                break;
            case 1:
                ptPStyle = PtPStyle.Accelerate;
                break;
            case 2:
                ptPStyle = PtPStyle.Decelerate;
                break;
            case 3:
                ptPStyle = PtPStyle.Arc;
                break;
            case 4:
                ptPStyle = PtPStyle.Sinusoidal;
                break;
        }
    }

    private void Stay()
    {
        
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
        //Lo voy a documentar porque es largo
        if (keys == null || keys.Length == 0) return;

    // Empieza el trayecto 
    if (!ptpInitialized)
    {
        currentKey = Mathf.Clamp(currentKey, 0, keys.Length - 1);

        ptpStart = transform.position;
        ptpTarget = keys[currentKey].position;

        float distance = Vector3.Distance(ptpStart, ptpTarget);

        if (distance < 0.01f)
        {
            NextKey();
            return;
        }
        ptpDuration = distance / Mathf.Max(bossSpeed, 0.01f);
        ptpElapsed = 0f;
        ptpInitialized = true;
    }

    ptpElapsed += Time.deltaTime;

    float t = Mathf.Clamp01(ptpElapsed / ptpDuration);

    //Aqui esta la magia, el progreso va a ser la forma en la que cada funcion asume la variable y la transforma en un movimiento de distinta forma
    float progress = t;

        switch (ptPStyle) 
        {
        
            case PtPStyle.Normal:
            // simplemente va de un punto a otro
            progress = t;
            break;

        
            case PtPStyle.Accelerate:
            // haciendo t*t hace se vaya haciendo cada vez mas rapido a medida que t sube
            progress = t * t;
            break;

        
            case PtPStyle.Decelerate:
            // empieza todo lo rapido que  puede y luego va bajando 
            progress = 1f - Mathf.Pow(1f - t, 2f);
            break;

        
            case PtPStyle.Arc:
            // va haciendo una especie de avance suave
            progress = t * t * (3f - 2f * t);
            break;

        
            case PtPStyle.Sinusoidal:
            // igual que al principio porq eso se hace despues
            progress = t;
            break;
        }

        Vector3 position = Vector3.Lerp(ptpStart, ptpTarget, progress);

    //Aqui esta la formula del arco
    if (ptPStyle == PtPStyle.Arc)
    { 
        position.y += Mathf.Sin(t * Mathf.PI) * arcHeight;
    }
    // Aquii tambien se hace el movimiento del sinusoidal
    if (ptPStyle == PtPStyle.Sinusoidal)
    {
        Vector3 direction = (ptpTarget - ptpStart).normalized;
        Vector3 perpendicular = new Vector3(-direction.y, direction.x, 0f);

        position += perpendicular * (Mathf.Sin(t * sineCycles * Mathf.PI * 2f) * sineAmplitude);
    }

    transform.position = position;
    
    if (t >= 1f)
    {
        transform.position = ptpTarget;
        NextKey();
    }
}

private void NextKey()
{
    currentKey++;

    if (currentKey >= keys.Length)
        currentKey = 0;

    ptpInitialized = false;
}
}
