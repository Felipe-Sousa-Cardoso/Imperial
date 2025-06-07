using Unity.Netcode;
using UnityEngine;

public class MovimentoBasico : NetworkBehaviour
{
    private InputsCliente inputs;
    [SerializeField] float velocidade;

    private void Awake()
    {
        inputs = new InputsCliente();
        inputs.Enable();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
        }
    }
}
