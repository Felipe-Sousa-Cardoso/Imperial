using Unity.Netcode;
using UnityEngine;

public class CriaçãoDeMapa : NetworkBehaviour
{
    [SerializeField] GameObject CadaCélula;
    [SerializeField] int _altura;
    [SerializeField] int _largura;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
                GameObject obj = Instantiate(CadaCélula, new Vector3(indexLargura - _largura / 2, indexAltura - _altura / 2), Quaternion.identity, transform);
                obj.name = $"Tile {indexLargura},{indexAltura}";

                NetworkObject networkObject = obj.GetComponent<NetworkObject>();
                if (networkObject != null)
                {
                    networkObject.Spawn();
                }

            }
        }
    }
    
}
