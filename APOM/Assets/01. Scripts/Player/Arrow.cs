using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [HideInInspector] public Vector3 target;

    private Vector3 dir;
    float angle;
    Quaternion targetRot;
    float angleDifferent;

    private LayerMask collisionMask;

    void Awake()
    {
        collisionMask =
            (1 << LayerMask.NameToLayer("Enemy")) |
            (1 << LayerMask.NameToLayer("Boss")) |
            (1 << LayerMask.NameToLayer("Ground")) |
            (1 << LayerMask.NameToLayer("Obstacle"));
    }

    void Start()
    {
        StartCoroutine(DisappearAfterTime(5f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * 50.0f * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & collisionMask) != 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DisappearAfterTime(float _time)
    {
        yield return new WaitForSeconds(_time);

        Destroy(gameObject);

        // gameObject.SetActive(false);
    }
}
