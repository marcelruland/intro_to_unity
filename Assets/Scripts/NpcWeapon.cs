using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcWeapon : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(DestroyAfterNSeconds(5f));
        
        IEnumerator DestroyAfterNSeconds(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }
        
    }
}
