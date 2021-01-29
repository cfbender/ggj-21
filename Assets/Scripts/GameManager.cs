using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public float SelectionResetDelay = 2;
    private Profile _selectedProfile1;
    private Profile _selectedProfile2;
    private bool _allowSelection = true;

    public float ChatBubbleCooldown = 5;
    private float _chatBubbleTimer;
    private bool _chatBubbleCooldownActive;

    private void Start()
    {
        Managers.ProfileGridControl.InitializeProfileButtons();
    }

    private void Update()
    {
        HandleChatBubbles();
    }

    #region ProfileGridChatBubbles

    private void HandleChatBubbles()
    {
        if (_chatBubbleCooldownActive) // Don't display too many chat bubbles at once
        {
            _chatBubbleTimer += Time.deltaTime;

            if (_chatBubbleTimer < ChatBubbleCooldown) return;

            _chatBubbleCooldownActive = false;
        }

        ;

        if (Random.Range(0f, 1f) <= .9f) return; // small chance each frame to display a new chat bubble

        Managers.ProfileGridControl.DisplayChatBubbleForRandomProfile();
        _chatBubbleCooldownActive = true;
        _chatBubbleTimer = 0;
    }

    #endregion

    #region ProfileSelection

    public void SelectProfile(Profile profile)
    {
        if (!_allowSelection) return;

        if (_selectedProfile1 == null)
        {
            _selectedProfile1 = profile;
            Managers.ProfileGridControl.ActivateProfileButton(profile, true);
            Managers.EvaluationManager.AssignEvaluationProfile(profile, true);
            Debug.Log($"{profile.Name} set to <color=blue>_selectedProfile1</color>");
            return;
        }

        // cancel selection
        if (_selectedProfile1 == profile)
        {
            _selectedProfile1 = null;
            Managers.ProfileGridControl.ActivateProfileButton(profile, false);
            Managers.EvaluationManager.ClearEvaluationProfiles();
            Debug.Log($"{profile.Name} removed from _selectedProfile1");
            return;
        }

        _selectedProfile2 = profile;
        Managers.ProfileGridControl.ActivateProfileButton(profile, true);
        Managers.EvaluationManager.AssignEvaluationProfile(profile, false);
        Debug.Log($"{profile.Name} set to <color=teal>_selectedProfile2</color>");
        _allowSelection = false;

        StartCoroutine(DelayedReset());
    }

    private IEnumerator DelayedReset()
    {
        yield return new WaitForSeconds(SelectionResetDelay);

        // reset selected profiles and buttons
        Managers.ProfileGridControl.ActivateProfileButton(_selectedProfile1, false);
        Managers.ProfileGridControl.ActivateProfileButton(_selectedProfile2, false);

        var matches = Managers.EvaluationManager.GetMatches(_selectedProfile1, _selectedProfile2);

        Debug.Log($"{_selectedProfile1.Name} matched with {_selectedProfile2.Name} on {matches.Count()} interests.");
        Debug.Log($"Matching interests: {String.Join(", ", matches)}");

        Managers.ScoreManager.AddMatchedPair(_selectedProfile1, _selectedProfile2, matches.Count() < 3);
        Managers.ProfileGridControl.RemoveAndReplaceMatchedPair(_selectedProfile1, _selectedProfile2);

        _selectedProfile1 = null;
        _selectedProfile2 = null;

        Managers.EvaluationManager.ClearEvaluationProfiles();

        if (Managers.ScoreManager.victory == true)
        {
            Debug.Log("Victory!");
        }
        else if (Managers.ScoreManager.victory == false)
        {

            Debug.Log("You friggin dingus");
        }
        _allowSelection = true;
    }

    #endregion
}