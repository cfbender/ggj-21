﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public List<MatchedProfile> matches = new List<MatchedProfile>();
    public List<MatchedProfile> failures = new List<MatchedProfile>();
    public int matchCount;
    public int failureCount;
    public int maxFailures = 3;
    public int maxMatches = 10;
    public bool victory;

    public void CheckVictoryDefeat()
    {
        if (failureCount == maxFailures)
        {
            victory = false;
        }
        else if (matchCount == maxMatches)
        {
            victory = true;
        }
    }
    
    public void AddMatchedPair(Profile profile1, Profile profile2, bool failure)
    {
        var list = failure ? failures : matches;
       list.Add(new MatchedProfile(profile1, profile2)); 
       matchCount = matches.Count;
       failureCount = failures.Count;
       CheckVictoryDefeat();
    }
    
}