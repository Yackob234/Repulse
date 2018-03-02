using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace repulse
{
    public class ControllerEntity : Entity
    {
        public ControllerEntity(EntityDrawData drawData, string assetName)
            : base(drawData, assetName)
        {

        }

        public virtual void AttachController(Controller controller)
        {

        }


        protected static bool attacked = false;
        protected static bool chosenDirection = false;

    }
}
