using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 3f;
    public float lifetime = 2f;
    public float distance;
    public LayerMask whatIsSolid;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyProjectile", lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.right, distance, whatIsSolid);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Player2"))
            {
                Debug.Log("Player damage");
            }
            Destroy(gameObject);
        }


        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }

}
