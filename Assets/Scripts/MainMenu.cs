using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/*
 * Created by Kubo Brehuv 30.7.2018.
 */
public class MainMenu : MonoBehaviour {

    public Text scoreText;

    public void Start()
    {
        Debug.Log("wtf");
        scoreText.text = "Best score: " + PlayerPrefs.GetInt("Score").ToString();
    }


    public void OnButtonClick(string sceneName)
    {
        Debug.Log(sceneName);

        SceneManager.LoadScene(sceneName);
    }
}
