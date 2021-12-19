using System.Collections;
using System.Collections.Generic;
using Outlive.Unit.Command;
using UnityEngine;

public abstract class BasicBehaviour : ScriptableObject, IBehaviour
{
    public abstract bool Condition(ICommand command);
    public abstract void Reset();
    public abstract void Setup(GameObject obj);
    public abstract bool UpdateBehaviour(GameObject obj, ICommand command);

    public abstract void Cancel(GameObject obj, ICommand command);
}
