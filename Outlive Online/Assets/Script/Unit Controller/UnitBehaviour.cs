using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;

public class UnitBehaviour : MonoBehaviour
{

    public Player player;
    public Tilemap map;

    private List<PlayerCommand> commands;
    protected PlayerCommand standCommand;
    protected NavMeshAgent navMeshAgent;
    protected NavMeshObstacle navMeshObstacle;

    public static int MOVIMENT_PRIORITY = 49;
    public static int ATTACK_PRIORITY = 48;
    public static int STOPPED_PRIORITY = 50;

    public static float MOVIMENT_RADIUS = 0.5f;
    public static float ATTACK_RADIUS = 0.5f;
    public static float STOPPED_RADIUS = 0.25f;

    public string GetGUIName { get => getGUIName();}
    protected virtual string getGUIName()
    {
        return "";
    }

    private UnitBehaviour(){}
    public UnitBehaviour(Player player)
    {
        this.player = player;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Renderer>().material.SetColor("_Color", player.color);
        commands = new List<PlayerCommand>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshObstacle = GetComponent<NavMeshObstacle>();
        // mouseInput.Mouse.MouseClick.performed += _ => MouseClick();
    }

    // Update is called once per frame
    void Update()
    {
        if (standCommand is MoveCommand)
        {
            UpdateMoveCommand();
        }
        if (standCommand is AttackCommand)
        {
            UpdateAttackCommand();
        }
    }

    public virtual void putCommand(PlayerCommand command, bool enfilerate)
    {
        if(command != null)
        {
            if (enfilerate)
                commands.Add(command);
            else
            {
                stopLastCommand();
                standCommand = command;
            }
        }
        else
        {
            stopLastCommand();
            standCommand = null;
        }

        ExecuteStandCommand();
    }

    void putInteract()
    {
        putCommand(new MoveCommand(0, 0, 0), false);
    }

    protected virtual void ExecuteStandCommand()
    {
        if (standCommand is MoveCommand)
        {
            ExecuteMoveCommand((MoveCommand) standCommand);
        }
        if (standCommand is AttackCommand)
        {
            ExecuteAttackCommand((AttackCommand) standCommand);
        }
    }

    protected virtual void ExecuteMoveCommand(MoveCommand moveCommand){
        // navMeshAgent.Move(moveCommand.getCoordinates());
        Vector3Int v = Vector3Int.FloorToInt(
            new Vector3(
                x: moveCommand.getCoordinates().x, 
                y: moveCommand.getCoordinates().y, 
                z: moveCommand.getCoordinates().z)
                );

        // navMeshObstacle.enabled = false;
        // NavMesh.
        // GameObject.FindObjectOfType<NavMeshSurface>()
        // navMeshAgent.enabled = true;
        navMeshAgent.SetDestination(moveCommand.getCoordinates());
        navMeshAgent.isStopped = false;
        navMeshAgent.avoidancePriority = MOVIMENT_PRIORITY;
    }

    protected virtual void ExecuteAttackCommand(AttackCommand attackCommand){
        ExecuteMoveCommand(new MoveCommand(attackCommand.getCoordinates()));
    }

    protected virtual void UpdateMoveCommand(){
        if (navMeshAgent.enabled && navMeshAgent.remainingDistance < 0.1)
        {
            stopLastCommand();
            // navMeshAgent.autoRepath = false;
            // navMeshAgent.enabled = false;
            // navMeshObstacle.enabled = true;
            RefreshStandCommand();
        }
    }

    protected virtual void UpdateAttackCommand()
    {
        if (navMeshAgent.remainingDistance < 0.1)
        {
            stopLastCommand();
            // navMeshAgent.enabled = false;
            // navMeshObstacle.enabled = true;
            // NavMesh.
            
            RefreshStandCommand();
        }
        
    }

    ///<summary>
    ///Chamado quando o comando atual é parado
    ///</summary>
    protected virtual void stopLastCommand()
    {
        if(standCommand is MoveCommand)
        {
            // if(!navMeshObstacle.enabled)
                navMeshAgent.isStopped = true;
            navMeshAgent.avoidancePriority = STOPPED_PRIORITY;
            navMeshAgent.radius = STOPPED_RADIUS;
            // navMeshObstacle.enabled = true;
            // navMeshAgent.enabled = false;
            return;
        }
        if(standCommand is AttackCommand)
        {
            // if(!navMeshObstacle.enabled)
                navMeshAgent.isStopped = true;
            navMeshAgent.avoidancePriority = STOPPED_PRIORITY;
            navMeshAgent.radius = STOPPED_RADIUS;
            // navMeshObstacle.enabled = true;
            // navMeshAgent.enabled = false;
            return;
        }
        standCommand = null;
    }

    void RefreshStandCommand()
    {

    }

    // public string GetGuiName { get guiName;}
}
