using System.Web.Mvc;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Forms {
    public class NumberInputModel : InputModel {
        /// <summary>
        /// </summary>
        /// <param name="modelMetaData"></param>
        /// <param name="htmlHelper"></param>
        /// <param name="propertyPath"></param>
        /// <param name="label">Wenn ungleich null, wird das Label �berschrieben, dass durch das MVC-Framework ermittelt wird.</param>
        /// <param name="placeholder">Der Pfad zum Property</param>
        /// <param name="minValue">kleinstm�glicher Eingabewert</param>
        /// <param name="maxValue">gr��tm�glicher Eingabewert</param>
        /// <param name="step">Schrittweite, die der Wert erh�ht oder verringert wird, wenn der Nutzer die Pfeiltasten oder das Mausrad benutzt.</param>
        public NumberInputModel(HtmlHelper htmlHelper, ModelMetadata modelMetaData, string propertyPath, string label, string placeholder, double minValue = double.MinValue, double maxValue = double.MaxValue, double? step = null)
                : base(htmlHelper, modelMetaData, propertyPath, label, placeholder) {
            MinValue = minValue;
            MaxValue = maxValue;
            Step = step;
        }

        /// <summary>
        ///     Ruft den Input-Typen ab.
        ///     Siehe http://www.w3schools.com/tags/att_input_type.asp
        /// </summary>
        public override string InputType {
            get { return "number"; }
        }

        /// <summary>
        ///     Ruft den gr��tm�glichen Wert ab, den der Nutzer eingeben kann.
        /// </summary>
        public double MaxValue { get; set; }

        /// <summary>
        ///     Ruft den kleinstm�glichen Wert ab, den der Nutzer eingeben kann.
        /// </summary>
        public double MinValue { get; set; }

        /// <summary>
        /// Ruft die Schrittweite ab, die der Wert erh�ht oder verringert wird, wenn der Nutzer die Pfeiltasten oder das Mausrad benutzt.
        /// </summary>
        public double? Step { get; set; }
    }
}