using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class TropaMovimento : NetworkBehaviour
{
    [SerializeField] Vector2 _destinoDeMovimento;
    [SerializeField] Vector2 _destinoDeMovimentoServidor;
    InputsEstrategia _inputs;
    [SerializeField] List<Vector2> _passos = new List<Vector2>();

    float x;


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
            if (_passos.Count > 0)
            {
                if (x > 0.5f)
                {
                    transform.position = _passos[0];
                    _passos.RemoveAt(0);
                    x = 0;
                }
                else
                {
                    x += Time.deltaTime;
                }
                
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
        _passos.Clear();
        _destinoDeMovimento = transform.position;

        Vector2 posicaoMouse = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());//Usa o novo sistema de inputs para ler a posição do mouse

        RaycastHit2D hit = Physics2D.Raycast(posicaoMouse, Vector2.zero); //Cria um raycast entre a posição do mouse que colide com o terreno

        if (hit.collider != null && hit.collider.TryGetComponent(out CelulaNoMapa celula))
        {
            SubmeterMovimentoParaOServidorServerRpc(celula.Posição);
        }
        AdicionarPassos();

    }
    void inputCancelado(InputAction.CallbackContext input)
    {

    }
    [ServerRpc]
    void SubmeterMovimentoParaOServidorServerRpc(Vector2 movimentoDoCliente)
    {
        _destinoDeMovimentoServidor = movimentoDoCliente;
    }
    private void AdicionarPassos()
    {
        for (int indexPasso = 0; indexPasso < 5; indexPasso++)
        {
            if (_destinoDeMovimento.x != _destinoDeMovimentoServidor.x)
            {
                _destinoDeMovimento.x = Mathf.MoveTowards(_destinoDeMovimento.x, _destinoDeMovimentoServidor.x, 1);
                _passos.Add(_destinoDeMovimento);
                continue;
            }
            if (_destinoDeMovimento.y != _destinoDeMovimentoServidor.y)
            {
                _destinoDeMovimento.y = Mathf.MoveTowards(_destinoDeMovimento.y, _destinoDeMovimentoServidor.y, 1);
                _passos.Add(_destinoDeMovimento);
                continue;
            }
        }
    }

}
