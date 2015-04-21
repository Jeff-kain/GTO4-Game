using UnityEngine;
using System.Collections;

public class GridTile : MonoBehaviour {

    public bool Selectable = true;
    void Start ()
    {
        GridGenerator.Instance.Tiles.Add(this);
    }

    void Update()
    {

    }

}
