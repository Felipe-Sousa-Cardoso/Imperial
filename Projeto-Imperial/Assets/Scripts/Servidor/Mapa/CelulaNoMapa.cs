using Unity.Netcode;
using UnityEngine;

public class CelulaNoMapa : NetworkBehaviour
{
     
    [SerializeField] NetworkVariable<Vector3> _posição = new NetworkVariable<Vector3>();
    [SerializeField] NetworkVariable<bool> _transponivel = new NetworkVariable<bool>();

    public NetworkVariable<Vector3> Posição { get => _posição; set => _posição = value; }
    public NetworkVariable<bool> Transponivel { get => _transponivel; set => _transponivel = value; }

    void Start()
    {
        gameObject.name = Posição.Value.x + "/" + Posição.Value.y;
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
