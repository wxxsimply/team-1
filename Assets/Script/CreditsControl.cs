using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsControl : MonoBehaviour
{
    // 这里的 "StartMenu" 必须和你开始界面场景的名字一模一样！
    public void BackToMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}