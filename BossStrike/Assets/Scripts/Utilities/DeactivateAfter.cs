using System.Collections;
using UnityEngine;

public class DeactivateAfter : MonoBehaviour
{
    [SerializeField]
    private float _deactivateDelayInSeconds = 5f;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(_deactivateDelayInSeconds);
        gameObject.SetActive(false);
    }
}
