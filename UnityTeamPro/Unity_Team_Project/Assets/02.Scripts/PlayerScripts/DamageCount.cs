using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using TMPro;

public class DamageCount : MonoBehaviour
{
    private float moveSpeed;
    private float alphaSpeed;
    private float destroyTime;
    TextMeshPro text;
    private Color alpha;
    public Color criticalColor;
    public float damage = 0;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 1.5f;
        alphaSpeed = 3.5f;
        destroyTime = 1.0f;

        text = GetComponent<TextMeshPro>();
        alpha = text.color;
        text.color = criticalColor;
        text.SetText(damage.ToString());
        Invoke("DestroyObject", destroyTime);
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0)); //Text position
        //alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);  //Damage Alpha Value
        //text.color = alpha;
    }

    private void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
