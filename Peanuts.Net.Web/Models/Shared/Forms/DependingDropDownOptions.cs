using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Forms {
    /// <summary>
    /// Typisierte Ableitung f�r die Optionen eines Selects, bei dem die ausw�hlbaren Optionen von einer Vorauswahl abh�ngen. 
    /// </summary>
    /// <typeparam name="TModel">Typ des ViewModels</typeparam>
    /// <typeparam name="TDepending">Typ der von der Auswahl in einem anderen Input abh�ngig ist.</typeparam>
    /// <typeparam name="TDependsOn">Typ von dem die Auswahloptionen abh�ngig sind</typeparam>
    public class DependingDropDownOptions<TModel, TDepending, TDependsOn> : DependingDropDownOptions {

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="dependsOn">Die Id des Controls, von welcher anderen Eigenschaft des ViewModels/-commands ist diese Eigenschaft abh�ngig?</param>
        /// <param name="dependsOnProperty">Welche Eigenschaft vom Parent wird als Wert/Value verwendet?</param>
        /// <param name="dependingValues">Liste der ausw�hlbaren Werte und bei welcher ParentOption k�nnen sie ausgew�hlt werden.</param>
        /// <param name="dependingPlaceholders">Optionale Platzhalter, je nachdem was beim Parent ausgew�hlt ist.</param>
        public DependingDropDownOptions(string dependsOn, string dependsOnProperty,
            IDictionary<TDepending, TDependsOn> dependingValues, IDictionary<TDependsOn, string> dependingPlaceholders = null) {
            DependsOn = dependsOn;
            DependingValues = dependingValues;
            DependingPlaceholders = dependingPlaceholders;
            DependsOnProperty = dependsOnProperty;

            //PropertyInfo propertyInfo = typeof(TDependsOn).GetProperty(DependsOnProperty);
            //if (propertyInfo == null) {
            //    throw new ArgumentOutOfRangeException("dependsOnProperty");
            //}
        }

        /// <summary>
        /// Ruft die optionalen Platzhalter ab, die je nach Auswahl im Parent angezeigt werden.
        /// Kann Null sein, wenn keine optionalen Platzhalter angezeigt werden sollen.
        /// </summary>
        public IDictionary<TDependsOn, string> DependingPlaceholders {
            get;
        }

        /// <summary>
        /// Ruft eine Liste der ausw�hlbaren Werte und bei welcher ParentOption k�nnen sie ausgew�hlt werden ab.
        /// </summary>
        public IDictionary<TDepending, TDependsOn> DependingValues {
            get; set;
        }

        /// <summary>
        /// Ruft die HTML-Id des Controls ab, von welcher Eigenschaft des ViewModels/-commands diese Eigenschaft abh�ngig ist.
        /// </summary>
        public string DependsOn {
            get; set;
        }

        /// <summary>
        /// Ruft den Namen der Eigenschaft des Parents ab, die als Wert/Value verwendet wird.
        /// </summary>
        public string DependsOnProperty {
            get; set;
        }

        /// <summary>
        /// Ruft alle Platzhalter ab, die je nach ausgew�hltem Wert im Parent angezeigt werden.
        /// </summary>
        /// <returns></returns>
        public override IDictionary<string, string> GetDependingPlaceholders() {
            if (DependingPlaceholders == null) {
                return new Dictionary<string, string>();
            }

            return DependingPlaceholders.ToDictionary(dp => GetDependsOnValue(dp.Key).ToString(), dp => dp.Value);
        }

        /// <summary>
        /// Ruft die Html-Id des Parent-Controls ab.
        /// </summary>
        /// <returns></returns>
        public override string GetDependsOnId() {
            return DependsOn;
        }

        /// <summary>
        /// Ruft den Wert ab, f�r welchen das �bergebenen Element g�ltig ist.
        /// </summary>
        /// <param name="selectableItem"></param>
        /// <returns></returns>
        public override object GetDependsOnValue(object selectableItem) {
            if (selectableItem == null) {
                return null;
            }

            if (!(selectableItem is TDepending)) {
                return null;
            }

            TDepending dependingItem = (TDepending)selectableItem;
            TDependsOn dependingValue;
            if (!DependingValues.TryGetValue(dependingItem, out dependingValue)) {
                return null;
            }

            return GetDependsOnValue(dependingValue);
        }

        private object GetDependsOnValue(TDependsOn dependingValue) {
            if (dependingValue == null) {
                return null;
            }

            if (string.IsNullOrWhiteSpace(DependsOnProperty)) {
                /*Wenn kein Property definiert ist, welches als Wert verwendet werden soll, dann das Objekt zur�ckgeben.*/
                return dependingValue;
            }

            return GetDependsOnValue(dependingValue, DependsOnProperty);
        }

        private static object GetDependsOnValue(object parent, string propertyNameOrPath) {
            if (parent == null) {
                return null;
            }

            string[] pathStrings = propertyNameOrPath.Split(new[] { "." }, 2, StringSplitOptions.RemoveEmptyEntries);


            if (pathStrings.Length == 0) {
                return parent;
            }


            PropertyInfo propertyInfo = parent.GetType().GetProperty(pathStrings[0]);
            object propertyValue = propertyInfo.GetValue(parent);

            if (pathStrings.Length == 1) {
                return propertyValue;
            } else {
                return GetDependsOnValue(propertyValue, pathStrings[1]);
            }
        }
    }

    /// <summary>
    /// Basis-Klasse f�r die Optionen eines Selects, bei dem die ausw�hlbaren Optionen von einer Vorauswahl abh�ngen.
    /// </summary>
    /// <remarks>
    /// Die Abstrakte Basisklasse ist notwendig, da das DropDownModel/SelectModel typenlos ist.
    /// </remarks>
    public abstract class DependingDropDownOptions {

        /// <summary>
        /// Ruft die optionalen Platzhalter ab, die je nach Auswahl im Parent angezeigt werden.
        /// Kann Null sein, wenn keine optionalen Platzhalter angezeigt werden sollen.
        /// </summary>
        public abstract IDictionary<string, string> GetDependingPlaceholders();

        /// <summary>
        /// Ruft die Html-Id des Parent-Controls ab.
        /// </summary>
        public abstract string GetDependsOnId();

        /// <summary>
        /// Ruft den Wert ab, f�r welchen das �bergebenen Element g�ltig ist.
        /// </summary>
        public abstract object GetDependsOnValue(object selectableItem);
    }
}