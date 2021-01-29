using UnityEngine;

public class Managers : MonoBehaviour
{
    public static GameManager GameManager;
    public static ProfileManager ProfileManager;
    public static InterestManager InterestManager;
    public static EvaluationManager EvaluationManager;

    private void Awake()
    {
        GameManager = GetComponent<GameManager>();
        ProfileManager = GetComponent<ProfileManager>();
        InterestManager = GetComponent<InterestManager>();
        EvaluationManager = GetComponent<EvaluationManager>();
    }
}
