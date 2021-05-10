using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartGameUIController : MonoBehaviour
{

    [SerializeField]
    TMP_Text levelValueText;

    // Start is called before the first frame update
    void Start()
    {
        int currentLevel = LevelController.Instance.GetLevelNumber();
        levelValueText.text = currentLevel.ToString();
    }

}
