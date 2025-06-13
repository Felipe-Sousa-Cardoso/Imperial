using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CriaçãoDeMapa : NetworkBehaviour
{
    List<InformacoesDaCelula> lista;
    [SerializeField] GameObject CadaCélula; //Prefab de cada célula
    [SerializeField] Transform mapa;
    [SerializeField] int _altura;
    [SerializeField] int _largura;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lista = new List<InformacoesDaCelula>();
    }
    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {
            GerarMapa();
        }    
    }
    void GerarMapa()
    {
        for (int indexLargura = 0; indexLargura < _largura; indexLargura++)
        {
            for (int indexAltura = 0; indexAltura < _altura; indexAltura++)
            {
                Vector3 pos = new Vector3(indexLargura - _largura / 2, indexAltura - _altura / 2);
                GameObject obj = Instantiate(CadaCélula,pos,Quaternion.identity, mapa);
                obj.GetComponent<NetworkObject>().Spawn();
                lista.Add(
                    new InformacoesDaCelula
                    {
                        celula = obj,
                        posi = pos,
                        transponivel = indexAltura == indexLargura
                    }
                    );
            }
        }
    }
}
public class InformacoesDaCelula
{
    public GameObject celula;
    public Vector2 posi;
    public bool transponivel;
}
