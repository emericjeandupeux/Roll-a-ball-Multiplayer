using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ChangeScene : MonoBehaviour {

    public void RestartScene()//int scene)
    {
        SceneManager.LoadScene(0);//scene);
    } 
}
