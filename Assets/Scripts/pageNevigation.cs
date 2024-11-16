using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pageNevigation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject NextGameObject;
    [SerializeField] private GameObject CurrentGameObject;
    [SerializeField] private GameObject mode;
    public float waitTime = 5f;
    public bool disableCurrentObject = true;
    public bool game = false;
    public bool login ;
    public bool disablePanel = false;

    void Start()
    {
        
        if (PlayerPrefs.HasKey("Login")) login = true;
        else login = false;
        if (game) StartCoroutine(LoadGame());
        else StartCoroutine(LoadPage());
        if (disableCurrentObject) StartCoroutine(pageDisable(waitTime));
    }


    IEnumerator LoadPage()
    {
        yield return new WaitForSeconds(waitTime);
        if (login) mode.SetActive(true);
        else NextGameObject.SetActive(true);
        if (disableCurrentObject)
        {
            transform.gameObject.SetActive(false);
        }
        else if (disablePanel)
        {
            CurrentGameObject.SetActive(false);
        }
    }

    IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("game");
        SceneManager.LoadScene("Game");
        if (disableCurrentObject)
        {
            transform.gameObject.SetActive(false);
        }
    }
    IEnumerator pageDisable(float val)
    {
        yield return new WaitForSeconds(val);
        NextGameObject.SetActive(true);
    }
}
