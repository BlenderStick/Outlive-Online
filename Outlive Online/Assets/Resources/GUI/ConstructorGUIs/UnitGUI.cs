using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

///<summary>
///
///</summary>
public class UnitGUI
{

    private static SortedList<string, Action<GUIEvent>> actionInstall;
    private static SortedList<string, Action<GUIEvent>> actionUninstall;

    ///<summary>
    ///Cadastra os métodos install e uninstall para a GUI de cada UnitBehaviour, installMethod e uninstallMethod recebem um GameObject como 
    ///argumento que representa o painel onde ele está sendo instanciado
    ///
    ///<para>Quando um UnitBehaviour é selecionado pelo jogador, ele invoca installMethod para construir a interface que poderá interagir com ele</para>
    ///<para>Quando um UnitBehaviour é descelecionado, ele invoca uninstallMethod para remover a interface que foi construída</para>
    ///</summary>
    ///<param name="name"> O nome dos install e uninstall metodos</param>
    ///<param name="installMethod"> Método invocado quando um UnitBehaviour for selecionado para construir sua interface</param>
    ///<param name="uninstallMethod"> Método invocado quando um UnitBehaviour for descelecionado para remover sua interface</param>
    public static void PutMethodsToCreateGUI(string name, Action<GUIEvent> installMethod, Action<GUIEvent> uninstallMethod) 
    {
        if (installMethod == null)
            throw new ArgumentNullException(nameof(installMethod), "Não pode ser nulo.");
        if (uninstallMethod == null)
            throw new ArgumentNullException(nameof(uninstallMethod), "Não pode ser nulo.");
        
        if(actionInstall == null)
            actionInstall = new SortedList<string, Action<GUIEvent>>();
        if(actionUninstall == null)
            actionUninstall = new SortedList<string, Action<GUIEvent>>();

        actionInstall.Add(name, new Action<GUIEvent>(installMethod));
        actionUninstall.Add(name, new Action<GUIEvent>(uninstallMethod));
    }

    
    public static void InvokeInstall(string name, GameObject arg, Player player)
    {
        Action<GUIEvent> action;
        if(actionInstall != null)
            if (actionInstall.TryGetValue(name, out action))
                action.Invoke(new GUIEvent(true, arg, player));
    }
    public static void InvokeUninstall(string name, GameObject arg, Player player)
    {
        Action<GUIEvent> action;
        if(actionUninstall != null)
            if (actionUninstall.TryGetValue(name, out action))
                action.Invoke(new GUIEvent(false, arg, player));
    }

    public class GUIEvent
    {
        GameObject obj;
        bool install;
        bool uninstall;
        Player player;
        public GUIEvent(bool install, GameObject obj, Player player)
        {
            this.install = install;
            if(!install)
                uninstall = true;
            this.obj = obj;
            this.player = player;
        }

        public GameObject Obj { get => obj;}
        public bool Install { get => install;}
        public bool Uninstall { get => uninstall;}
        public Player Player { get => player;}

    }

}
