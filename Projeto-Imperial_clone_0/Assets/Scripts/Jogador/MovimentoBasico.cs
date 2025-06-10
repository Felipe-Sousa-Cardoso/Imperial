using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class MovimentoBasico : NetworkBehaviour
{
    InputsCliente _inputs; //Variável que armazena a classe usada para os inputs do jogador
    float _velocidade = 3;
    [SerializeField] Vector2 _movimentoAtual;
    [SerializeField] Vector2 _movimentoServidor;
    Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _inputs = new InputsCliente();
    }
    void Start()
    {

    }
    private void Update()
    {
         if (IsServer)
         {
             transform.position += new Vector3(_movimentoServidor.x, _movimentoServidor.y) * _velocidade * Time.deltaTime;
         }
        
    }
    private void OnDisable() //É chamado localmente porque os métodos sao locais e é mais abrangente que o networkDespawn
    {
        if (IsOwner && _inputs != null)
        {
            _inputs.Gameplay.Movimento.performed -= InputMovimnetoFeito;
            _inputs.Gameplay.Movimento.canceled -= InputMovimnetoCancelado;
            _inputs.Disable();
        }
    }
    public override void OnNetworkSpawn()
    {
        if (IsOwner) //Ativa o sistema de inputs apenas para o cliente dono do servidor
        {
            _inputs.Enable();
            _inputs.Gameplay.Movimento.performed += InputMovimnetoFeito; //é um evento que é chamado quando esse input é realizado,
            _inputs.Gameplay.Movimento.canceled += InputMovimnetoCancelado;

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
