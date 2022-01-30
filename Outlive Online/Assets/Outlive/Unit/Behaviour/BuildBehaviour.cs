using Outlive.Human.Generic;
using Outlive.Unit.Command;
using UnityEngine;

namespace Outlive.Unit.Behaviour
{
    [CreateAssetMenu(fileName = "BuildBehaviour", menuName = "Behaviour/Builder")]
    public class BuildBehaviour : BasicBehaviour
    {

        private GameObject gameObject;
        private IConstructorHandler constructor;
        private bool isFirstUpdate;


        public override bool Condition(ICommand command)
        {
            return command is BuildCommand;
        }

        public override void Reset()
        {
            gameObject = null;
            constructor = null;
            isFirstUpdate = true;
        }

        public override void Setup(GameObject obj)
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

        public override bool UpdateBehaviour(GameObject obj, ICommand command)
        {
            if (command is BuildCommand && obj == gameObject)
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
            return false;
        }
        public override void Cancel(GameObject obj, ICommand command)
        {
            if (command is BuildCommand && obj == gameObject)
            {
                BuildCommand buildCommand = (BuildCommand) command;

                buildCommand.constructable.ConstructorNotTryToBuild(constructor);
                constructor.DisconectConstructable(buildCommand.constructable);
            }
        }
    }
}