using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CriaçãoDeMapa : NetworkBehaviour
{
    public static List<InformacoesDaCelula> lista;
    [SerializeField] GameObject CadaCélula; //Prefab de cada célula
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
                GameObject obj = Instantiate(CadaCélula,pos,Quaternion.identity, transform);
                //Servidor
                obj.GetComponent<NetworkObject>().Spawn();
                CelulaNoMapa celula = obj.GetComponent<CelulaNoMapa>();
                lista.Add(
                    new InformacoesDaCelula
                    {
                        celula = celula,
                        posi = pos,
                        transponivel = indexAltura == indexLargura
                    }
                    );
                //InformaçõesDacélula              
                celula.Posição.Value = pos;
                celula.Transponivel.Value = indexAltura == indexLargura;
            }
        }
    }
}
public class InformacoesDaCelula
{
    public CelulaNoMapa celula;
    public Vector3 posi;
    public bool transponivel;
}
