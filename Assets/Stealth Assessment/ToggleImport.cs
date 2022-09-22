using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleImport : MonoBehaviour
{
    //toggle
    public Toggle toggle;
    private ObjectsToObserve saver;
    private GameManager GM;
    // Start is called before the first frame update
    void Start()
    {
        saver = FindObjectOfType<ObjectsToObserve>();
        GM = FindObjectOfType<GameManager>();
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(delegate {
            ChangeToggle(toggle);
        });
    }

    void ChangeToggle(Toggle toggle)
    {
        saver.startImport(toggle.isOn);
        saver.stopImport(toggle.isOn);
    }
}
