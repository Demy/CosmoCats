using System;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum Type
    {
        Speed, Score, Objective
    }
    public const String BONUS_TAG = "Bonus";

    public Type type = Type.Score;
    public float value = 1f;
    public bool isTemporary = true;
    public float duration = 0f;
}