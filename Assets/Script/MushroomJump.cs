//MushroomJump.cs
using UnityEngine;

public class MushroomJump : MonoBehaviour
{
    public float mushroomJumpPower;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.mushroomJump(mushroomJumpPower);
                Debug.Log("MushroomJump Ãæµ¹ °¨ÁöµÊ!");
            }
        }
    }
}
