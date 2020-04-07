using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    private Camera uiCamera;
    private Canvas canvas;
    //부모 
    private RectTransform rectParent;
    //자식
    private RectTransform rectHp;
    //HpBar 이미지의 위치를 조절할 오프셋   
    [HideInInspector] public Vector3 offset = Vector3.zero;
    //추적할 대상의 Transform 컴포넌트 
    [HideInInspector] public Transform targetTr;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = this.gameObject.GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        //월드 좌표를 스크린의 좌표로 전환
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);
        //카메라의 뒷쪽 영역 ( 180도 회전) 일 때 좌푯값 보정
        if(screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }
        //RectTransform 좌푯삾을 전달받을 변수 
        var localPos = Vector2.zero;
        //스크린 좌표를 RectTransform 기준의 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);
        //생명 게이지 이미지의 위치를 변경 
        rectHp.localPosition = localPos;
    }
}
