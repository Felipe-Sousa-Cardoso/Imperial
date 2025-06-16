using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class movimentoWASD : NetworkBehaviour
{
    InputsClienteWASD _inputs; //Vari�vel que armazena a classe usada para os inputs do jogador
    float _velocidade = 3;
    [SerializeField] Vector2 _movimentoAtual;
    [SerializeField] Vector2 _movimentoServidor;
    Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _inputs = new InputsClienteWASD();
    }
    private void Update()
    {
         if (IsServer)
         {
             transform.position += new Vector3(_movimentoServidor.x, _movimentoServidor.y) * _velocidade * Time.deltaTime;
         }       
    }
   
    public override void OnNetworkSpawn()
    {
        if (IsOwner) //Ativa o sistema de inputs apenas para o cliente dono do objeto
        {
            _inputs.Enable();
            _inputs.Gameplay.Movimento.performed += InputMovimnetoFeito; //� um evento que � chamado quando esse input � realizado,
            _inputs.Gameplay.Movimento.canceled += InputMovimnetoCancelado;

        }     
    }
    private void OnDisable() //� chamado localmente porque os m�todos sao locais e � mais abrangente que o networkDespawn
    {
        if (IsOwner && _inputs != null)
        {
            _inputs.Gameplay.Movimento.performed -= InputMovimnetoFeito;
            _inputs.Gameplay.Movimento.canceled -= InputMovimnetoCancelado;
            _inputs.Disable();
        }
    }
    void InputMovimnetoFeito(InputAction.CallbackContext input )
    {
        _movimentoAtual = input.ReadValue<Vector2>();
        SubmeterMovimentoParaOServidorServerRpc(_movimentoAtual);
    }
    void InputMovimnetoCancelado(InputAction.CallbackContext input)
    {
        _movimentoAtual = Vector2.zero;
        SubmeterMovimentoParaOServidorServerRpc(_movimentoAtual);
    }
    [ServerRpc]
    void SubmeterMovimentoParaOServidorServerRpc(Vector2 movimentoDoCliente)
    {
        _movimentoServidor = movimentoDoCliente;
    }
}
