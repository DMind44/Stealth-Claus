using System;
using UnityEngine;

[System.Serializable]
public abstract class EntityAction
{
    public abstract string ActionName { get; }
}

[System.Serializable]
public class MoveAction : EntityAction
{
    public override string ActionName => "Move";
    public int dx;
    public int dy;
    public int distance;
    public int speed;
}


[System.Serializable]
public class WaitAction : EntityAction
{
    public override string ActionName => "Wait";
    public int duration;
}