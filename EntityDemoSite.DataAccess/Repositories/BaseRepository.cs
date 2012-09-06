using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityDemoSite.DataAccess.Repositories
{
    public abstract class BaseRepository : IDisposable
    {
        protected EntityContext Context { get; set; }
        protected bool RequiresContextDisposal { get; set; }
        private bool _disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    //Only if we created the internal copy of the context should we dispose.
                    //Again note I am only doing this for my web forms implementation
                    //and at that, I dont like that implementation in web forms.
                    //When you use the server controls to 'automagically' read
                    //from your data source, you lose control over object lifetime.
                    //So I don't recommend that, I'm simply showing it how it can be done
                    //as there are those that only stick to the server control behavior.
                    if (this.RequiresContextDisposal && this.Context!=null)
                    {
                        this.Context.Dispose();
                    }
                }
            }
            this._disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
