using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jogador : NetworkBehaviour
{
    [SerializeField] InputsEstrategia _inputs;
    [SerializeField] CelulaNoMapa celulaSelecionada;
    [SerializeField] GameObject objetoSelecionado;
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
        print("sim");
        Vector2 posicaoMouse = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());//Usa o novo sistema de inputs para ler a posição do mouse

        RaycastHit2D hit = Physics2D.Raycast(posicaoMouse, Vector2.zero); //Cria um raycast entre a posição do mouse que colide com o terreno

        if (hit.collider != null && hit.collider.TryGetComponent(out CelulaNoMapa celula))
        {
            foreach(InformacoesDaCelula inf in CriaçãoDeMapa.lista)
            {
                if (inf.posi == celula.Posição.Value)
                {
                    celulaSelecionada = inf.celula;
                }
            }
            
        }
        if (hit.collider != null && hit.collider.TryGetComponent(out TropaMovimento tropa))
        {
            objetoSelecionado = tropa.gameObject;
        }
    }
}
