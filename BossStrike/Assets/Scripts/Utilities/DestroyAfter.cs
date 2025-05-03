using System.Collections;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    [SerializeField]
    private float _destroyDelayInSeconds = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(_destroyDelayInSeconds);
        Destroy(gameObject);
    }
}
