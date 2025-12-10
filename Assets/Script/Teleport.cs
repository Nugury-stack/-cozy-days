using System.Collections;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform linkedPortal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어의 쿨타임 체크용 컴포넌트 가져오기
            PlayerTeleportCooldown cooldown = other.GetComponent<PlayerTeleportCooldown>();
            if (cooldown == null)
            {
                // 없다면 자동으로 추가
                cooldown = other.gameObject.AddComponent<PlayerTeleportCooldown>();
            }

            if (!cooldown.IsOnCooldown)
            {
                StartCoroutine(TeleportPlayer(other.transform, cooldown));
            }
        }
    }

    private IEnumerator TeleportPlayer(Transform player, PlayerTeleportCooldown cooldown)
    {
        cooldown.IsOnCooldown = true;

        // 위치 이동
        player.position = linkedPortal.position;

        // 약간의 시간 후 쿨타임 해제 (예: 1초)
        yield return new WaitForSeconds(4f);

        cooldown.IsOnCooldown = false;
    }
}

// 플레이어 쿨타임 상태를 저장하는 내부 컴포넌트
public class PlayerTeleportCooldown : MonoBehaviour
{
    public bool IsOnCooldown = false;
}
