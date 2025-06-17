using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jogador : NetworkBehaviour
{
    [SerializeField] InputsEstrategia _inputs;
    [SerializeField] CelulaNoMapa _celulaSelecionada;
    [SerializeField] TropaMovimento _objetoSelecionado;
    [SerializeField] GameObject prefabTropa;
    private void Awake()
    {
        _inputs = new InputsEstrategia();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpamarTropaServerRpc();
            }
        }           
    }
   

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            _inputs.Enable();
            _inputs.GamePlay.Click.performed += inputClickFeito;
        }
    }
    private void OnDisable()
    {
        if (IsOwner && _inputs != null)
        {
            _inputs.Disable();
            _inputs.GamePlay.Click.performed -= inputClickFeito;
        }
    }
    void inputClickFeito(InputAction.CallbackContext input)
    {
        Vector2 posicaoMouse = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());//Usa o novo sistema de inputs para ler a posição do mouse

        RaycastHit2D hit = Physics2D.Raycast(posicaoMouse, Vector2.zero); //Cria um raycast entre a posição do mouse que colide com o terreno

        if (hit.collider != null && hit.collider.TryGetComponent(out CelulaNoMapa celula) && _objetoSelecionado!=null)
        {
           _celulaSelecionada = celula; 
        }
        if (hit.collider != null && hit.collider.TryGetComponent(out TropaMovimento tropa))
        {
            _objetoSelecionado = tropa;
        }
        if (_objetoSelecionado!=null && _celulaSelecionada != null)
        {
            print("movimento");
            _objetoSelecionado.DestinoDeMovimentoServidor = _celulaSelecionada.Posição.Value;
            _objetoSelecionado.AdicionarPassos();
        }
    }
    [ServerRpc]
    private void SpamarTropaServerRpc()
    {
        GameObject obj = Instantiate(prefabTropa);
        obj.GetComponent<NetworkObject>().Spawn();
    }

}
