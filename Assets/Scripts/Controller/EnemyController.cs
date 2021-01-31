using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 5f;

    private void Update()
    {
        Vector3 playerPos = PlayerController.self.position;
        playerPos.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, playerPos, Time.deltaTime * speed);

        if (Vector3.Distance(playerPos, transform.position) < 3f)
        {
            DirectorController.instance.GameOver();
        }
    }
}
