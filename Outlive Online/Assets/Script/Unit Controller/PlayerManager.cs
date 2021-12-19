using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public List<Player> players;

    public Player[] getEnemysFrom(Player player)
    {
        Player[] enemys = null;
        if(players.Contains(player))
        {
            enemys = new Player[players.Count - 2];
            int i = 0;
            foreach (Player p in players)
            {
                if(p != player)
                {
                    enemys[i] = p;
                    i++;
                }
            }
        }
        return enemys;
    }

    public void InstallUnitsInScene(GameObject obj)
    {
        foreach (Player p in players)
        {  
            Player.InstallPlayer(p, obj);
        }   
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
