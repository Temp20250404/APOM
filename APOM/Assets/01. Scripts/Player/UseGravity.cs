using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseGravity : MonoBehaviour
{
    private float gravity = -9.8f;
    private Vector3 velocity;
    private int groundMask;
    public bool isGrounded { get; private set; }
    private Vector3 castStartPosition = new Vector3(0f, 1f, 0f);
    [SerializeField] private float groundCheckRadius = 0.4f;
    [SerializeField] private float groundCheckDistance = 0.6f;

    // Start is called before the first frame update
    void Start()
    {
        groundMask = (1 << LayerMask.NameToLayer("Ground"));
    }

    private void Update()
    {
        Gravity();
    }

    private void FixedUpdate()
    {
        CheckGrounded();
    }

    private void Gravity()
    {
        if (!isGrounded)
        {
            // 바닥에 닿아 있지 않을 때 중력 적용
            velocity.y += gravity * Time.deltaTime;
        }
        // 속도를 위치에 적용
        transform.position += velocity * Time.deltaTime;
    }

    private void CheckGrounded()
    {
        RaycastHit hitInfo;
        Vector3 origin = transform.position + castStartPosition;

        // SphereCast: 구 반경을 가진 광선을 아래로 쏘아서 GroundMask만 감지
        isGrounded = Physics.SphereCast(origin, groundCheckRadius, Vector3.down, out hitInfo, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0f)
            velocity.y = 0f;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position + castStartPosition;

        // 착지 여부에 따라 색 변경
        Gizmos.color = isGrounded ? Color.green : Color.red;

        // 시작 구체
        Gizmos.DrawWireSphere(origin, groundCheckRadius);

        // 끝 구체
        Vector3 endCenter = origin + Vector3.down * groundCheckDistance;
        Gizmos.DrawWireSphere(endCenter, groundCheckRadius);

        // 두 구체를 연결하는 선
        Gizmos.DrawLine(origin + Vector3.right * groundCheckRadius, endCenter + Vector3.right * groundCheckRadius);
        Gizmos.DrawLine(origin + Vector3.left * groundCheckRadius, endCenter + Vector3.left * groundCheckRadius);
        Gizmos.DrawLine(origin + Vector3.forward * groundCheckRadius, endCenter + Vector3.forward * groundCheckRadius);
        Gizmos.DrawLine(origin + Vector3.back * groundCheckRadius, endCenter + Vector3.back * groundCheckRadius);
    }
}
