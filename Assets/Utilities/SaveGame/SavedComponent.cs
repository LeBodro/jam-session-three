using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SavedComponent : MonoBehaviour
{
    protected virtual void Awake() => SaveGame.Register(this);
    protected virtual void OnDestroy() => SaveGame.Unregister(this);
    public abstract string Serialize();
    public abstract SavedComponent Deserialize(string content);
}
