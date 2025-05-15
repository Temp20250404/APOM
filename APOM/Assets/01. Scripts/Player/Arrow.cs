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

    void Start()
    {
        StartCoroutine(DisappearAfterTime(5f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * 50.0f * Time.deltaTime;
    }

    private IEnumerator DisappearAfterTime(float _time)
    {
        yield return new WaitForSeconds(_time);

        Destroy(gameObject);

        // gameObject.SetActive(false);
    }
}
