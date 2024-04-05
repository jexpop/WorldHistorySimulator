using UnityEngine;

public class SymbolTexture
{

    private string _name;
    private Texture2D _texture;

    public string Name { get { return _name; } }
    public Texture2D Texture { get { return _texture; } }


    public SymbolTexture(string name, Texture2D texture)
    {
        this._name = name;
        this._texture = texture;
    }

}