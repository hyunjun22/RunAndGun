using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    void OnEnable()
    {
        // 풀에서 다시 꺼낼 때 항상 첫 프레임부터 재생합니다.
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
        }
    }

    // 애니메이션 이펙트와 연결 Connection Animation Event
    public void EndExplosion()
    {
        gameObject.SetActive(false);
    }
}
