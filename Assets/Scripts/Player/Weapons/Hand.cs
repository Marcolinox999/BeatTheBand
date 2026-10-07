using Unity.Cinemachine;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private GameObject Cursor;
    private Vector2 Direction;
    private float rotationSpeed = 5f;
    private float smoothAngle;
    public bool canMove;
    private CinemachineImpulseSource impulseSource;

    private float angle;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor = GameObject.Find("Cursor");
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            Direction = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
            smoothAngle = Mathf.LerpAngle(transform.rotation.eulerAngles.z, angle, Time.deltaTime * rotationSpeed);
            transform.rotation = Quaternion.AngleAxis(smoothAngle, Vector3.forward);
        }
    }
    
}