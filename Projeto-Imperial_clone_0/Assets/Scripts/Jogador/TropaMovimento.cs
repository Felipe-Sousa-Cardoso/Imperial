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
    [SerializeField] List<Vector2> _passos = new List<Vector2>();

    float x;


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
