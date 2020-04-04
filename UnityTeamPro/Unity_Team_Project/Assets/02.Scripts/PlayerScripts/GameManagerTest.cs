using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerTest : MonoBehaviour
{
    //마우스 포인터로 사용할 텍스쳐
    public Texture2D cursorTexture;
    //텍스쳐의 중심을 마우스좌표로 할 것인지에 대한 변수
    public bool hotSpotIsCenter = false;
    //텍스쳐의 어느부분을 마우스좌표로 할 것인지 텍스쳐의 좌표 받음.
    public Vector2 adjustHotSpot = Vector2.zero;
    //내부에서 사용할 필드 선언.
    private Vector2 hotSpot;

    private void Start()
    {
        //cursorTexture.
        //
        //StartCoroutine(MyCursor());
    }

    IEnumerator MyCursor()
    {
        //모든 렌더링이 완료 될 때까지 대기할테니 렌더링이 완료되면 
        //깨워달라고 유니티에 부탁하고 대기합니다.

        yield return new WaitForEndOfFrame();

        //텍스쳐의 중심을 마우스의 좌표로 사용하는 경우
        //텍스쳐의 폭과 높이의 1/2을 HotSpot 좌표로 입력받은 것을 사용합니다.

        if(hotSpotIsCenter)
        {
            hotSpot.x = cursorTexture.width / 2;
            hotSpot.y = cursorTexture.height / 2;
        }
        else
        {
            //중심으로 사용하지 않을 경우 AdjustHost로 입력받은 것을 사용합니다.
            hotSpot = adjustHotSpot;
        }

        //세팅이 끝났다면 새로운마우스 커서를 표시합니다.
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }
}
