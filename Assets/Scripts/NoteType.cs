using System;
using UnityEngine;

[System.Serializable]
public enum NoteType
{
    Tap,
    Hold,
}

[System.Serializable]
public enum NoteDir {
    Left = 0, 
    Up = 1,
    Down = 2,
    Right = 3
}

[System.Serializable]
[Flags]
public enum InputDir {
    None = 0,
    Left = 1, 
    Up = 1 << 1,
    Down = 1 << 2,
    Right = 1 << 3
}

public enum BeatHitType
{
    Miss,
    Good,
    Nice,
    Perfect
}

public static class InputDirExtensions
{
    public static bool MatchesNote(this InputDir inputDir, NoteDir dir)
    {
        return dir switch
        {
            NoteDir.Left => (inputDir & InputDir.Left) != 0,
            NoteDir.Right => (inputDir & InputDir.Right) != 0,
            NoteDir.Up => (inputDir & InputDir.Up) != 0,
            NoteDir.Down => (inputDir & InputDir.Down) != 0,
            _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
        };
    }
}
