using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 必须引入这一行，才能切换场景！

public class MainMenu : MonoBehaviour
{
    // 点击“开始游戏”时调用
    public void PlayGame()
    {
        // 加载构建列表中的下一个场景
        // 比如：当前是 0 号场景，点一下就加载 1 号场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // 点击“退出游戏”时调用
    public void QuitGame()
    {
        Debug.Log("游戏已退出！"); // 在编辑器里看不到退出，所以打印一句话证明好使
        Application.Quit();
    }
}
