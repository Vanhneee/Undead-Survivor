using UnityEngine;

public class Result : MonoBehaviour
{
    public GameObject[] titles;
    public GameManager[] artists;

    public void Lose()
    {
        titles[0].SetActive(true);
    }

    public void Win()
    {
        titles[1].SetActive(true);
    }
}
