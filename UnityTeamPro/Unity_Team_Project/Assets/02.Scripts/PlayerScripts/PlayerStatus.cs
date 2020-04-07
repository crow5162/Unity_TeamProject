using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerControll))]

public class PlayerStatus : MonoBehaviour
{
    private const string enemyBulletTag = "E_BULLET";
    private const string enemyTag = "ENEMY";

    private Animator anim;
    private readonly int hashDeath = Animator.StringToHash("isDead");

    private float maxHp = 250.0f;
    private float currentHp;

    public Image hpBar;
    public Text hpInfo;
    private readonly Color initColor = new Vector4(0, 1.0f, 0.0f, 1.0f);
    private Color currentColor;

    private PlayerControll pControll;


    // Start is called before the first frame update
    void Start()
    {
        pControll = GetComponent<PlayerControll>();
        anim = GetComponent<Animator>();
        currentHp = maxHp;

        hpBar.color = initColor;
        currentColor = initColor;

        UpdateHpInfo();
    }

    void Update()
    {
        if(currentHp <= 0 )
        {
            currentHp = 0;
            UpdateHpInfo();
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == enemyBulletTag)
        {
            currentHp -= 40.0f;
            Destroy(coll.gameObject);

            //생명게이지 크기및 수치 변경 함수 호출
            DisplayHpBar();
            //피격시 마다 체력 Text Update
            UpdateHpInfo();

            if (currentHp <= 0)
            {
                pControll.isDead = true;
                OnPlayerDie();
            }
        }
    }

    void OnPlayerDie()
    {
        anim.SetTrigger(hashDeath);
    }

    void DisplayHpBar()
    {
        //생명수치가 절반 일 때 녹색에서 노란색으로 변경
        if((currentHp / maxHp) > 0.5f)
        {
            currentColor.r = (1 - (currentHp / maxHp)) * 2;
        }
        else //노란색에서 빨간색으로 으로 변경
        {
            currentColor.g = (currentHp / maxHp) * 2.0f;
        }

        //hpBar의 색상과 크기를 변경합니다.
        hpBar.color = currentColor;
        hpBar.fillAmount = (currentHp / maxHp);
    }

    void UpdateHpInfo()
    {
        hpInfo.text = string.Format("{0} / {1}", currentHp, maxHp);
    }
}
