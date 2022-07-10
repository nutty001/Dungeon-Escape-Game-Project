using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColletableController : MonoBehaviour
{
    // public List gems = new List();
    public SpriteRenderer spriteRenderer;

    [SerializeField] private AudioSource CollectionSoundEffect;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // AddCollectable(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.tag == "Player")
        {
            Collected();
        }    
    }

    void Collected()
    {
        spriteRenderer.enabled = false;
        Destroy(gameObject);
        CollectionSoundEffect.Play();
    }

    // public void AddCollectable(ColletableController colletablecontroller)
    // {
    //     gems.Add(colletablecontroller);
    // }
}
