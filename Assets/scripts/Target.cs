using UnityEngine;
using System.Collections;

public enum Type {
    Player,
    Card,
    Board
}
public abstract class Target : MonoBehaviour {

    public Type type;
    public string fullName;

    void OnMouseUp()
    {
        GameManager.instance.elementClicked(this);
    }
}