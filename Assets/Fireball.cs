using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private GameObject _hit;
    private bool _isActive = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_isActive)
        {
            rb.velocity = new Vector2(20, 0);
        }
        else
        {
            rb.velocity = Vector2.zero; 
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            collision.transform.parent.gameObject.SetActive(false);
            //gameObject.SetActive(false);
            _isActive = false;
            SoundManager.Instance.MakeRockHitSound();
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            _hit.SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            //gameObject.SetActive(false);
            SoundManager.Instance.MakeRockHitSound();
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            _isActive = false;
            _hit.SetActive(true);
        }
    }


    
}
