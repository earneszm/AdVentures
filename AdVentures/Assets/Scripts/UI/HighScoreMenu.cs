﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HighScoreMenu : MonoBehaviour
{
    private List<HighScoreRow> rows;

    private List<HighScoreRow> Rows
    {
        get
        {
            if (rows == null)
                rows = GetComponentsInChildren<HighScoreRow>().ToList();

            return rows;
        }
    }

    public void Show(HighScoreData scores)
    {     
        int rank = 0;
        int previousScore = 0;
        HighScoreRow row;

        for (int i = 1; i <= Rows.Count; i++)
        {
            row = Rows[i - 1];
            if (scores.records.Count < i)
            {
                row.UpdateText("");
                continue;
            }

            
            var score = scores.records[i - 1];

            if(score.Score != previousScore)
            {
                rank++;
                previousScore = score.Score;
            }

            row.UpdateText(string.Format("{0}. {1} {2}", rank, score.Name, score.Score));
        }

        gameObject.SetActive(true);
    }
}
