using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCutscene : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        if (!GameInput.Instance) { return;  }
        GameInput.Instance.OnGravcastPerformed += Instance_OnGravcastPerformed;
    }

    private void Instance_OnGravcastPerformed(object sender, System.EventArgs e) {
        GetComponent<Loader>().LoadMainMenu();
    }


    private void OnDestroy() {
        if (!GameInput.Instance) { return; }
        GameInput.Instance.OnSpellcastPerformed -= Instance_OnGravcastPerformed;


    }
}
