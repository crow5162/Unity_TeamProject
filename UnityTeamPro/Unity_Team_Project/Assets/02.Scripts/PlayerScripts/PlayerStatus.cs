using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private const string enemyBulletTag = "E_BULLET";
    private const string enemyTag = "ENEMY";

    private Animator anim;
    private readonly int hashDeath = Animator.StringToHash("isDead");

    private float maxHp = 250.0f;
    private float currentHp;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        currentHp = maxHp;
    }

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == enemyBulletTag)
        {
            currentHp -= 20.0f;
            Destroy(coll.gameObject);

            if(currentHp <= 0)
            {
                OnPlayerDie();
            }
        }
    }

    void OnPlayerDie()
    {
        anim.SetBool(hashDeath, true);
    }
}
