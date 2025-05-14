using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 targetPosition;
    private bool isFired = false;

    [SerializeField] private GameObject OrbEff;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject flameThrower;
    public void Fire(Vector3 target)
    {
        targetPosition = target;
        isFired = true;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void Update()
    {
        if (!isFired) return;

        // 목표 방향으로 이동
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // 도착 확인 (optional)
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            OnHit();
        }
    }

    private void OnHit()
    {
        OrbEff.SetActive(false);
        explosionEffect.SetActive(true);
        flameThrower.SetActive(true);

        StartCoroutine(DestroyEff());
    }

    IEnumerator DestroyEff()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
