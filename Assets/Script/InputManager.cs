using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    

    [SerializeField] Player player;
    [SerializeField] LayerMask tileLayer;

    private void Update()
    {
        PlayerInput();
    }

    void PlayerInput()
    {
        if (Time.timeScale == 0) return;
        // Move
        Vector2 dir = Vector2.zero;

        if (Input.GetKey(KeyCode.A))
        {
            if (Physics2D.Raycast(player.transform.position, Vector2.left, 0.6f, tileLayer))
            {
                player.Focusing(FocusDir.Left);
            }
            else
            {                
                player.FocusClear();
            }
            dir.x -= 1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (Physics2D.Raycast(player.transform.position, Vector2.right, 0.6f, tileLayer))
            {
                player.Focusing(FocusDir.Right);
            }
            else
            {                
                player.FocusClear();
            }
            dir.x += 1f;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            player.Jump();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            player.Focusing(FocusDir.Down);
        }

        player.MoveDir = dir;

        // Action
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.Mining();
        }

        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    player.Marking();
        //}
    }
}
