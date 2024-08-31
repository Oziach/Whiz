using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] float lifetime;
    private float lifeLeft;

    private void Awake() {
        lifeLeft = lifetime;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifeLeft -= Time.deltaTime;
        if(lifeLeft <= 0) {
            Destroy(gameObject);
        }
    }
}
