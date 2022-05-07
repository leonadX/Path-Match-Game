using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static Score Instance { get; private set;}

    private int _scorePoints;
    private int currentScorePeg;

    private void Start()
    {
        currentScorePeg = 2;
    }

    private void Update()
    {
        // if score is 200 or more do something
        if(_scorePoints >= currentScorePeg)
        {
            // Do something
            //Debug.Log("You Win !!");

            //TODO! @Peter, Implement the Main Game here..

            currentScorePeg *= 2;
            TrafficManager.Instance.MoveCar();
        }
    }

    public int ScorePoints
    {
        get => _scorePoints;

        set
        {
            if (_scorePoints == value) return;

            _scorePoints = value;

            //scoreText.text($"Score = {_scorePoints}");
            scoreText.SetText($"Score = {_scorePoints}");
        }
    }

    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake() => Instance = this;
}
