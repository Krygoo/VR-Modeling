using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SaveLoadUI : MonoBehaviour
{
    bool shouldSave;

    public void CheckSaves()
    {
        Button[] buttons = transform.GetChild(1).GetChild(0).GetComponentsInChildren<Button>();
        for (int i=0;i<buttons.Length;i++)
        {
            if (!File.Exists(Application.persistentDataPath + "/saves/" + (i + 1) + ".save"))
            {
                ColorBlock cb = buttons[i].colors;
                cb.normalColor = new Color(1f, 1f, 1f, 0.5f);
                buttons[i].colors = cb;
            }
        }
    }

    public void SaveOrLoad(int index)
    {
        SaveManager saveManager = FindObjectOfType<SaveManager>();
        if (shouldSave)
            saveManager.Save(index);
        else if (File.Exists(Application.persistentDataPath + "/saves/" + index + ".save"))
            saveManager.Load(index);
    }

    public void SetShouldSave(bool value)
    {
        shouldSave = value;
    }

    public void LoadNextScene()
    {
        SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
        sceneLoader.LoadNextScene();
    }

    public void LoadSavedScene(int index)
    {
        SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
        sceneLoader.LoadSavedScene(index);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
