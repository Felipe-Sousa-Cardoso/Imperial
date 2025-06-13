using UnityEngine;

public class CelulaNoMapa : MonoBehaviour
{
    Vector2 _posição;

    public Vector2 Posição { get => _posição; set => _posição = value; }

    void Start()
    {
        gameObject.name = Posição.x + "/" + Posição.y;
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
    }
}
