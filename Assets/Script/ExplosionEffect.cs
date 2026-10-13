using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    // 애니메이션 이펙트와 연결 Connection Animation Event
    public void EndExplosion()
    {
        gameObject.SetActive(false);
    }
}
