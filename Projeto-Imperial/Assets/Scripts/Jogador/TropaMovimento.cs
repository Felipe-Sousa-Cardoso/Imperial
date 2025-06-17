using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class TropaMovimento : NetworkBehaviour
{
    [SerializeField] Vector3 _destinoDeMovimento;
    [SerializeField] Vector3 _destinoDeMovimentoServidor;
    [SerializeField] List<Vector2> _passos = new List<Vector2>();

    float x;

    public Vector3 DestinoDeMovimentoServidor { get => _destinoDeMovimentoServidor; set => _destinoDeMovimentoServidor = value; }

    void Start()
    {

    }
    private void Update()
    {
        if (IsServer)
        {
            if (_passos.Count > 0)
            {
                if (x > 0.1f)
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
    public void AdicionarPassos()
    {
        for (int indexPasso = 0; indexPasso < 5; indexPasso++)
        {
            if (_destinoDeMovimento.x != DestinoDeMovimentoServidor.x)
            {
                _destinoDeMovimento.x = Mathf.MoveTowards(_destinoDeMovimento.x, DestinoDeMovimentoServidor.x, 1);
                adicionarPassosServerRpc(_destinoDeMovimento);
                continue;
            }
            if (_destinoDeMovimento.y != DestinoDeMovimentoServidor.y)
            {
                _destinoDeMovimento.y = Mathf.MoveTowards(_destinoDeMovimento.y, DestinoDeMovimentoServidor.y, 1);
                adicionarPassosServerRpc(_destinoDeMovimento);
                continue;
            }
        }
    }
    [ServerRpc]
    void adicionarPassosServerRpc(Vector3 destino)
    {
        _passos.Add(destino);
    }

}
