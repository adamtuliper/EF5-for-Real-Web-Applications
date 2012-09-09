//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Data.Objects;
//using System.Data;

//namespace BidTracker.Extensions
//{
//    /// <summary>
//    /// This class allows you to attach an entity.
//    /// For instance, a controller method Edit(Customer customer)
//    /// using ctx.AttachAsModified(customer); 
//    /// ctx.SaveChanges();
//    /// allows you to easily reattach this item for udpating.
//    /// Credit goes to: http://geekswithblogs.net/michelotti/archive/2009/11/27/attaching-modified-entities-in-ef-4.aspx
//    /// </summary>
//    static class EntityFrameworkExtensions
//    {
//        public static void AttachAsModified<T>(this ObjectSet<T> objectSet, T entity) where T : class
//        {
//            objectSet.Attach(entity);
//            objectSet.Context.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);
//        }

//        public static void AttachAllAsModified<T>(this ObjectSet<T> objectSet, IEnumerable<T> entities) where T : class
//        {
//            foreach (var item in entities)
//            {
//                objectSet.Attach(item);
//                objectSet.Context.ObjectStateManager.ChangeObjectState(item, EntityState.Modified);
//            }
//        }
//    }
//}