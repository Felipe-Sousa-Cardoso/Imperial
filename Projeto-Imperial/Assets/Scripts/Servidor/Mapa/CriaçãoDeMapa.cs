using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CriaçãoDeMapa : NetworkBehaviour
{
    NetworkList<InformacoesDaCelula> lista;
    [SerializeField] GameObject CadaCélula;
    [SerializeField] int _altura;
    [SerializeField] int _largura;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lista = new NetworkList<InformacoesDaCelula>();
    }
    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {
            GerarMapa();
        }
        if (IsClient)
        {           
            IntanciarMapa();           
        }      
    }
    void GerarMapa()
    {
        for (int indexLargura = 0; indexLargura < _largura; indexLargura++)
        {
            for (int indexAltura = 0; indexAltura < _altura; indexAltura++)
            {
                Vector3 pos = new Vector3(indexLargura - _largura / 2, indexAltura - _altura / 2);
                lista.Add(
                    new InformacoesDaCelula
                    {
                        posi = pos
                    }
                    );
            }
        }
    }
    private void IntanciarMapa()
    {
        foreach (InformacoesDaCelula celula in lista)
        {
            GameObject obj = Instantiate(CadaCélula, celula.posi, Quaternion.identity);
            obj.GetComponent<CelulaNoMapa>().Posição = celula.posi;
        }
    }
}
public struct InformacoesDaCelula : INetworkSerializable, IEquatable<InformacoesDaCelula>
{
    public Vector2 posi;
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref posi);
    }
    public bool Equals(InformacoesDaCelula other)
    {
        return posi == other.posi;
    }
}
