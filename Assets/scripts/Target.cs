using UnityEngine;
using System.Collections;

public enum Type {
    Player,
    Card
}
public abstract class Target : MonoBehaviour {

    public Type type;
    public string fullName;
}