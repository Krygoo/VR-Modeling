using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushManager : MonoBehaviour
{
    /// <summary>
    /// Nie musi byc public. Typ pędzla:
    /// 0 - Wszystkie bloki wypełnione
    /// 1 - Bloki wypełnione co drugi
    /// 2 - Trzy linie
    /// </summary>
    public int brushType = 0;
    /// <summary>
    /// Nie musi byc public. Tryb pędzla:
    /// 0 - Sześcian
    /// 1 - Kula
    /// </summary>
    public int brushMode = 1;
    public Text brushTypeText;
    public Text brushModeText;

    void Start()
    {
        ChangeBrushModeText();
        ChangeBrushTypeText();
    }


    public void ChangeBrushType()
    {
        if (brushType == 2)
            brushType = 0;
        else brushType++;
    }

    public void ChangeBrushMode()
    {
        if (brushMode == 1)
            brushMode = 0;
        else brushMode++;
    }

    public void ChangeBrushTypeText()
    {
        brushTypeText.text = "Typ pędzla: " + brushType.ToString();
    }

    public void ChangeBrushModeText()
    {
        brushModeText.text = "Tryb pędzla: " + brushMode.ToString();
    }
}
