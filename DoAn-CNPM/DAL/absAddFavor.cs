using System;
using System.Collections.Generic;
using DoAn_CNPM.Controllers;
using System.Linq;
using System.Web;

namespace DoAn_CNPM.DAL
{
    public abstract class absAddFavor
    {
        protected ContextaddFavor _state;
        public void SetState(ContextaddFavor state)
        {
            this._state = state;
        }
        public abstract void Favor(string username, int songID);
    }
}