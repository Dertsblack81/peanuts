using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.ModelBinding {
    /// <summary>
    /// Persister der Methoden bereitstellt, um Werte in eine Session zu schreiben oder daraus zu lesen.
    /// </summary>
    public static class SessionPersister {

        /// <summary>
        /// Schreibt die Werte an die Session.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="values"></param>
        public static void WriteToSession(this HttpSessionStateBase session, SessionValueProvider values) {
            Require.NotNull(session, "session");
            Require.NotNull(values, "values");

            /*Alle Werte aus dem Value-Provider an die Session schreiben.*/
            foreach (string key in values.Keys) {
                ValueProviderResult valueProviderResult = values.GetValue(key);
                if (valueProviderResult != null) {
                    /*Neu gesetzten Wert an die Session schreiben*/
                    session[key] = valueProviderResult;
                }
                else {
                    /*Wenn der Wert nicht mehr gesetzt ist, dann leeren, damit keine alten Werte an der Session zur�ckbleiben*/
                    session[key] = null;
                }
            }
        }

        /// <summary>
        /// Lie�t Werte aus der Session.
        /// </summary>
        /// <param name="session">Die Session aus der gelesen werden soll.</param>
        /// <param name="bindingContext">Der BindingContext, f�r welchen die Werte aus der Session gelesen werden sollen.</param>
        public static SessionValueProvider ReadFromSession(this HttpSessionStateBase session, ModelBindingContext bindingContext) {
            IDictionary<string, ValueProviderResult> valueProviderResults = new Dictionary<string, ValueProviderResult>();

            if (session != null) {
                foreach (KeyValuePair<string, ModelMetadata> modelMetadata in bindingContext.PropertyMetadata) {
                    string key = string.Format("{0}.{1}", bindingContext.ModelName, modelMetadata.Key);
                    if (session[key] != null) {
                        /*Wert f�r das Property steht an der Session => in ValueProvider schreiben.*/
                        valueProviderResults.Add(key, session[key] as ValueProviderResult);
                    }
                }
            }

            return new SessionValueProvider(valueProviderResults);
        }

        /// <summary>
        /// L�scht Werte aus der Session, deren Key mit dem �bergebenen �bereinstimmt oder damit beginnt.
        /// </summary>
        /// <param name="session">Die Session aus der gel�scht werden soll.</param>
        /// <param name="prefix">Der Schl�ssel bzw. der Beginn der Schl�ssel die gel�scht werden sollen.</param>
        public static IList<string> RemoveFromSession(this HttpSessionStateBase session, string prefix) {
            IList<string> keysToRemove = new List<string>();

            if (session != null) {
                foreach (string sessionKey in session.Keys) {
                    if (sessionKey == prefix || sessionKey.StartsWith(sessionKey)) {
                        keysToRemove.Add(sessionKey);
                    }
                }
                foreach (string keyToRemove in keysToRemove) {
                    session[keyToRemove] = null;
                }
            }

            return keysToRemove;
        }

        /// <summary>
        /// L�scht einen Wert aus der Session, dessen Key mit dem �bergebenen �bereinstimmt.
        /// </summary>
        /// <param name="session">Die Session aus der gel�scht werden soll.</param>
        /// <param name="key">Der Schl�ssel der gel�scht werden sollen.</param>
        public static void RemoveKeyFromSession(this HttpSessionStateBase session, string key) {
            IDictionary<string, ValueProviderResult> valueProviderResults = new Dictionary<string, ValueProviderResult>();
            if (session != null) {
                foreach (string sessionKey in session.Keys) {
                    if (sessionKey == key) {
                        session[sessionKey] = null;
                    }
                }
            }
        }
    }
}