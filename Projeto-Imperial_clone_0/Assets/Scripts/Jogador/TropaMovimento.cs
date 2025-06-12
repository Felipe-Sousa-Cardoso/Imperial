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
            if (transform.position.x != _destinoDeMovimentoServidor.x)
            {
                transform.position =  Vector2.MoveTowards(transform.position, new Vector2(_destinoDeMovimentoServidor.x,transform.position.y),3*Time.deltaTime);
            }
            else
            {
               if (transform.position.y != _destinoDeMovimentoServidor.y)
                {
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(_destinoDeMovimentoServidor.y, transform.position.x), 3 * Time.deltaTime);
                }
            }
            
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

    void inputFeito(InputAction.CallbackContext input)
    {
        Vector2 posicaoMouse = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(posicaoMouse, Vector2.zero);
        Debug.DrawRay(posicaoMouse, Vector2.zero, Color.green, 1);

        if (hit.collider != null && hit.collider.TryGetComponent(out CelulaNoMapa celula))
        {
            SubmeterMovimentoParaOServidorServerRpc(celula.Posição);
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
