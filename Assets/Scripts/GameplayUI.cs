using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameplayUI : MonoBehaviour
{

    public static GameplayUI Instance { get; private set; }

    [SerializeField] GameObject gravityArrow;

    private void Awake() {
        if (Instance) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGravityArrowDirection(Vector2 dir) {
        gravityArrow.transform.right = dir;
    }
}
