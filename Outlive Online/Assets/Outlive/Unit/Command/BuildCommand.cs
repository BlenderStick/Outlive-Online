using System;
using Outlive.Human.Generic;

namespace Outlive.Unit.Command
{
    public class BuildCommand : ICommand
    {
        private IConstructableHandler constructableField;

        public BuildCommand(IConstructableHandler ctr)
        {
            if (ctr == null)
                throw new ArgumentNullException();
            constructableField = ctr;
        }
        public object alvo 
        {
            get
            {
                return constructable;
            }    
        }

        public IConstructableHandler constructable
        {
            get
            {
                return constructableField;
            }
        }
    }
}