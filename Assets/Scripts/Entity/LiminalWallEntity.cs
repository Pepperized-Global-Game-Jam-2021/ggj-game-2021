using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiminalWallEntity : MonoBehaviour
{
    DirectorController director;
    MeshCollider col;

    public Vector3 teleportTarget;

    private void Start()
    {
        director = DirectorController.instance;
        col = GetComponent<MeshCollider>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position + teleportTarget, 0.5f);
    }
}
