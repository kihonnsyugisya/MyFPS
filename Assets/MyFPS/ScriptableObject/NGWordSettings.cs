using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

[CreateAssetMenu(menuName = "ScriptableObject/NGWords")]
public class NGWordSettings : ScriptableObject
{
    [SerializeField]
    private List<string> ngWords = new();

    public bool IsWordSafe(string input)
    {
        foreach (string ngWord in ngWords)
        {
            string escapedNgWord = Regex.Escape(ngWord); // 正規表現パターン内の特殊文字をエスケープ
            if (Regex.IsMatch(input, escapedNgWord, RegexOptions.IgnoreCase))
            {
                // NGワードが含まれていた場合、falseを返します
                return false;
            }
        }

        // NGワードが含まれていない場合、trueを返します
        return true;
    }
}
