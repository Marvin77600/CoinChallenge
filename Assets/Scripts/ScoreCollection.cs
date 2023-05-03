using UnityEngine;

[CreateAssetMenu(menuName = "Score Collection")]
public class ScoreCollection : ScriptableObject
{
    public int Health;

    public float TimeElapsed;

    public int Points;

    public bool IsDead;

    public void Save(int _health, float _timeElapsed, int _points, bool _isDead)
    {
        Health = _health;
        TimeElapsed = _timeElapsed;
        Points = _points;
        IsDead = _isDead;
    }

    public override string ToString()
    {
        string str1 = IsDead ? "Mince, tu as perdu !" : "Bien joué, tu viens de finir le jeu !";
        return $"{str1}\n\n" +
            $"Tu as ramassé {Points} points en {(int)TimeElapsed} secondes.";
    }
}