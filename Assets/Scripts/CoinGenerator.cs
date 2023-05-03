using UnityEngine;

namespace Assets.Scripts
{
    public class CoinGenerator : MonoBehaviour
    {
        [SerializeField] int count;
        [SerializeField] Vector3 soPos, nePos;
        [SerializeField]
        GameObject simpleCoin, rareCoin, superRareCoin;

        void Start()
        {
            SpawnCoins();
        }

        void SpawnCoins()
        {
            for (int i = 0; i < count; i++)
            {
                int x = (int)Random.Range(soPos.x, nePos.x + 1);
                int z = (int)Random.Range(soPos.z, nePos.z + 1);

                int random = Random.Range(1, 101);

                if (random > 0 && random <= 20)
                {
                    Instantiate(superRareCoin, new Vector3(x, 50, z), Quaternion.identity);
                }
                else if (random > 20 && random <= 50)
                {
                    Instantiate(rareCoin, new Vector3(x, 50, z), Quaternion.identity);
                }
                else
                {
                    Instantiate(simpleCoin, new Vector3(x, 50, z), Quaternion.identity);
                }
            }
        }
    }
}