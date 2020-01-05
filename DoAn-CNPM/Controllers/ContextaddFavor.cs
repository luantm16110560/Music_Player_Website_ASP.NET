using System;
using DoAn_CNPM.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn_CNPM.Controllers
{
    public class ContextaddFavor
    {
        private absAddFavor _addFavor = null;
        public ContextaddFavor(absAddFavor addFavor)
        {
            this.TransitionTo(addFavor);
        }
        public void TransitionTo(absAddFavor favor)
        {
            this._addFavor = favor;

        }
    }
}