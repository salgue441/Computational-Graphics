using TMPro;
using UnityEngine;

public class ChuckNorris : MonoBehaviour
{
    public TextMeshProUGUI jokeText;

    /// <summary>
    /// Calls the Chuck Norris API and displays a joke.
    /// </summary>
    public void NewJoke()
    {
        Joke joke = APIHelper.GetNewJoke();

        jokeText.text = joke.value;
    }
}
