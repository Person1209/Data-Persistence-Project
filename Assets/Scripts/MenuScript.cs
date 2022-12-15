using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    private Text nameField;

    // Start is called before the first frame update
    private void Start()
    {
        nameField = GameObject.Find("Canvas/NameField/Text").GetComponent<Text>();
    }

    public void StartGame()
    {
        DataManager.Instance.SetCurPlayer(nameField.text);
        SceneManager.LoadScene(1);
    }
}
