using UnityEngine;
using UnityEngine.InputSystem;


public class AimLine : MonoBehaviour
{
    private LineRenderer lineRen;

    void Start()
    {
        lineRen = GetComponent<LineRenderer>();
        lineRen.positionCount = 2;

        // 선 굵기
        lineRen.startWidth = 0.1f;
        lineRen.endWidth = 0.1f;

        // 선 색상
        lineRen.startColor = Color.red;
        lineRen.endColor = Color.red;
    }

    void Update()
    {
        if(Time.timeScale == 0) return;

        OnDrawLine();
    }

    void OnDrawLine()
    {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue(); // 이 변수에 현재 마우스 좌표를 읽게한다.

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition); // 카메라에 비교했을 때 좌표를 다시 읽는다.
        mouseWorldPosition.z = 0f; // z 좌표는 고정

        // 플레이어와 마우스 사이의 거리를 계산하고, 그 거리를 1로 정규화하여 시작점을 설정
        Vector3 distanceOne = mouseWorldPosition - transform.position;
        distanceOne = distanceOne.normalized * 1f;

        distanceOne += transform.position;

        lineRen.SetPosition(0, distanceOne);
        lineRen.SetPosition(1, mouseWorldPosition);
    }
}
