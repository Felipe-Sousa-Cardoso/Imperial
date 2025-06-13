using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class TropaMovimento : NetworkBehaviour
{
    [SerializeField] Vector2 _destinoDeMovimento;
    [SerializeField] Vector2 _destinoDeMovimentoServidor;
    InputsEstrategia _inputs;


    private void Awake()
    {
        _inputs = new InputsEstrategia();
    }
    void Start()
    {

    }
    private void Update()
    {
        if (IsServer)
        {
            Movimento();
            
        }
    }



    public override void OnNetworkSpawn()
    {
        _inputs.Enable();
        _inputs.GamePlay.Click.performed += inputFeito;
    }

    private void OnDisable()
    {
        _inputs.Disable();
        _inputs.GamePlay.Click.performed -= inputFeito;
    }
    private void Movimento()
    {
        transform.position = Vector3.MoveTowards(transform.position, _destinoDeMovimentoServidor, Time.deltaTime * 5);
    }
    void inputFeito(InputAction.CallbackContext input)
    {
        Vector2 posicaoMouse = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(posicaoMouse, Vector2.zero);
        Debug.DrawRay(posicaoMouse, Vector2.zero, Color.green, 1);

        if (hit.collider != null && hit.collider.TryGetComponent(out CelulaNoMapa celula))
        {
            SubmeterMovimentoParaOServidorServerRpc(celula.Posi��o);
        }
    }
    void inputCancelado(InputAction.CallbackContext input)
    {

    }
    [ServerRpc]
    void SubmeterMovimentoParaOServidorServerRpc(Vector2 movimentoDoCliente)
    {
        _destinoDeMovimentoServidor = movimentoDoCliente;
    }
}
