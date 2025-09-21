using UnityEngine;
using UnityEngine.UI;

public class ValeraScore : MonoBehaviour
{
    private Text scoreText;

    private void OnEnable()
    {
        scoreText = GetComponent<Text>(); 
    }

    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
