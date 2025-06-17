using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jogador : NetworkBehaviour
{
    [SerializeField] InputsEstrategia _inputs;
    [SerializeField] NetworkObject celulaSelecionada;
    [SerializeField] CelulaNoMapa celulateste;
    [SerializeField] NetworkObject objetoSelecionado;
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
        if (IsServer)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject obj = Instantiate(prefabTropa);
                obj.GetComponent<NetworkObject>().Spawn();
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
        if (hit.collider != null && hit.collider.TryGetComponent(out CelulaNoMapa celula))
        {
            print(celula.name);
            NetworkObjectReference celularef = celula.GetComponent<NetworkObject>();
            celulateste = celula; 
        }
    }
    [ServerRpc]
    void SelecionarCelulaServerRpc(NetworkObjectReference celulaRef)
    {
        if (celulaRef.TryGet(out NetworkObject netObj))
        {
            print(netObj.name);           
            AtualizarClienteSelecionadoClientRpc(celulaRef);
        }
    }
    [ClientRpc]
    void AtualizarClienteSelecionadoClientRpc(NetworkObjectReference celulaRef)
    {
        if (celulaRef.TryGet(out NetworkObject netObj))
        {
            print(netObj.name+"ss");
            celulaSelecionada = netObj;
        }
        
    }

}
