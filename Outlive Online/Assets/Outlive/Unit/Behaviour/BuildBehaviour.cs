using System;
using Outlive.Human.Generic;
using Outlive.Unit.Command;
using UnityEngine;

namespace Outlive.Unit.Behaviour
{
    public class BuildBehaviour : IBehaviour
    {

        private GameObject gameObject;
        private IConstructorHandler constructor;
        private bool isFirstUpdate;


        public void Dispose()
        {
            gameObject = null;
            constructor = null;
            GC.SuppressFinalize(this);
        }

        public bool UpdateBehaviour(GameObject obj, ICommand command, bool cancel = false)
        {
            BuildCommand buildCommand = (BuildCommand) command;
            if (isFirstUpdate){
                constructor.ConnectConstructable(buildCommand.constructable);
                isFirstUpdate = false;
            }

            if (buildCommand.constructable.ConstructorTryToBuild(constructor))
                return true;
            else
            {
                constructor.DisconectConstructable(buildCommand.constructable);
                return false;
            }
        }

        public void ForceCancel(GameObject obj, ICommand command)
        {
            if (command is BuildCommand && obj == gameObject)
            {
                BuildCommand buildCommand = (BuildCommand) command;

                buildCommand.constructable.ConstructorNotTryToBuild(constructor);
                constructor.DisconectConstructable(buildCommand.constructable);
            }
        }

        public void Setup(GameObject obj, ICommand command)
        {
            isFirstUpdate = true;
            if (obj.TryGetComponent<IConstructorHandler>(out constructor))
            {
                gameObject = obj;
            }
            else
            {
                throw new System.Exception("O GameObject não possui um Component que herda de IConstructorHandler, tente adicionar o Script BasicConstructor");
            }
        }
    }
}