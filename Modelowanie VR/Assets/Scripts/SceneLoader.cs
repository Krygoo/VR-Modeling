using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneLoader : MonoBehaviour
{
    bool shouldLoad = false;
    int saveIndex;

    //Awake jest wywoływane jednorazowo na początku działania skryptu. Ustawia ilość scen i sprawia, że obiekt nie jest niszczony przy zmianie sceny (DontDestroyOnLoad)
    private void Awake()
    {
        int numberOfObjects = FindObjectsOfType<SceneLoader>().Length;
        if (numberOfObjects > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(shouldLoad)
        {
            SaveManager saveManager = gameObject.GetComponent<SaveManager>();
            saveManager.Load(saveIndex);
            shouldLoad = false;
        }
    }

    //Wczytuje kolejną scenę. Jeżeli wywoływana na ostatnim poziomie wczytywany jest poziom pierwszy.
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex != SceneManager.sceneCountInBuildSettings - 1)
            SceneManager.LoadScene(currentSceneIndex + 1);
        else
            SceneManager.LoadScene(0);
    }

    public void LoadSavedScene(int index)
    {
        if(File.Exists(Application.persistentDataPath + "/saves/" + index + ".save"))
        {
            shouldLoad = true;
            saveIndex = index;
            LoadNextScene();
        }
    }
}
