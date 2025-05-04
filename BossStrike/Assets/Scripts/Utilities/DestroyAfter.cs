using System.Collections;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    [SerializeField]
    private float _destroyDelayInSeconds = 1f;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(_destroyDelayInSeconds);
        Destroy(gameObject);
    }
}
