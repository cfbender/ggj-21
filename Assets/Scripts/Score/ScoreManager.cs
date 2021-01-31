﻿using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    public ScoreScreenControl ScoreScreenControl;

    public List<MatchedProfile> matches = new List<MatchedProfile>();
    public List<MatchedProfile> failures = new List<MatchedProfile>();
    public int matchCount;
    public int failureCount;

    public int currentStreak;
    public bool currentStreakSuccessful;
    public int longestSuccessStreak;
    public int longestMissedStreak;


    public void AddMatchedPair(Profile profile1, Profile profile2, EvaluationResult result, bool failure)
    {
        var list = failure ? failures : matches;
        list.Add(new MatchedProfile(profile1, profile2, result));
        matchCount = matches.Count;
        failureCount = failures.Count;
        GenerateScoreText();
    }

    public void UpdateStreaks(bool success)
    {
        if (success)
        {
            currentStreak = currentStreakSuccessful ? currentStreak + 1 : 1;
            currentStreakSuccessful = true;
            if (currentStreak > longestSuccessStreak)
            {
                longestSuccessStreak = currentStreak;
            }
        }
        else
        {
            currentStreak = currentStreakSuccessful ? 1 : currentStreak + 1;
            currentStreakSuccessful = false;
            if (currentStreak > longestMissedStreak)
            {
                longestMissedStreak = currentStreak;
            }
        }
    }

    public void ShowFinalScoreScreen()
    {
        ScoreScreenControl.InitializeScoreScreen();
        Debug.Log("Final score screen activated.");
    }

    public void HideFinalScoreScreen()
    {
        ScoreScreenControl.DisableScoreScreen();
    }

    private void GenerateScoreText()
    {
        ScoreText.text = $"Successful Matches: {matchCount} \n" +
                         $"Unsuccessful Matches: {failureCount}";
    }
}