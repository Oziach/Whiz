using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{

    [SerializeField] int level;
    [SerializeField] TextMeshProUGUI levelText;

    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance && level <= GameManager.Instance.GetHighestLevelReached()) {
            levelText.text = level.ToString();

            button = GetComponent<Button>();
            button.onClick.AddListener(LoadRespectiveLevel);

        }
        else {
            Destroy(gameObject);
        }

    }

    public void LoadRespectiveLevel() {
        if (GameManager.Instance) {
            GameManager.Instance.LoadLevel(level);
        }
    }

   
}
