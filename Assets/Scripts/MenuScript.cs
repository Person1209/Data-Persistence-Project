using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    private Text nameField;
    private Text highscores;

    private IEnumerator Load()
    {
        yield return new WaitUntil(() => DataManager.Instance);

        nameField.text = DataManager.Instance.CurPlayer.Name;

        string str = "";
        PlayerData[] data = DataManager.Instance.GetHighscores();
        for (int i = 0; i < data.Length; ++i)
            str += $"#{i + 1} {data[i]}\n";

        RectTransform content = GameObject.Find("Canvas/Highscores/Viewport/Content").GetComponent<RectTransform>();
        content.sizeDelta += new Vector2(0, data.Length * 22);

        highscores.text = str;
    }

    // Start is called before the first frame update
    private void Start()
    {
        nameField = GameObject.Find("Canvas/NameField/Text").GetComponent<Text>();
        highscores = GameObject.Find("Canvas/Highscores/Viewport/Content/Text").GetComponent<Text>();

        InputField input = GameObject.Find("Canvas/NameField").GetComponent<InputField>();
        input.onValidateInput += ValidateInput;

        StartCoroutine(Load());
    }

    public char ValidateInput(string input, int i, char added)
    {
        if (char.IsLetter(added))
            return char.ToUpper(added);
        else
            return '\0';
    }

    public void StartGame()
    {
        DataManager.Instance.SetCurPlayer(nameField.text);
        SceneManager.LoadScene(1);
    }
}
