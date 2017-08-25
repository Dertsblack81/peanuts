using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Forms {
    /// <summary>
    /// Model f�r das View-Template zur Anzeige einer Passwort-St�rke.
    /// </summary>
    public class PasswordStrengthModel {

        public PasswordStrengthModel(HtmlHelper htmlHelper, ModelMetadata modelMetaData, string propertyPath) {
            Require.NotNull(htmlHelper, "htmlHelper");
            Require.NotNull(modelMetaData, "modelMetaData");

            HtmlHelper = htmlHelper;
            ModelMetaData = modelMetaData;
            PasswordPropertyPath = propertyPath;


            PasswordInputId = htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldId(propertyPath);

        }

        /// <summary>
        ///     Ruft den kompletten Pfad zum Passwort-Property ab inklusive des <see cref="PropertyName" />.
        /// </summary>
        public string PasswordPropertyPath {
            get; set;
        }

        /// <summary>
        /// Ruft den HTML-Helper ab.
        /// </summary>
        public HtmlHelper HtmlHelper {
            get; private set;
        }

        /// <summary>
        ///     Ruft die Id des Inputs ab, f�r welches die Passwortst�rke angezeigt werden soll.
        /// </summary>
        public string PasswordInputId {
            get; private set;
        }

        /// <summary>
        ///     Ruft die Meta-Daten f�r das Model ab.
        /// </summary>
        protected ModelMetadata ModelMetaData {
            get; set;
        }

        /// <summary>
        ///     Ruft den Pfad zum Property ab.
        /// </summary>
        public string PropertyName {
            get {
                return ModelMetaData.PropertyName;
            }
        }
    }
}