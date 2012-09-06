using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Validation;
using System.Diagnostics;
using EntityDemoSite.DataAccess.Cache;
using EntityDemoSite.DataAccess.Interfaces;
using EntityDemoSite.Domain;
using EntityDemoSite.Domain.Entities;
using EntityDemoSite.Domain.Interfaces;
namespace EntityDemoSite.DataAccess.Repositories
{
    /// <summary>
    /// Note we are not using IDisposable because there are no resources required that support it.
    /// </summary>
    public class StateRepository : BaseRepository, IStateRepository
    {
        private ICacheProvider _cache { get; set; }
        private static readonly object CacheLockObject = new object();

        /// <summary>
        /// The parameterless constructor is used by ObjectDataSource
        /// </summary>
        public StateRepository(ICacheProvider cache)
        {
            _cache = cache;
        }

        public StateRepository()
        {
            _cache = new DefaultCacheProvider();
            this.Context = new EntityContext();
            this.RequiresContextDisposal = true;
        }

        
        public IEnumerable<State> GetAll()
        {
            
            // Check the cache
            IEnumerable<State> states = _cache.Get("StatesGetAll") as IEnumerable<State>;

            if (states == null)
            {
                lock (CacheLockObject)
                {
                    //make sure between the get and now it wasnt added.
                    states = _cache.Get("StatesGetAll") as IEnumerable<State>;
                    if (states == null)
                    {
                        //definitely doesn't exist. add it to the cache.
                        states = new State[] { new State() { StateId = "PA" }, new State() { StateId = "NY" } };
                        _cache.Set("StatesGetAll", states, 60);
                    }
                }
            }
            return states;
        }

    }
}
