using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] ObjectPoolingManager ObjPoolManager;
    [SerializeField] Transform Muzzle;
    [SerializeField] PlayerStats playerStats;
    [SerializeField, Range(0f, 90f)] float angleBetweenBullets = 15f;
    [SerializeField, Range(0f, 180f)] float maxSpreadAngle = 90f;

    float nextAttackTime = 0f;

    void Update()
    {
        if(Time.timeScale == 0) return;

        GunRotation();

        if (Mouse.current.leftButton.isPressed && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + 1f / playerStats.AttackSpeed;

            Shoot();
        }        
    }

    private void GunRotation()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f;

        Vector2 direction = (mouseWorldPosition - transform.position).normalized;
        transform.right = direction;

        if(direction.x < 0)
        {
            spriteRenderer.flipY = true;
        }
        else
        {
            spriteRenderer.flipY = false;
        }
    }

    private void Shoot()
    {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f;

        // 총알 발사 방향 계산
        Vector2 direction = (mouseWorldPosition - transform.position).normalized;
        FireVolley(direction);
    }

    private void FireVolley(Vector2 direction)
    {
        direction = direction.sqrMagnitude > 0f ? direction.normalized : (Vector2)transform.right;
        int count = playerStats.ProjectileCount;
        float spread = Mathf.Min(angleBetweenBullets * (count - 1), maxSpreadAngle);
        float step = count > 1 ? spread / (count - 1) : 0f;

        // 한 프레임에 조준 방향을 중심으로 대칭 발사합니다.
        for (int i = 0; i < count; i++)
        {
            float angle = -spread * 0.5f + step * i;
            Quaternion offset = Quaternion.Euler(0f, 0f, angle);
            Vector2 shotDirection = offset * direction;
            GameObject bullet = ObjPoolManager.GetBullet();
            if (bullet == null) return;
            bullet.transform.position = Muzzle.position;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f,
                Mathf.Atan2(shotDirection.y, shotDirection.x) * Mathf.Rad2Deg);
            bullet.SetActive(true);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = shotDirection * playerStats.BulletSpeed;
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.SetDamage(playerStats.Damage);
            bulletScript.SetPierceCount(playerStats.PierceCount);
        }
    }
}
