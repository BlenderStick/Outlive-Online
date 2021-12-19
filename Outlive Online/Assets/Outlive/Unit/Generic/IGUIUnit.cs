using Outlive.Unit.Command;

namespace Outlive.Unit.Generic
{
    public interface IGUIUnit
    {
        string guiName{get;}
        void GUIReceivedFocus();
        void GUILostFocus();
    }
}