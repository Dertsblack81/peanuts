using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Utils;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Forms {
    public class DropDownModel : FormControlModel {
        private readonly IList<string> _selectedItemsValues = new List<string>();
        private readonly string _selectedItemValue;

        /// <summary>
        ///     Initialisiert eine Instanz, bei der die ToString() Methode der SelectableItems sowohl als Value, als auch als Text
        ///     f�r einen Listeneintrag verwendet werden.
        ///     Zum Beispiel bei String-Listen.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="modelMetaData"></param>
        /// <param name="propertyPath"></param>
        /// <param name="selectableItems"></param>
        /// <param name="label"></param>
        /// <param name="placeholder"></param>
        public DropDownModel(HtmlHelper htmlHelper, ModelMetadata modelMetaData, string propertyPath, IList selectableItems, string label,
            string placeholder, string lazyUrl = null)
            : base(htmlHelper, modelMetaData, propertyPath, label) {
            Require.NotNull(selectableItems);

            SelectableItems = selectableItems;
            Placeholder = placeholder;

            IsMultiSelect = TypeHelper.IsListType(modelMetaData.ModelType);
            if (IsMultiSelect) {
                /*Es k�nnen mehrere Items ausgew�hlt sein.*/

                /* !!! Muss nach dem setzen von _valueProperty aufgerufen werden !!!*/
                if (modelMetaData.Model != null) {
                    ICollection selectedItems = modelMetaData.Model as ICollection;
                    if (selectedItems != null) {
                        foreach (object selectedItem in selectedItems) {
                            _selectedItemsValues.Add(GetSelectableItemValue(selectedItem));
                        }
                    }
                }
            } else {
                /*Es kann maximal ein Item ausgew�hlt sein.*/
                /* !!! Muss nach dem setzen von _valueProperty aufgerufen werden !!!*/
                _selectedItemValue = GetSelectableItemValue(modelMetaData.Model);
            }

            LazyUrl = lazyUrl;
        }

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public DropDownModel(HtmlHelper htmlHelper, ModelMetadata modelMetaData, string propertyPath, IList selectableItems, string valuePropertyName,
            string textPropertyName, string label, string placeholder, DependingDropDownOptions dependingDropDownOptions = null)
            : base(htmlHelper, modelMetaData, propertyPath, label) {
            Require.NotNull(selectableItems);

            SelectableItems = selectableItems;
            ValuePropertyName = valuePropertyName;

            TextPropertyName = textPropertyName;
            Placeholder = placeholder;

            DependingDropDownOptions = dependingDropDownOptions;

            IsMultiSelect = TypeHelper.IsListType(modelMetaData.ModelType);
            if (IsMultiSelect) {
                /*Es k�nnen mehrere Items ausgew�hlt sein.*/

                /* !!! Muss nach dem setzen von _valueExpression aufgerufen werden !!!*/
                if (modelMetaData.Model != null) {
                    ICollection selectedItems = modelMetaData.Model as ICollection;
                    if (selectedItems != null) {
                        foreach (object selectedItem in selectedItems) {
                            _selectedItemsValues.Add(GetSelectableItemValue(selectedItem));
                        }
                    }
                }
            } else {
                /*Es kann maximal ein Item ausgew�hlt sein.*/

                /* !!! Muss nach dem setzen von _valueExpression aufgerufen werden !!!*/
                _selectedItemValue = GetSelectableItemValue(modelMetaData.Model);
            }
        }

        public DependingDropDownOptions DependingDropDownOptions {
            get; set;
        }

        /// <summary>
        ///     Ruft ab, ob in dem DropDown ein Element oder mehrere Elemente ausgew�hlt werden k�nnen.
        /// </summary>
        public bool IsMultiSelect {
            get;
        }

        /// <summary>
        ///     Ruft ab, ob in dem DropDown auch kein Element ausgew�hlt werden kann.
        ///     F�r diese Auswahl wird der Text <see cref="Placeholder" /> verwendet.
        ///     Geht nur f�r SingleSelect-DropDown.
        /// </summary>
        public bool IsNullable {
            get {
                return Placeholder != null && !IsMultiSelect;
            }
        }

        /// <summary>
        ///     Ruft die URL ab, �ber welche die ausw�hlbaren Eintr�ge nachgeladen werden k�nnen.
        /// </summary>
        public string LazyUrl {
            get; private set;
        }

        /// <summary>
        ///     Ruft den Platzhalter ab, der in der DropDown angezeigt werden soll, wenn kein Element durch den Nutzer ausgew�hlt
        ///     wurde.
        /// </summary>
        public string Placeholder {
            get;
        }

        /// <summary>
        ///     Ruft die ausw�hlbaren Werte in der DropDown ab.
        /// </summary>
        public IList SelectableItems {
            get; private set;
        }

        /// <summary>
        ///     Ruft den ausgew�hlten Wert ab.
        /// </summary>
        public object SelectedValue {
            get {
                return ModelMetaData.Model;
            }
        }

        public bool IsDepending {
            get {
                return DependingDropDownOptions != null;
            }
        }



        public string TextPropertyName {
            get; set;
        }

        public string ValuePropertyName {
            get; set;
        }

        public string GetDependsOnAttribute<TModel>(ViewDataDictionary<TModel> viewData) {
            if (DependingDropDownOptions != null) {
                return string.Format("js-depends-on={0}", DependingDropDownOptions.GetDependsOnId());
            } else {
                return "";
            }
        }

        public string GetDependingAttributeMarkup(object dependsOnValue) {
            return string.Format("js-depends-on-value={0}", dependsOnValue);

        }

        public string GetDependingAttribute(object selectableItem) {
            if (DependingDropDownOptions != null) {
                return GetDependingAttributeMarkup(DependingDropDownOptions.GetDependsOnValue(selectableItem));
            } else {
                return "";
            }
        }

        /// <summary>
        ///     Ruft ab, ob die DropDownBox eine Mehrfachauswahl zu l�sst und liefert das entsprechende Html-Attribute.
        /// </summary>
        /// <returns></returns>
        public string GetMultipleDropDown() {
            if (IsMultiSelect) {
                /*Es k�nnen mehrere Elemente ausgew�hlt werden*/
                return "multiple=\"multiple\"";
            }

            /*Es kann maximal ein Element ausgew�hlt werden*/
            return "";
        }

        /// <summary>
        ///     �berpr�ft f�r ein Ausw�hlbares Item, ob es ausgew�hlt ist.
        ///     Ist es ausgew�hlt wird das von HTML ben�tigte Keyword "selected" geliefert, andernfalls <see cref="string.Empty" />
        ///     .
        /// </summary>
        /// <param name="selectableItem"></param>
        /// <returns></returns>
        public string GetSelectableItemSelected(object selectableItem) {
            if (IsItemSelected(selectableItem)) {
                /*Das Element ist ausgew�hlt.*/
                return "selected";
            }

            /*Das Element ist nicht ausgew�hlt*/
            return string.Empty;
        }

        /// <summary>
        ///     Ruft die Zeichenfolge f�r das ausw�hlbare Element ab, die als angezeigter Text f�r das [option]-Tag verwendet wird.
        /// </summary>
        /// <param name="selectableItem">Das ausw�hlbare Element, f�r welches der Anzeigetext ermittelt werden soll.</param>
        /// <returns></returns>
        public string GetSelectableItemText(object selectableItem) {
            try {
                object value = GetText(selectableItem, TextPropertyName);
                if (value != null) {
                    return value.ToString();
                }
            } catch (Exception) {
                //
            }

            return string.Empty;
        }

        /// <summary>
        ///     Ruft den Wert f�r das ausw�hlbare Element ab, der als Schl�ssel (value) f�r das [option]-Tag verwendet wird.
        /// </summary>
        /// <param name="selectableItem">Das ausw�hlbare Element, f�r welches der Schl�ssel ermittelt werden soll.</param>
        /// <returns></returns>
        public string GetSelectableItemValue(object selectableItem) {
            try {
                object value = GetValue(selectableItem);
                if (value != null) {
                    return value.ToString();
                }
            } catch (Exception) {
                //
            }

            return string.Empty;
        }

        private static object GetText(object parent, string propertyNameOrPath) {
            if (parent == null) {
                return null;
            }

            if (propertyNameOrPath == null) {
                return parent;
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
                return GetText(propertyValue, pathStrings[1]);
            }
        }

        private object GetValue(object item) {
            if (item == null) {
                /*Null bleibt null*/
                return null;
            }

            if (string.IsNullOrWhiteSpace(ValuePropertyName)) {
                /*Kein Name f�r Value-Property angegeben. Item direkt zur�ckgeben.*/
                return item;
            }

            /*Wert anhand des PropertyName ermitteln.*/
            Type type = item.GetType();

            if (type.IsEnum) {
                /*Enum ist ein Sonderfall.*/
                return item;
            } else {
                PropertyInfo textProperty = type.GetProperty(ValuePropertyName);
                if (textProperty == null) {
                    return null;
                }

                return textProperty.GetValue(item);
            }
        }

        private bool IsItemSelected(object selectableItem) {
            if (IsMultiSelect) {
                return _selectedItemsValues.Contains(GetSelectableItemValue(selectableItem));
            }

            return GetSelectableItemValue(selectableItem) == _selectedItemValue;
        }

        public IDictionary<string, string> GetDependingPlaceholders() {
            if (DependingDropDownOptions != null) {
                return DependingDropDownOptions.GetDependingPlaceholders();
            } else {
                return new Dictionary<string, string>();
            }
        }
    }
}