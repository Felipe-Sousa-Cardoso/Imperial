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
    [SerializeField] List<Vector3> _passos = new List<Vector3>();
    LineRenderer ln;
    bool podeMover; //Usado para a confirmação do mocimento

    float x;

    public Vector3 DestinoDeMovimentoServidor { get => _destinoDeMovimentoServidor; set => _destinoDeMovimentoServidor = value; }
    public bool PodeMover { get => podeMover; set => podeMover = value; }

    void Start()
    {
        ln = GetComponent<LineRenderer>();
        _destinoDeMovimento = transform.position;
    }
    private void Update()
    {
        if (IsServer)
        {
            if (_passos.Count > 0)
            {
                if (podeMover)
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
    }
    public void AdicionarPassos()
    {
        removerPassosServerRpc();
        _passos.Clear();
        _destinoDeMovimento = transform.position;
        for (int indexPasso = 0; indexPasso < 5; indexPasso++)
        {
            if (_destinoDeMovimento.x != DestinoDeMovimentoServidor.x)
            {
                _destinoDeMovimento.x = Mathf.MoveTowards(_destinoDeMovimento.x, DestinoDeMovimentoServidor.x, 1);
                if (!IsServer)
                {
                    _passos.Add(_destinoDeMovimento); //Adiciona local e no servidor o movimento
                }              
                adicionarPassosServerRpc(_destinoDeMovimento);
                continue;
            }
            if (_destinoDeMovimento.y != DestinoDeMovimentoServidor.y)
            {
                _destinoDeMovimento.y = Mathf.MoveTowards(_destinoDeMovimento.y, DestinoDeMovimentoServidor.y, 1);
                if (!IsServer)
                {
                    _passos.Add(_destinoDeMovimento); //Adiciona local e no servidor o movimento
                }
                adicionarPassosServerRpc(_destinoDeMovimento);
                continue;
            }
        }
      
        ln.positionCount = _passos.Count; //Define o tamanho da linha como o numero de passos

        int index = 0;
        foreach(Vector3 passo in _passos)
        {
            ln.SetPosition(index, passo);
            index++;
        }
    }
    [ServerRpc]
    void adicionarPassosServerRpc(Vector3 destino)
    {
        _passos.Add(destino);
    }
    [ServerRpc]
    void removerPassosServerRpc()
    {
        _passos.Clear();
        _destinoDeMovimento = transform.position;
    }
}
