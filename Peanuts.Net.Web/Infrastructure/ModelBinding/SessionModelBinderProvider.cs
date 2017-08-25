using System;
using System.Web.Mvc;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.ModelBinding {
    public class SessionModelBinderProvider : IModelBinderProvider {
        /// <summary>
        ///     Gibt den Modellbinder f�r den angegebenen Typ zur�ck.
        /// </summary>
        /// <returns>
        ///     Der Modellbinder f�r den angegebenen Typ.
        /// </returns>
        /// <param name="modelType">Der Typ des Modells.</param>
        public IModelBinder GetBinder(Type modelType) {
            if (typeof(ISessionBindable).IsAssignableFrom(modelType)) {
                return new SessionModelBinder();
            } else {
                return null;
            }
        }
    }
}