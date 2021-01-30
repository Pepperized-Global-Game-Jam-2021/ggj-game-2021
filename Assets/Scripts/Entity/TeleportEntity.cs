using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEntity : MonoBehaviour
{
    public Vector3 TeleportPosition;
    public bool IsDirectional = false;
    public PortalDirections portalDirections;
    
    [System.Serializable]
    public class PortalDirections
    {
        public bool PosX;
        public bool NegX;
        public bool PosZ;
        public bool NegZ;
    }

    public bool CheckDirection(Vector3 playerPos)
    {
        if (!IsDirectional) return true;
        else
        {
            Vector3 diff = transform.position - playerPos;
            //Debug.Log(diff.normalized);

            bool isPosX = diff.x > 0;
            bool isPosZ = diff.z > 0;

            Debug.Log($"isPosX {isPosX}, isPosZ {isPosZ}");

            bool xTest = (isPosX && portalDirections.PosX) || (!isPosX && portalDirections.NegX);
            bool zTest = (isPosZ && portalDirections.PosZ) || (!isPosZ && portalDirections.NegZ);

            Debug.Log($"xTest {xTest}, zTest {zTest}");

            return xTest || zTest;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(TeleportPosition, 0.5f);
    }
}
