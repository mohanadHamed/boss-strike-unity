using System.Collections;
using UnityEngine;

public class DeactivateAfter : MonoBehaviour
{
    [SerializeField]
    private float _deactivateDelayInSeconds = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(_deactivateDelayInSeconds);
        gameObject.SetActive(false);
    }
}
