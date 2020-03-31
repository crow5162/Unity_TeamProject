using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    private float testHp = 50.0f;
    private float currentHp;

    private MeshRenderer meshRenderer;
    private CapsuleCollider capCollider;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        capCollider = GetComponent<CapsuleCollider>();
    }

    private void OnEnable()
    {
        currentHp = testHp;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == "BULLET")
        {
            currentHp -= 10.0f;

            Destroy(coll.gameObject);

            if (currentHp <= 0)
            {
                StartCoroutine(SpawnEnemy());
            }
        }
    }

    //Player SniperRifle Hitting
    void OnDamage(object[] _infos)
    {
        currentHp -= (float) _infos[1];

        if(currentHp <= 0)
        {
            StartCoroutine(SpawnEnemy());
        }
    }

    IEnumerator SpawnEnemy()
    {

        capCollider.enabled = false;
        meshRenderer.enabled = false;

        yield return  new WaitForSeconds(3.0f);

        capCollider.enabled = true;
        meshRenderer.enabled = true;
        currentHp = testHp;
    }
}
