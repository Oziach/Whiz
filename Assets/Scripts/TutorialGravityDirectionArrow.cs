using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGravityDirectionArrow : MonoBehaviour
{

    [SerializeField] PlayerCasting playerCasting;
    [SerializeField] GameObject arrow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerCasting) { return;  }

        arrow.transform.right = playerCasting.RotationToDirection();
    }
}
